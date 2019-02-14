using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class NodeConfigurator : MonoBehaviour
{
	public int NumNodes; //노드개수
	public Transform[] Nodes; //노드 객체
	public bool[,]  connectionExists; //노드들끼리 연결이 되어 있는지 안되어 있는지
	float[,]  distances; //노드들 간의 거리
	public LineRenderer[] line; //엣지 객체
	public GameObject[] lineHolder;
	int NumEdges; //엣지개수
	public int a = 0, b = 0; //출발지, 목적지
	public Dijkstra dijkstra;
	public Material lineMaterial, highlightMaterial, traveledMaterial;
	public Text debugText;

    public Text guideText1;
    public Text guideText2;

    static int[] path; //알고리즘으로 얻어진 최종 최단경로
	static float distanceTraveled; //알고리즘으로 얻어진 최종 거리 비용
	
    string pathString = "";

    public Ray ray;
    public RaycastHit hitInfo;

    int nodeName = 0;

    //출발지전달받음
	private void OnEnable()
	{
        nodeName = PlayerPrefs.GetInt("nodeName");
        Debug.Log("OnEnable()");
        Debug.Log(nodeName);
	}

    //경로보기 버튼
	public void RunPathfinder()
    {
        Debug.Log("RunPathfinder()");

        //최단 경로
		path = runDijkstra ();
        Debug.Log(path);

        //최단 경로를 화면에 표시하는 함수 실행
        StartCoroutine(markPath (path));
		
        //최단경로의 거리
        distanceTraveled = getTotalDistance (path);
        Debug.Log(distanceTraveled);

        //최단경로의 노드
        pathString = getPathString(path);
        Debug.Log(pathString);

        //debugText.text += "\n<color=#800000>=== Summary ===</color>";

    
        //최단 경로의 총 비용
        //debugText.text += "\nDistance to end : " + distanceTraveled.ToString("F2");
        guideText1.text = "총 이동거리는: " + distanceTraveled.ToString("F2")+" 입니다. /n";

        //최단 경로 노드 출력
        //debugText.text +=  "\n" + pathString;
        guideText2.text += "촬영해야 할 마커 순서는: " + pathString+ " 입니다. /n";
	}	

    //출발지 다시 설정 버튼
    public void NavigationToMain(){
        SceneManager.LoadScene("2_StartScene");
    }

    //최단 경로 값 전달
    void OnDisable()
    {
        PlayerPrefs.SetInt("a", a);
        PlayerPrefs.SetInt("b", b);
        PlayerPrefs.SetFloat("distanceTraveled", distanceTraveled);
    }

    //네비게이션 시작 버튼
    public void NavigationToNaviScene()
    {
        SceneManager.LoadScene("4_NaviScene");
    }

	void Start () 
	{
        //총 노드 개수
		NumNodes = Nodes.Length;

        Debug.Log(NumNodes);

        initializeNodes();	//노드 초기화
		initializeLines();  //엣지 초기화
	}

    //목적지 선택
    void endClicked()
    {
        Debug.Log("endClicked()");
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                b = int.Parse(hitInfo.transform.gameObject.name);

                Nodes[b].GetComponent<Renderer>().material.color = Color.red;
                Debug.Log(b);

                var CurrentCubeText = Nodes[b].Find("New Text").GetComponent(typeof(TextMesh)) as TextMesh;
                CurrentCubeText.text = "End";
                CurrentCubeText.fontSize = 17;
            }
        }
    }
    
    void Update ()
	{
        Debug.Log("Update()");

		connectNodes();
        
        switch (nodeName)
        {
            case 0:
                a = 0;
                break;
            case 1:
                a = 1;
                break;
            case 2:
                a = 2;
                break;
            case 3:
                a = 3;
                break;
            case 4:
                a = 4;
                break;
            case 5:
                a = 5;
                break;
            case 6:
                a = 6;
                break;
        }

        //출발지 노드 UI 변경
        Nodes[a].GetComponent<Renderer>().material.color = Color.red;
        var CurrentCubeText = Nodes[a].Find("New Text").GetComponent(typeof(TextMesh)) as TextMesh;
        CurrentCubeText.text = "Start";
        CurrentCubeText.fontSize = 17;
        Debug.Log(a);

        //목적지 클릭 함수 실행
        endClicked();
	}
	
    //노드들 초기화
	void initializeNodes()
	{
        Debug.Log("initializeNodes()");

        //노드들끼리 연결 되어 있는지: 0 또는 1
		connectionExists = new bool[NumNodes,NumNodes];
        //노드들 사이의 거리
		distances = new float[NumNodes,NumNodes];

        //노드 개수만큼 큐브 이름 가져오기
		for (int i = 0; i < NumNodes; i++)
		{
			var CurrentCubeText = Nodes[i].Find("New Text").GetComponent(typeof(TextMesh)) as TextMesh;
			CurrentCubeText.text = ""+i;
			Nodes[i].gameObject.name = i.ToString();
		}
		NumEdges = determineConnections();
		//debugText.text += "\nJumlah Path : "+NumEdges;
	}
	
    //엣지 초기화 함수
	void initializeLines()
	{
        Debug.Log("initializeLines()");

		line = new LineRenderer[NumEdges];
		lineHolder = new GameObject[NumEdges];

		for (int i = 0; i < NumEdges; i++)
		{
            //경로찾기 수행전에는 보이지 않던 경로 그리기
			line[i] = new LineRenderer();
			lineHolder[i] = new GameObject();
			line[i] = lineHolder[i].AddComponent<LineRenderer>();
			line[i].SetVertexCount(2);
			line[i].SetWidth(.3F, .3F);
			line[i].material = lineMaterial;

			line[i].enabled = true;
		}
	}	

	void connectNodes()
	{
        Debug.Log("connectNodes()");

		Vector3 nodeToCheck, neighbor;
		int currentLine = 0;

		for (int i = 0; i < NumNodes; i++)
		{
			nodeToCheck = Nodes [i].gameObject.transform.position;
			
            for (int j = 0; j < NumNodes; j++)
			{
				neighbor = Nodes [j].gameObject.transform.position;
				
                //node i와 node j가 연결되어 있다면
                if (connectionExists[i,j])
				{
					line[currentLine].SetPosition (0, nodeToCheck);
					line[currentLine].SetPosition (1, neighbor);
					lineHolder[currentLine].name = "Line from " + i + " to " + j;
					currentLine++;
				} 
			}
		}
	}
	
    //경로, 노드, 이웃노드, 노드 연결
	int determineConnections()
	{
        Debug.Log("determineConnections()");

		float distance;
		Vector3 nodeToCheck, neighbor;
		int counter = 0; 

		// -------------------- Node 0 -> 1
		nodeToCheck = Nodes [0].gameObject.transform.position;
		neighbor = Nodes [1].gameObject.transform.position;
		distance = Vector3.Distance(nodeToCheck, neighbor);
        Debug.Log(distance);
//		debugText.text += "\nDistance : "+0+" > "+1+" = "+distance;
		declareConnection(0,1,distance);	
		counter++;
		// -------------------- Node 0 -> 2
		nodeToCheck = Nodes [0].gameObject.transform.position;
		neighbor = Nodes [2].gameObject.transform.position;
		distance = Vector3.Distance(nodeToCheck, neighbor);
        Debug.Log(distance);
//		debugText.text += "\nDistance : "+0+" > "+2+" = "+distance;
		declareConnection(0,2,distance);					
		counter++;
		// -------------------- Node 2 -> 3
		nodeToCheck = Nodes [2].gameObject.transform.position;
		neighbor = Nodes [3].gameObject.transform.position;
		distance = Vector3.Distance(nodeToCheck, neighbor);
        Debug.Log(distance);
//		debugText.text += "\nDistance : "+2+" > "+3+" = "+distance;
		declareConnection(2,3,distance);					
		counter++;
		// -------------------- Node 2 -> 4
		nodeToCheck = Nodes [2].gameObject.transform.position;
		neighbor = Nodes [4].gameObject.transform.position;
		distance = Vector3.Distance(nodeToCheck, neighbor);
        Debug.Log(distance);
//		debugText.text += "\nDistance : "+2+" > "+4+" = "+distance;
		declareConnection(2,4,distance);
		counter++;
		// -------------------- Node 1 -> 5
		nodeToCheck = Nodes [1].gameObject.transform.position;
		neighbor = Nodes [5].gameObject.transform.position;
		distance = Vector3.Distance(nodeToCheck, neighbor);
        Debug.Log(distance);
//		debugText.text += "\nDistance : "+1+" > "+5+" = "+distance;
		declareConnection(1,5,distance);					
		counter++;
		// -------------------- Node 4 -> 6
		nodeToCheck = Nodes [4].gameObject.transform.position;
		neighbor = Nodes [6].gameObject.transform.position;
		distance = Vector3.Distance(nodeToCheck, neighbor);
        Debug.Log(distance);
//		debugText.text += "\nDistance : "+4+" > "+6+" = "+distance;
		declareConnection(4,6,distance);					
		counter++;
		// -------------------- Node 5 -> 5
		nodeToCheck = Nodes [5].gameObject.transform.position;
		neighbor = Nodes [6].gameObject.transform.position;
		distance = Vector3.Distance(nodeToCheck, neighbor);
        Debug.Log(distance);
//		debugText.text += "\nDistance : "+5+" > "+6+" = "+distance;
		declareConnection(5,6,distance);					
		counter++;
		return counter;
	}
	
    //연결되었다고 선언 및 연결된 노드들 사이의 경로 비용 저장
	void declareConnection(int i, int j, float distance)
	{
        Debug.Log("declareconnection()");
		connectionExists[i,j] = true;
		distances[i,j] = distance;
	}
	
    //경로의 엣지를 회색으로 표시
	IEnumerator markPath(int[] path){
        
        Debug.Log("markPath()");

        foreach (GameObject o in lineHolder) {
		}
		
        Vector3 nodeToCheck, neighbor;
		
        debugText.text += "\n<color=#800000>Tandai Path</color>";
		
        for (int i = 0; i < path.Length; i++) {
			debugText.text += "\nnode to check : "+path[i];
			

            nodeToCheck = Nodes[path[i]].gameObject.transform.position;
			
            for(int j = 0; j < path.Length; j++){

               
				neighbor = Nodes[path[j]].gameObject.transform.position;
				
                debugText.text += "\ncek : "+path[i]+" + "+path[j];
				if (connectionExists[path[i],path[j]])
				{
					string holderName = "Line from " + path[i] + " to " + path[j];
					
                    debugText.text += "\nTandai path "+path[i] +" > "+path[j];
					
                    for(int k = 0; k < lineHolder.Length; k++){
						if(lineHolder[k].name == holderName){
							lineHolder[k].SetActive(true);
							line[k].material = highlightMaterial;

							yield return new WaitForSeconds(1f);
						}
					}
				}
			}
		}
	}

    //연결된 엣지들을 회색으로 표시
	public void markCurrentPath(int startNode, int endNode){
        Debug.Log("markCurrentPath()");

        string holderName = "Line from " + startNode + " to " + endNode;
		
        for(int k = 0; k < lineHolder.Length; k++){
			if(lineHolder[k].name == holderName){
				line[k].material = traveledMaterial;
			}
		}
	}
	
    //최단 경로의 문자열 얻기
	public string getPathString(int[] path)
	{
		string pathText = "";

		for (int j = 0; j < path.Length; j++)
		{
			if (j == 0)
				pathText = "Path : Start ";
			else if (j != path.Length-1)
				pathText = pathText + " -> " + path[j];
			else			
				pathText = pathText + " -> " + "End";
		}
		
		return pathText;
	}
	
    //최단 경로의 총 거리비용 얻기
	public float getTotalDistance(int[] nodesTraveled)
	{
		float total = 0;

		for(int i = 0; i < nodesTraveled.Length; i++)
		{
			if(i != nodesTraveled.Length - 1)
			{
				total += getTraversalCost(nodesTraveled[i],nodesTraveled[i+1]);
			}
		}
		return total;
	}
	
	public float getTraversalCost(int start, int neighbor)
	{
		if (connectionExists [start, neighbor])
			return distances [start, neighbor];
        
		else if (connectionExists[neighbor, start])
			return distances[neighbor, start];
        
		else return Mathf.Infinity; 
	}
	
    // 최단경로 찾기 알고리즘 수행
	public int[] runDijkstra()
	{
        Debug.Log("runDijkstra()");

		dijkstra = gameObject.GetComponent<Dijkstra> ();
		
        //start node(a)에서 end node(b)까지 알고리즘으로 얻은 최단 경로
        int[] dijkstraPaths = dijkstra.GetMinimumPath(a, b);
		
        //최종 경로
        debugText.text += "\n<color=#800000>Final Path</color>";
		for (int i = 0; i < dijkstraPaths.Length; i++) {
			debugText.text +="\n<color=blue>> "+dijkstraPaths[i]+"</color>";
		}
		return dijkstraPaths;
	}

    //인접 노드 얻기
	public IEnumerable<int> GetNearbyNodes(int startingNode)
	{
		List<int> nearbyNodes = new List<int>();

		for (int j = 0; j < NumNodes; j++)
		{
			if(connectionExists[startingNode,j] || connectionExists[j,startingNode])
			{
				nearbyNodes.Add(j);
			}
		}
		return nearbyNodes;
	}
}


