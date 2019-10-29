using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Patrol
{
    public class Director : System.Object
    {
        private static Director _instance;
        
        public ISceneController currentSceneController { get;set;}

        public bool running{ get; set;}

        public static Director getInstance(){
            if(_instance == null ){
                _instance = new Director();
            }
            return _instance;
        }

        public void LoadScene(int num){
            SceneManager.LoadScene(num);
        }
        public int getFPS(){
            return Application.targetFrameRate;
        }

        public void setFPS(int fps){
            Application.targetFrameRate = fps;
        }
    }
}
