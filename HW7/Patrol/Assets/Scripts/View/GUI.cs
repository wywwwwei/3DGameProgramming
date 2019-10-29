using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    public class HomeGUI : MonoBehaviour
    {
        private IHomeAction action;
        private bool state;
        private void Start() {
            action = Director.getInstance().currentSceneController as IHomeAction;
        }
        private void OnGUI() {
            //In order to facilitate positioning control
            float screenWidth = UnityEngine.Screen.width;
            float screenHeight = UnityEngine.Screen.height;

            //Add Title
            float titleWidth = 100;
            float titleHeight = 50;
            GUIStyle fontStyle= new GUIStyle();
            fontStyle.alignment = TextAnchor.MiddleCenter;
            fontStyle.fontSize = 40;
            fontStyle.normal.textColor = Color.red;
            GUI.Label(new Rect((screenWidth-titleWidth)/2, (screenHeight-titleHeight)*2/5, titleWidth, titleHeight), "Automatic Patrol",fontStyle);

            //Add Button and click event
            float buttonWidth = 100;
            float buttonHeight = 50;
            float spaceBetweenButton = 30;
            if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2-buttonWidth, (screenHeight-buttonHeight)*3/4, buttonWidth, buttonHeight), "Start Game"))
            {
                action.startGame();
            }
            if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2+buttonWidth, (screenHeight-buttonHeight)*3/4, buttonWidth, buttonHeight), "Quit"))
            {
                action.finish();
            }        
        }
    }

    public class UserGUI : MonoBehaviour{
        private IUserAction action;
        private Judge judgement;

        void Start()
        {
            action = Director.getInstance().currentSceneController as IUserAction;
            judgement = Judge.getInstance();
        }

        private void OnGUI()
        {
            float screenWidth = UnityEngine.Screen.width;
            float screenHeight = UnityEngine.Screen.height;

            //Add Button and click event
            float buttonWidth = 100;
            float buttonHeight = 50;

            if(GUI.Button(new Rect(0, (screenHeight-buttonHeight), buttonWidth, buttonHeight), "Back"))
            {
                action.back();
            }        

            //Add Score Lable
            float scoreWidth = 100;
            float scoreHeight = 30;
            GUIStyle scoreFontStyle= new GUIStyle();
            scoreFontStyle.alignment = TextAnchor.MiddleCenter;
            scoreFontStyle.fontSize = 20;
            scoreFontStyle.normal.textColor = Color.red;
            GUI.Label(new Rect((screenWidth-scoreWidth), 0,scoreWidth,scoreHeight), "Score: " + judgement.getScore(), scoreFontStyle);

            if(judgement.getCurStatus() == GameStatus.Lose){
                float resultWidth = 500;
                float resultHeight = 50;
                string cur_text="You Lose";
                GUIStyle fontStyle= new GUIStyle();
                fontStyle.alignment = TextAnchor.MiddleCenter;
                fontStyle.fontSize = 40;
                fontStyle.normal.textColor = Color.red;
                GUI.Label(new Rect((screenWidth-resultWidth)/2, (screenHeight-resultHeight)*2/7, resultWidth, resultHeight), cur_text,fontStyle);
            }
        }
    }

}
