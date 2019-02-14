using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarCityNavi : MonoBehaviour {

    //Object Definition
    public TrackingObject obj_lancekun_;

    //public TrackingObject trackingObject;

    public TrackingObject arrow0_;
    public TrackingObject arrow1_;
    public TrackingObject arrow2_;
    public TrackingObject arrow3_;
    public TrackingObject arrow4_;
    public TrackingObject arrow5_;
    //public TrackingObject arrow6_;

    public eGameState game_state_ = eGameState.Ready;
    public string system_message_ = "";
    public string system_message2_ = "";
    public string system_message3_ = "";
  
    void OnGUI()
    {
        GUIStyle gui_style_btn = new GUIStyle("Button");
        gui_style_btn.fontSize = 50;

        GUIStyle gui_style = new GUIStyle();
        gui_style.fontSize = 40;
        gui_style.normal.textColor = Color.yellow;

        GUI.Box(new Rect(70, 130, 200, 60),system_message_, gui_style);
        GUI.Box(new Rect(70, 180, 200, 60), system_message2_, gui_style);
        GUI.Box(new Rect(70, 2300, 200, 60), system_message3_, gui_style);

        //GUI.Label(new Rect(70, 150, 200, 60), system_message_, gui_style);
        //GUI.Label(new Rect(70, 200, 200, 60), system_message2_, gui_style);
        //GUI.Label(new Rect(70, 250, 200, 60), system_message3_, gui_style);

        if (game_state_ == eGameState.Ready)
        {
            system_message_ = "FOLDER 마커를 촬영해주세요";
            system_message2_ = "총 이동해야 할 거리: 57.5m";
            system_message3_ = "";

            if (arrow1_.is_detected_)
            {
                game_state_ = eGameState.Battle;
            }

        }
        if (game_state_ == eGameState.Battle)
        {
            if (arrow0_.is_detected_)
            {
                system_message_ = "올바른 길로 가고 있습니다";
                system_message2_ = "다음 버거킹 마커를 촬영해주세요";
                system_message3_ = "다음 마커까지 남은거리: 5.3m";

                obj_lancekun_.obj_animation_.Play("Walk");
            }
            else if (arrow1_.is_detected_)
            {
                system_message_ = "올바른 길로 가고 있습니다";
                system_message2_ = "다음 Buru Judy 마커를 촬영해주세요";
                system_message3_ = "다음 마커까지 남은거리: 7.2m";

                obj_lancekun_.obj_animation_.Play("Walk");
            }
            else if (arrow2_.is_detected_)
            {
                system_message_ = "올바른 길로 가고 있습니다";
                system_message2_ = "다음 Innisfree 마커를 촬영해주세요";
                system_message3_ = "다음 마커까지 남은거리: 7.2m";

                obj_lancekun_.obj_animation_.Play("Walk");
            }
            else if (arrow3_.is_detected_)
            {
                system_message_ = "올바른 길로 가고 있습니다";
                system_message2_ = "다음 EBLIN 마커를 촬영해주세요";
                system_message3_ = "다음 마커까지 남은거리: 16.9m";

                obj_lancekun_.obj_animation_.Play("Walk");
            }
            else if (arrow3_.is_detected_)
            {
                system_message_ = "올바른 길로 가고 있습니다";
                system_message2_ = "다음 올리브영 마커를 촬영해주세요";
                system_message3_ = "다음 마커까지 남은거리: 3.3m";

                obj_lancekun_.obj_animation_.Play("Walk");
            }
            else if (arrow4_.is_detected_)
            {
                system_message_ = "잘못된 경로입니다";
                system_message2_ = "현재위치에서 경로 재탐색을 수행합니다";
                system_message3_ = "";

                obj_lancekun_.obj_animation_.Play("Idle");

                if (GUI.Button(new Rect(150, 450, 450, 150), "경로 재탐색", gui_style_btn))
                {
                    SceneManager.LoadScene("5_StarCity2");
                }
            }

            else if (arrow5_.is_detected_)
            {
                system_message_ = "목적지에 도착하였습니다";
                system_message2_ = "";
                system_message3_ = "";

                obj_lancekun_.obj_animation_.Play("Attack");

                game_state_ = eGameState.Result;
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


/*
game_state_ = eGameState.Battle;

                    system_message_ = "SEIKO 마커를 촬영해주세요";
                    system_message2_ = "총 이동해야 할 거리: 1.4m";
                    system_message3_ = "";

                    if (arrow4_.is_detected_)
                    {
                        system_message_ = "올바른 길로 가고 있습니다";
                        system_message2_ = "다음 올리브영 마커를 촬영해주세요";
                        system_message3_ = "다음 마커까지 남은거리: 3.3m";

                        obj_lancekun_.obj_animation_.Play("Walk");

*/