using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSceneController : MonoBehaviour,ISceneController,IHomeAction
{
    void Awake() {
        SSDirector director = SSDirector.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;
        director.currentSceneController.LoadResources();
        this.gameObject.AddComponent<HomeGUI>();
    }

    public void startGame(){
        SSDirector.getInstance().LoadScene(1);
    }
    public void finish(){
        Application.Quit();
    }
    public void LoadResources()
    {

    }
    public void getClick(Character.Type objType,int id){
        
    }
}
