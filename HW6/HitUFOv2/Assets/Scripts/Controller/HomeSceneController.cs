using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
{
    public class HomeSceneController : MonoBehaviour,ISceneController,IHomeAction
    {
        void Awake() {
            Director director = Director.getInstance();
            director.setFPS(60);
            director.currentSceneController = this;
            director.currentSceneController.LoadResources();
            this.gameObject.AddComponent<HomeGUI>();
        }

        public void startGame(bool PhysicMode){
            if(PhysicMode){
                ModeChoose.getInstance().setPhysic();
            }
            else
            {
                ModeChoose.getInstance().unsetPhysic();
            }
            Director.getInstance().LoadScene(1);
        }
        public void finish(){
            Application.Quit();
        }
        public void LoadResources()
        {

        }
    }
}
