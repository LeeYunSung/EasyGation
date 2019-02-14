using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;


public enum eGameState
{
    Ready = 0,
    Battle,
    Result
}

public class BattleCardWar : MonoBehaviour
{
    //Object Definition
    public TrackingObject obj_lancekun_;
  
    //public TrackingObject trackingObject;

    public TrackingObject arrow1_;
    public TrackingObject arrow2_;
    public TrackingObject arrow3_;
    public TrackingObject arrow4_;
    public TrackingObject arrow5_;
    public TrackingObject arrow6_;
    public TrackingObject arrow7_;

    public eGameState game_state_ = eGameState.Ready;

    public string system_message_ = "";
    public string system_message2_ = "";
    public string system_message3_ = "";
    public string system_message4_ = "";


    //Algorithm Definition
    public Dijkstra2 dijkstra;
    public DefaultTrackableEventHandler2 defaultTrackableEventHandler2;

    public static int a , b , c , d ; //출발지, 목적지, 현재위치,촬영해야할 다음노드
    public string aText, bText, cText, dText;
    public float distanceTraveled = 0;
    public int[] path;

    public int NumNodes; //노드개수
    public Transform[] Nodes; //노드 객체
    public bool[,] connectionExists; //노드들끼리 연결이 되어 있는지 안되어 있는지
    float[,] distances; //노드들 간의 거리
    public LineRenderer[] line; //엣지 객체
    public GameObject[] lineHolder;
    int NumEdges; //엣지개수
    public Material lineMaterial, highlightMaterial, traveledMaterial, currentMaterial;
    public Material crocodileM, turtleM, birdM, monkeyM, graftM, penguinM, elephantM;
    public string pathString;
    //List<GameObject> GameObjects = new List<GameObject>();

    //유니티 함수 실행 순서
    //https://docs.unity3d.com/kr/530/Manual/ExecutionOrder.html
    //Unity Android Accelerometer Input
    //https://www.youtube.com/watch?v=HIduNSjAQjU

    private bool isFirstLoad = true;

    //1.
    private void OnEnable()
    {
        a = PlayerPrefs.GetInt("a");
        b = PlayerPrefs.GetInt("b");

        if (a == 0)
        {
            aText = "악어";
        }
        else if (a == 1)
        {
            aText = "거북이";
        }
        else if (a == 2)
        {
            aText = "새";
        }
        else if (a == 3)
        {
            aText = "원숭이";
        }
        else if (a == 4)
        {
            aText = "기린";
        }
        else if (a == 5)
        {
            aText = "펭귄";
        }
        else if (a == 6)
        {
            aText = "코끼리";
        }
        if (b == 0)
        {
            bText = "악어";
        }
        else if (b == 1)
        {
            bText = "거북이";
        }
        else if (b == 2)
        {
            bText = "새";
        }
        else if (b == 3)
        {
            bText = "원숭이";
        }
        else if (b == 4)
        {
            bText = "기린";
        }
        else if (b == 5)
        {
            bText = "펭귄";
        }
        else if (b == 6)
        {
            bText = "코끼리";
        }

        distanceTraveled = PlayerPrefs.GetFloat("distanceTraveled");
    }

    //2.
    public void Start()
    {
        NumNodes = Nodes.Length;
        initializeNodes();    //노드 초기화
        initializeLines();  //엣지 초기화
    }

    void Update()
    {
       // Debug.Log("Update()");
        connectNodes();

        //a = 0;
        //b = 3;
        Connector.a = a;
        Connector.b = b;


        //출발지 노드 UI 변경
        Nodes[a].GetComponent<Renderer>().material.color = Color.red;
        var CurrentCubeText = Nodes[a].Find("New Text").GetComponent(typeof(TextMesh)) as TextMesh;
        CurrentCubeText.text = "Start";
        CurrentCubeText.fontSize = 17;

        //목적지 노도 UI 변경
        Nodes[b].GetComponent<Renderer>().material.color = Color.red;
        CurrentCubeText = Nodes[b].Find("New Text").GetComponent(typeof(TextMesh)) as TextMesh;
        CurrentCubeText.text = "End";
        CurrentCubeText.fontSize = 17;

        RunPathFinder();
    }


    public void RunPathFinder(){
        //최단 경로
        path = runDijkstra(a,b);
        d = path[1];//최단경로의 2번째                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               초기화

        if (isFirstLoad)
        {
            Connector.d = d;
            isFirstLoad = false;
            Debug.Log("d : " + d);
        }

        //최단 경로를 화면에 표시하는 함수 실행
        markPath(path);

        //최단경로의 거리
        distanceTraveled = getTotalDistance(path);

        //최단경로의 노드
        pathString = getPathString(path);
       

    }


    //노드들 초기화
    void initializeNodes()
    {
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
    }
    
    //엣지 초기화 함수
    void initializeLines()
    {
        line = new LineRenderer[NumEdges];
        lineHolder = new GameObject[NumEdges];
        
        for (int i = 0; i < NumEdges; i++)
        {
            //경로찾기 수행 전에는 보이지 않던 경로 그리기
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
        Vector3 nodeToCheck, neighbor;
        int currentLine = 0;

        for (int i = 0; i < NumNodes; i++)
        {
            nodeToCheck = Nodes[i].gameObject.transform.position;

            for (int j = 0; j < NumNodes; j++)
            {
                neighbor = Nodes[j].gameObject.transform.position;

                //node i와 node j가 연결되어 있다면
                if (connectionExists[i, j])
                {
                    line[currentLine].SetPosition(0, nodeToCheck);
                    line[currentLine].SetPosition(1, neighbor);
                    lineHolder[currentLine].name = "Line from " + i + " to " + j;
                    currentLine++;
                }
            }
        }
    }

    //경로, 노드, 이웃노드, 노드 연결
    int determineConnections()
    {
        float distance;
        Vector3 nodeToCheck, neighbor;
        int counter = 0;

        // -------------------- Node 0 -> 1
        nodeToCheck = Nodes[0].gameObject.transform.position;
        neighbor = Nodes[1].gameObject.transform.position;
        distance = Vector3.Distance(nodeToCheck, neighbor);
        declareConnection(0, 1, distance);
        counter++;

        // -------------------- Node 0 -> 2
        nodeToCheck = Nodes[0].gameObject.transform.position;
        neighbor = Nodes[2].gameObject.transform.position;
        distance = Vector3.Distance(nodeToCheck, neighbor);
        declareConnection(0, 2, distance);
        counter++;

        // -------------------- Node 2 -> 3
        nodeToCheck = Nodes[2].gameObject.transform.position;
        neighbor = Nodes[3].gameObject.transform.position;
        distance = Vector3.Distance(nodeToCheck, neighbor);
        declareConnection(2, 3, distance);
        counter++;

        // -------------------- Node 2 -> 4
        nodeToCheck = Nodes[2].gameObject.transform.position;
        neighbor = Nodes[4].gameObject.transform.position;
        distance = Vector3.Distance(nodeToCheck, neighbor);
     
        declareConnection(2, 4, distance);
        counter++;

        // -------------------- Node 1 -> 5
        nodeToCheck = Nodes[1].gameObject.transform.position;
        neighbor = Nodes[5].gameObject.transform.position;
        distance = Vector3.Distance(nodeToCheck, neighbor);
     
        declareConnection(1, 5, distance);
        counter++;

        // -------------------- Node 4 -> 6
        nodeToCheck = Nodes[4].gameObject.transform.position;
        neighbor = Nodes[6].gameObject.transform.position;
        distance = Vector3.Distance(nodeToCheck, neighbor);

        declareConnection(4, 6, distance);
        counter++;

        // -------------------- Node 5 -> 5
        nodeToCheck = Nodes[5].gameObject.transform.position;
        neighbor = Nodes[6].gameObject.transform.position;
        distance = Vector3.Distance(nodeToCheck, neighbor);
      
        declareConnection(5, 6, distance);
        counter++;

        return counter;
    }

    //연결되었다고 선언 및 연결된 노드들 사이의 경로 비용 저장
    void declareConnection(int i, int j, float distance)
    {
        connectionExists[i, j] = true;
        distances[i, j] = distance;
    }

    //최단경로의 엣지를 빨강색으로 표시
    void markPath(int[] path)
    {
        foreach (GameObject o in lineHolder)
        {
        }

        Vector3 nodeToCheck, neighbor;

        for (int i = 0; i < path.Length; i++)
        {
            
            nodeToCheck = Nodes[path[i]].gameObject.transform.position;

            for (int j = 0; j < path.Length; j++)
            {
                neighbor = Nodes[path[j]].gameObject.transform.position;
               
                if (connectionExists[path[i], path[j]])
                {
                    string holderName = "Line from " + path[i] + " to " + path[j];


                    for (int k = 0; k < lineHolder.Length; k++)
                    {
                        if (lineHolder[k].name == holderName)
                        {
                            lineHolder[k].SetActive(true);
                            line[k].material = highlightMaterial;


                        }
                    }
                }
            }
        }
    }

    //경로의 엣지를 회색으로 표시
    public void markCurrentPath(int startNode, int endNode)
    {
       
        string holderName = "Line from " + startNode + " to " + endNode;

        for (int k = 0; k < lineHolder.Length; k++)
        {
            if (lineHolder[k].name == holderName)
            {
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
            
            else if (j != path.Length - 1)
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

        for (int i = 0; i < nodesTraveled.Length; i++)
        {
            if (i != nodesTraveled.Length - 1)
            {
                total += getTraversalCost(nodesTraveled[i], nodesTraveled[i + 1]);
            }
        }
        return total;
    }

    public float getTraversalCost(int start, int neighbor){
           

        if (connectionExists[start, neighbor])
            return distances[start, neighbor];

        else if (connectionExists[neighbor, start])
            return distances[neighbor, start];

        else return Mathf.Infinity;
    }

   
    //인접 노드 얻기
    public IEnumerable<int> GetNearbyNodes(int startingNode)
    {
  
        List<int> nearbyNodes = new List<int>();

        for (int j = 0; j < NumNodes; j++)
        {
            if (connectionExists[startingNode, j] || connectionExists[j, startingNode])
            {
                nearbyNodes.Add(j);
            }
        }
        return nearbyNodes;
    }

    //3.
    void OnGUI()
    {
        GUIStyle gui_style = new GUIStyle();
        gui_style.fontSize = 40;
        gui_style.normal.textColor = Color.yellow;

        GUI.Label(new Rect(65, 150, 200, 60), system_message_, gui_style);
        GUI.Label(new Rect(65, 200, 200, 60), system_message2_, gui_style);
        GUI.Label(new Rect(65, 250, 200, 60), system_message3_, gui_style);
        //GUI.Label(new Rect(65, 300, 200, 60), system_message4_, gui_style);

        GUIStyle gui_style_btn = new GUIStyle("Button");

        gui_style_btn.fontSize = 50;
        int check_num = 0;


        //trackingObject = gameObject.GetComponent<TrackingObject>();


        if (game_state_ == eGameState.Ready)
        {

            system_message_ = a + " 마커를 촬영해주세요";
            system_message2_ = "총 이동해야 할 거리: " + distanceTraveled;
            system_message3_ = "";
            game_state_ = eGameState.Battle;
        }

        //경로에 있으면 맞다. 경로에 없으면 틀렸다.
        if (game_state_ == eGameState.Battle)
        {

            if (arrow1_.is_detected_)
            {
                c = 0;
                Connector.c = c;
                Nodes[c].GetComponent<Renderer>().material = currentMaterial;
                check_num = 0;

                if (c == b)
                {
                    system_message_ = "목적지에 도착하였습니다";
                    system_message2_ = "";
                    system_message3_ = "";

                    obj_lancekun_.obj_animation_.Play("Attack");
                    game_state_ = eGameState.Result;
                }

                else if (c != b)
                {
                    for (int i = 0; i < path.Length; i++)
                    {//0, 1, 2

                        if (c == path[i])
                        {
                            if (path[i + 1] == 0)
                            {
                                dText = "악어";
                            }
                            else if (path[i + 1] == 1)
                            {
                                dText = "거북이";
                            }
                            else if (path[i + 1] == 2)
                            {
                                dText = "새";
                            }
                            else if (path[i + 1] == 3)
                            {
                                dText = "원숭이";
                            }
                            else if (path[i + 1] == 4)
                            {
                                dText = "기린";
                            }
                            else if (path[i + 1] == 5)
                            {
                                dText = "펭귄";
                            }
                            else if (path[i + 1] == 6)
                            {
                                dText = "코끼리";
                            }
                            system_message_ = "올바른 길로 가고 있습니다";
                            system_message2_ = "다음 " + dText + " 마커를 촬영해주세요";
                            system_message3_ = "다음 마커까지 남은거리: " + distances[path[i], path[i + 1]];
                            d = path[i + 1];
                            Connector.d = d;

                            obj_lancekun_.obj_animation_.Play("Walk");

                            break;
                        }
                        check_num++;//1,2,3
                    }

                    if (check_num == path.Length)
                    {

                        system_message_ = "잘못된 경로 입니다";
                        system_message2_ = "현재 위치에서 경로 재 탐색을 실행합니다";
                        system_message3_ = "";
                        obj_lancekun_.obj_animation_.Play("Idle");

                        if (GUI.Button(new Rect(150, 450, 450, 150), "경로 재탐색", gui_style_btn))
                        {
                            Nodes[c].GetComponent<Renderer>().material = crocodileM;
                            a = c;
                            Connector.a = a;

                            SceneManager.LoadScene("4_NaviScene");
                        }
                    }
                }
            }

            if (arrow2_.is_detected_)
            {
                c = 2;
                Connector.c = c;
                check_num = 0;
                Nodes[c].GetComponent<Renderer>().material = currentMaterial;

                if (c == b)
                {
                    system_message_ = "목적지에 도착하였습니다";
                    system_message2_ = "";
                    system_message3_ = "";
                    obj_lancekun_.obj_animation_.Play("Attack");
                    game_state_ = eGameState.Result;
                }
                else if (c != b)
                {

                    for (int i = 0; i < path.Length; i++)//0,1,2
                    {

                        if (c == path[i])
                        {
                            if (path[i + 1] == 0)
                            {
                                dText = "악어";
                            }
                            else if (path[i + 1] == 1)
                            {
                                dText = "거북이";
                            }
                            else if (path[i + 1] == 2)
                            {
                                dText = "새";
                            }
                            else if (path[i + 1] == 3)
                            {
                                dText = "원숭이";
                            }
                            else if (path[i + 1] == 4)
                            {
                                dText = "기린";
                            }
                            else if (path[i + 1] == 5)
                            {
                                dText = "펭귄";
                            }
                            else if (path[i + 1] == 6)
                            {
                                dText = "코끼리";
                            }

                            if (c == path[i])
                            {

                                system_message_ = "올바른 길로 가고 있습니다";
                                system_message2_ = "다음 " + dText + " 마커를 촬영해주세요";
                                system_message3_ = "다음 마커까지 남은거리: " + distances[path[i], path[i + 1]];
                                d = path[i + 1];
                                Connector.d = d;

                                obj_lancekun_.obj_animation_.Play("Walk");

                                break;
                            }
                            check_num++;
                        }

                        if (check_num == path.Length)
                        {

                            system_message_ = "잘못된 경로 입니다";
                            system_message2_ = "현재 위치에서 경로 재 탐색을 실행합니다";
                            system_message3_ = "";
                            obj_lancekun_.obj_animation_.Play("Idle");

                            if (GUI.Button(new Rect(150, 450, 450, 150), "경로 재탐색", gui_style_btn))
                            {

                                Nodes[c].GetComponent<Renderer>().material = crocodileM;
                                a = c;
                                Connector.a = a;

                                SceneManager.LoadScene("4_NaviScene");
                            }
                        }
                    }
                }

                if (arrow3_.is_detected_)
                {
                    c = 4;
                    Connector.c = c;
                    check_num = 0;

                    Nodes[c].GetComponent<Renderer>().material = currentMaterial;


                    if (c == b)
                    {
                        system_message_ = "목적지에 도착하였습니다";
                        system_message2_ = "";
                        system_message3_ = "";
                        obj_lancekun_.obj_animation_.Play("Attack");
                        game_state_ = eGameState.Result;
                    }

                    else if (c != b)
                    {
                        for (int i = 0; i < path.Length; i++)
                        {
                            if (c == path[i])
                            {
                                if (path[i + 1] == 0)
                                {
                                    dText = "악어";
                                }
                                else if (path[i + 1] == 1)
                                {
                                    dText = "거북이";
                                }
                                else if (path[i + 1] == 2)
                                {
                                    dText = "새";
                                }
                                else if (path[i + 1] == 3)
                                {
                                    dText = "원숭이";
                                }
                                else if (path[i + 1] == 4)
                                {
                                    dText = "기린";
                                }
                                else if (path[i + 1] == 5)
                                {
                                    dText = "펭귄";
                                }
                                else if (path[i + 1] == 6)
                                {
                                    dText = "코끼리";
                                }
                                system_message_ = "올바른 길로 가고 있습니다";
                                system_message2_ = "다음 " + dText + " 마커를 촬영해주세요";
                                system_message3_ = "다음 마커까지 남은거리: " + distances[path[i], path[i + 1]];

                                d = path[i + 1];
                                Connector.d = d;


                                obj_lancekun_.obj_animation_.Play("Walk");
                                break;
                            }
                            check_num++;
                        }
                    }

                    if (check_num == path.Length)
                    {
                        Debug.Log("잘못된경로");
                        system_message_ = "잘못된 경로 입니다";
                        system_message2_ = "현재 위치에서 경로 재탐색을 실행합니다";
                        system_message3_ = "";

                        obj_lancekun_.obj_animation_.Play("Idle");

                        if (GUI.Button(new Rect(150, 450, 450, 150), "경로 재탐색", gui_style_btn))
                        {

                            Nodes[c].GetComponent<Renderer>().material = graftM;
                            a = c;
                            Connector.a = a;

                            SceneManager.LoadScene("4_NaviScene");
                        };


                    }
                }

                if (arrow4_.is_detected_)
                {
                    c = 6;
                    Connector.c = c;
                    check_num = 0;

                    Nodes[c].GetComponent<Renderer>().material = currentMaterial;

                    if (c == b)
                    {
                        system_message_ = "목적지에 도착하였습니다";
                        system_message2_ = "";
                        system_message3_ = "";
                        obj_lancekun_.obj_animation_.Play("Attack");
                        game_state_ = eGameState.Result;
                    }

                    else if (c != b)
                    {
                        for (int i = 0; i < path.Length; i++)
                        {
                            if (c == path[i])
                            {

                                if (path[i + 1] == 0)
                                {
                                    dText = "악어";
                                }
                                else if (path[i + 1] == 1)
                                {
                                    dText = "거북이";
                                }
                                else if (path[i + 1] == 2)
                                {
                                    dText = "새";
                                }
                                else if (path[i + 1] == 3)
                                {
                                    dText = "원숭이";
                                }
                                else if (path[i + 1] == 4)
                                {
                                    dText = "기린";
                                }
                                else if (path[i + 1] == 5)
                                {
                                    dText = "펭귄";
                                }
                                else if (path[i + 1] == 6)
                                {
                                    dText = "코끼리";
                                }
                                //Debug.Log("올바른경로");
                                system_message_ = "올바른 길로 가고 있습니다";
                                system_message2_ = "다음 " + dText + " 마커를 촬영해주세요";
                                system_message3_ = "다음 마커까지 남은거리: " + distances[path[i], path[i + 1]];
                                d = path[i + 1];
                                Connector.d = d;


                                obj_lancekun_.obj_animation_.Play("Walk");
                                break;
                            }
                            check_num++;
                        }

                        if (check_num == path.Length)
                        {
                            system_message_ = "잘못된 경로 입니다";
                            system_message2_ = "현재 위치에서 경로 재탐색을 실행합니다";
                            system_message3_ = "";

                            obj_lancekun_.obj_animation_.Play("Idle");

                            if (GUI.Button(new Rect(150, 450, 450, 150), "경로 재탐색", gui_style_btn))
                            {
                                Nodes[c].GetComponent<Renderer>().material = elephantM;
                                a = c;
                                Connector.a = a;
                                SceneManager.LoadScene("4_NaviScene");
                            }
                        }
                    }

                }


                if (arrow5_.is_detected_)
                {
                    c = 3;
                    Connector.c = c;
                    check_num = 0;
                    Nodes[c].GetComponent<Renderer>().material = currentMaterial;

                    if (c == b)
                    {
                        system_message_ = "목적지에 도착하였습니다";
                        system_message2_ = "";
                        system_message3_ = "";
                        obj_lancekun_.obj_animation_.Play("Attack");
                        game_state_ = eGameState.Result;
                    }

                    else if (c != b)
                    {
                        for (int i = 0; i < path.Length; i++)
                        {
                            if (c == path[i])
                            {
                                if (path[i + 1] == 0)
                                {
                                    dText = "악어";
                                }
                                else if (path[i + 1] == 1)
                                {
                                    dText = "거북이";
                                }
                                else if (path[i + 1] == 2)
                                {
                                    dText = "새";
                                }
                                else if (path[i + 1] == 3)
                                {
                                    dText = "원숭이";
                                }
                                else if (path[i + 1] == 4)
                                {
                                    dText = "기린";
                                }
                                else if (path[i + 1] == 5)
                                {
                                    dText = "펭귄";
                                }
                                else if (path[i + 1] == 6)
                                {
                                    dText = "코끼리";
                                }
                                system_message_ = "올바른 길로 가고 있습니다";
                                system_message2_ = "다음 " + dText + " 마커를 촬영해주세요";
                                system_message3_ = "다음 마커까지 남은거리: " + distances[path[i], path[i + 1]];

                                d = path[i + 1];
                                Connector.d = d;


                                obj_lancekun_.obj_animation_.Play("Walk");
                                break;
                            }
                            check_num++;
                        }

                        if (check_num == path.Length)
                        {
                            system_message_ = "잘못된 경로 입니다";
                            system_message2_ = "현재 위치에서 경로 재탐색을 실행합니다";
                            system_message3_ = "";

                            obj_lancekun_.obj_animation_.Play("Idle");

                            if (GUI.Button(new Rect(150, 450, 450, 150), "경로 재탐색", gui_style_btn))
                            {

                                Nodes[c].GetComponent<Renderer>().material = monkeyM;
                                a = c;
                                Connector.a = a;
                                SceneManager.LoadScene("4_NaviScene");
                            }
                        }
                    }
                }

                if (arrow6_.is_detected_)
                {
                    c = 1;
                    Connector.c = c;
                    check_num = 0;
                    Nodes[c].GetComponent<Renderer>().material = currentMaterial;

                    if (c == b)
                    {
                        system_message_ = "목적지에 도착하였습니다";
                        system_message2_ = "";
                        system_message3_ = "";
                        obj_lancekun_.obj_animation_.Play("Attack");
                        game_state_ = eGameState.Result;
                    }

                    else if (c != b)
                    {
                        for (int i = 0; i < path.Length; i++)
                        {
                            if (c == path[i])
                            {
                                if (path[i + 1] == 0)
                                {
                                    dText = "악어";
                                }
                                else if (path[i + 1] == 1)
                                {
                                    dText = "거북이";
                                }
                                else if (path[i + 1] == 2)
                                {
                                    dText = "새";
                                }
                                else if (path[i + 1] == 3)
                                {
                                    dText = "원숭이";
                                }
                                else if (path[i + 1] == 4)
                                {
                                    dText = "기린";
                                }
                                else if (path[i + 1] == 5)
                                {
                                    dText = "펭귄";
                                }
                                else if (path[i + 1] == 6)
                                {
                                    dText = "코끼리";
                                }
                                system_message_ = "올바른 길로 가고 있습니다";
                                system_message2_ = "다음 " + dText + " 마커를 촬영해주세요";
                                system_message3_ = "다음 마커까지 남은거리: " + distances[path[i], path[i + 1]];

                                d = path[i + 1];
                                Connector.d = d;

                                obj_lancekun_.obj_animation_.Play("Walk");
                                break;
                            }
                            check_num++;
                        }
                        if (check_num == path.Length)
                        {
                            system_message_ = "잘못된 경로 입니다";
                            system_message2_ = "현재 위치에서 경로 재탐색을 실행합니다";
                            system_message3_ = ""; system_message3_ = "현재 위치에서 경로 재탐색을 실행합니다";

                            obj_lancekun_.obj_animation_.Play("Idle");

                            if (GUI.Button(new Rect(150, 450, 450, 150), "경로 재탐색", gui_style_btn))
                            {

                                Nodes[c].GetComponent<Renderer>().material = turtleM;
                                a = c;
                                Connector.a = a;

                                SceneManager.LoadScene("4_NaviScene");
                            }
                        }
                    }
                }

                if (arrow7_.is_detected_)
                {
                    c = 5;
                    Connector.c = c;
                    check_num = 0;
                    Nodes[c].GetComponent<Renderer>().material = currentMaterial;

                    if (c == b)
                    {
                        system_message_ = "목적지에 도착하였습니다";
                        system_message2_ = "";
                        system_message3_ = "";
                        obj_lancekun_.obj_animation_.Play("Attack");
                        game_state_ = eGameState.Result;
                    }

                    else if (c != b)
                    {
                        for (int i = 0; i < path.Length; i++)
                        {
                            if (c == path[i])
                            {
                                if (path[i + 1] == 0)
                                {
                                    dText = "악어";
                                }
                                else if (path[i + 1] == 1)
                                {
                                    dText = "거북이";
                                }
                                else if (path[i + 1] == 2)
                                {
                                    dText = "새";
                                }
                                else if (path[i + 1] == 3)
                                {
                                    dText = "원숭이";
                                }
                                else if (path[i + 1] == 4)
                                {
                                    dText = "기린";
                                }
                                else if (path[i + 1] == 5)
                                {
                                    dText = "펭귄";
                                }
                                else if (path[i + 1] == 6)
                                {
                                    dText = "코끼리";
                                }
                                system_message_ = "올바른 길로 가고 있습니다";
                                system_message2_ = "다음 " + dText + " 마커를 촬영해주세요";
                                system_message3_ = "다음 마커까지 남은거리: " + distances[path[i], path[i + 1]];

                                d = path[i + 1];
                                Connector.d = d;


                                obj_lancekun_.obj_animation_.Play("Walk");

                                break;
                            }
                            check_num++;
                        }

                        if (check_num == path.Length)
                        {
                            system_message_ = "잘못된 경로 입니다";
                            system_message2_ = "현재 위치에서 경로 재탐색을 실행합니다";
                            system_message3_ = "";

                            obj_lancekun_.obj_animation_.Play("Idle");

                            if (GUI.Button(new Rect(150, 450, 450, 150), "경로 재탐색", gui_style_btn))
                            {

                                Nodes[c].GetComponent<Renderer>().material = penguinM;
                                a = c;
                                Connector.a = a;

                                SceneManager.LoadScene("4_NaviScene");
                            }
                        }
                    }
                }
            }

            if (game_state_ == eGameState.Result)
            {
                if (GUI.Button(new Rect(150, 450, 450, 150), "처음으로", gui_style_btn))
                {
                    game_state_ = eGameState.Ready;

                    SceneManager.LoadScene("1_main");
                }
            }
        }
    }
    public int[] runDijkstra(int a, int b)
    {
        dijkstra = gameObject.GetComponent<Dijkstra2>();

        //start node(a)에서 end node(b)까지 알고리즘으로 얻은 최단 경로
        int[] dijkstraPaths = dijkstra.GetMinimumPath(a, b);

        return dijkstraPaths;

    }
}