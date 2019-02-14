using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour {
    
    public GameObject titleScreen;
  
    public GameObject helpScreen;

    public void NavigateToTitle()
    {
        this.titleScreen.SetActive(true);
     
        this.helpScreen.SetActive(false);
    }

    public void NavigateToStartSetting()
    {
        //SceneManager.LoadScene("5_StarCity");
        SceneManager.LoadScene("2_StartScene");
    }


    public void NavigateToChScene()
    {
        SceneManager.LoadScene("chSelect");
    }
  

    public void NavigateToHelp()
    {
        this.titleScreen.SetActive(false);
       
        this.helpScreen.SetActive(true);
    }

    public void NavigateToExit(){
        Application.Quit();
    }
}
