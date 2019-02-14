using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public string system_message_ = "";
    static int nodeName = 0;

    public TrackingObject arrow0_;
    public TrackingObject arrow1_;
    public TrackingObject arrow2_;
    public TrackingObject arrow3_;
    public TrackingObject arrow4_;
    public TrackingObject arrow5_;
    public TrackingObject arrow6_;
    public TrackingObject arrow7_;

    void OnGUI()
    {
        GUIStyle gui_style_btn = new GUIStyle("Button");
        gui_style_btn.fontSize = 50;

        GUIStyle gui_style = new GUIStyle();
        gui_style.fontSize = 40;
        gui_style.normal.textColor = Color.yellow;

        GUI.Label(new Rect(100, 150, 200, 60), system_message_, gui_style);
       
        system_message_ = "가장 가까운 마커를 촬영해주세요";

        if (arrow0_.is_detected_)
        {
            nodeName = int.Parse(arrow0_.name);
          
            system_message_ = "현재 위치가 확인되었습니다";
            if (GUI.Button(new Rect(150, 450, 450, 150), "목적지 선택하기", gui_style_btn))
            {
                SceneManager.LoadScene("3_Pathfinding");
            }

        }
        else if (arrow1_.is_detected_)
        {
            nodeName = int.Parse(arrow1_.name);

            system_message_ = "현재 위치가 확인되었습니다";
            if (GUI.Button(new Rect(150, 450, 450, 150), "목적지 선택하기", gui_style_btn))
            {
                SceneManager.LoadScene("3_Pathfinding");
            }
        }

        else if (arrow2_.is_detected_)
        {
            nodeName = int.Parse(arrow2_.name);

            system_message_ = "현재 위치가 확인되었습니다";
            if (GUI.Button(new Rect(150, 450, 450, 150), "목적지 선택하기", gui_style_btn))
            {
                SceneManager.LoadScene("3_Pathfinding");
            }
        }

        else if (arrow3_.is_detected_)
        {
            nodeName = int.Parse(arrow3_.name);

            system_message_ = "현재 위치가 확인되었습니다";
            if (GUI.Button(new Rect(150, 450, 450, 150), "목적지 선택하기", gui_style_btn))
            {
                SceneManager.LoadScene("3_Pathfinding");
            }

        }
        else if (arrow4_.is_detected_)
        {
            nodeName = int.Parse(arrow4_.name);

            system_message_ = "현재 위치가 확인되었습니다";
            if (GUI.Button(new Rect(150, 450, 450, 150), "목적지 선택하기", gui_style_btn))
            {
                SceneManager.LoadScene("3_Pathfinding");
            }
        }
        else if (arrow5_.is_detected_)
        {
            nodeName = int.Parse(arrow5_.name);

            system_message_ = "현재 위치가 확인되었습니다";
            if (GUI.Button(new Rect(150, 450, 450, 150), "목적지 선택하기", gui_style_btn))
            {
                SceneManager.LoadScene("3_Pathfinding");
            }
        }

        else if (arrow6_.is_detected_)
        {
            nodeName = int.Parse(arrow6_.name);

            system_message_ = "현재 위치가 확인되었습니다";
            if (GUI.Button(new Rect(150, 450, 450, 150), "목적지 선택하기", gui_style_btn))
            {
                SceneManager.LoadScene("3_Pathfinding");
            }
        }
        else if (arrow7_.is_detected_)
        {
            system_message_ = "현재 위치가 확인되었습니다";
            if (GUI.Button(new Rect(150, 450, 450, 150), "목적지 선택하기", gui_style_btn))
            {
                SceneManager.LoadScene("5_StarCity");
            }
        }
    }
	
    void OnDisable()
	{
         PlayerPrefs.SetInt("nodeName", nodeName);
	}
}
