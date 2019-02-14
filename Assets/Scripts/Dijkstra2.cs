using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

public class Dijkstra2 : MonoBehaviour{
    private BattleCardWar battleCardWar;
	//private Text debugText;
	private int TotalNodeCount;
	
	void Start(){
        battleCardWar = gameObject.GetComponent<BattleCardWar> ();
        //debugText = battleCardWar.debugText;
	}
	
	
	public struct Results
	{
		public int[] MinimumPath;
		public float[] MinimumDistance;
		public Results(int[] minimumPath, float[] minimumDistance)
		{
			MinimumDistance = minimumDistance;
			MinimumPath = minimumPath;
		}
	}

	public Results Perform(int start)
	{
		float[] d = GetStartingTraversalCost(start);
		int[] p = GetStartingBestPath(start);
		BasicHeap Q = new BasicHeap();
		
        //debugText.text += "\n<color=#800000>Starting Traversal Cost from node 0</color>";
		
        for (int i = 0; i != TotalNodeCount; i++) {
			
            Q.Push (i, d [i]); 
		
            if(d[i] != Mathf.Infinity){
				//debugText.text += "\n<color=#0000ff>node "+i+" = "+d[i]+"</color>";
			}
			else{
				//debugText.text += "\n<color=#0000ff>node "+i+" = "+d[i]+"</color>";
			}
		}

		//debugText.text += "\n<color=#800000>Start Main Loop</color>";
		
        while (Q.Count > 0)
		{
			int v = Q.Pop();
			
            //debugText.text += "\n<color=blue>[---CEK NODE "+v+"---] \n> Remaining Q = "+Q.Count+"] : </color> ";
			
            for(int i = 0; i < Q.Count; i++){
				//debugText.text += "\nnode "+Q.getIndex(i)+" : ";
				//debugText.text +="cost : "+Q.getWight(i);
			}
			
            //debugText.text += "\n<color=blue>> Cek Nearby dari node "+v+ " : </color>";
			
            foreach (int w in battleCardWar.GetNearbyNodes(v))
			{
				//debugText.text += "\nnode "+v+" > node "+w;
				
                float cost = battleCardWar.getTraversalCost(v, w);
                battleCardWar.markCurrentPath(v, w);
				
                //debugText.text += ", cost = "+cost;
				
                if (d[v] + cost < d[w])
				{
					//debugText.text += "\nTravecost dari "+v+" > "+w+ " = "+(d[v]+cost)+"\n<color=#660066>Lebih kecil dari cost yg sekarang = "+d[w]+"</color>";
                    d[w] = d[v] + cost;
					//debugText.text +="\n> Set cost : dr node sebelumnya > "+v+" > "+w+" = "+d[w];
					//debugText.text += "\n> Set best path menuju "+w+" dari = "+p[w]+" menjadi = "+v;
					p[w] = v;
					//debugText.text += ">\nPush index = "+w+", weight = "+d[w]+" ke Q untuk dicek selanjutnya";
					Q.Push(w, d[w]);
					//debugText.text += "\njadi Q = "+Q.Count;
				}
                else if(d[v] + cost == d[w]){
					//debugText.text += "\nTravecost dari "+v+" > "+w+ " = "+(d[v]+cost)+"\n<color=#006600>Harga yang sama dari cost yg sekarang = "+d[w]+"</color>";
				}
                else if(d[v] + cost > d[w]){
					//debugText.text += "\nTravecost dari start > "+v+" > "+w+ " = "+(d[v]+cost)+"\n<color=#006600>Harga yang lebih besar dari cost yg sekarang = "+d[w]+"</color>";
				}
			}
		}
		
		//debugText.text += "\n<color=#800000>Node hasil perhitungan</color>";
		
        for (int i = 0; i < p.Length; i++) {
			//debugText.text += "\nStart > node "+i+" melalui node "+p[i];
		}
		
        //debugText.text += "\n<color=#800000>Distance dari start ke masing2 node</color>";
		
        for (int i = 0; i < d.Length; i++) {
			//debugText.text += "\n> Start > "+i+" = "+d[i];
		}
		
        return new Results(p, d);
	}

	public int[] GetMinimumPath(int start, int finish)
	{ 
        TotalNodeCount = battleCardWar.NumNodes;
		Results results = Perform(start);
		
        //debugText.text += "\n<color=#800000>Step menuju start point</color>";
		
        int[] x =  GetMinimumPath(start, finish, results.MinimumPath);
		
        //debugText.text += "\n<color=#800000>Node optimal Path</color>";
		
        for (int i = 0; i < x.Length; i++) {
			//debugText.text += "\n> "+x[i];
		}
		return x;
	}

	private int[] GetMinimumPath(int start, int finish, int[] shortestPath)
	{
		foreach (int i in shortestPath) {
			//debugText.text += "\n> sortestpath "+i;
		}
		
        Stack<int> path = new Stack<int>();

		do
		{
			//debugText.text += "\n<color=blue>> "+finish+"</color>";
			
            path.Push(finish);
			finish = shortestPath[finish];
		}
		while (finish != start);
		
        //debugText.text += "\n<color=blue>> "+finish+"</color>";
		
        path.Push (finish);

		return path.ToArray();
	} 

	private int[] GetStartingBestPath(int startingNode)
	{
		int[] p = new int[TotalNodeCount];
		
        for (int i = 0; i < p.Length; i++)
			p[i] = startingNode;
		
        return p;
	}

	private float[] GetStartingTraversalCost(int start)
	{
		float[] subset = new float[TotalNodeCount];
		
        for (int i = 0; i != subset.Length; i++) {
			subset [i] = Mathf.Infinity;
		}
		subset[start] = 0;
		
        foreach (int nearby in battleCardWar.GetNearbyNodes(start)) {
            subset [nearby] = battleCardWar.getTraversalCost (start, nearby);
		}
		return subset;
	}
}