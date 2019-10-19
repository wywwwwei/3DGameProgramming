using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Targeting
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
            GUI.Label(new Rect((screenWidth-titleWidth)/2, (screenHeight-titleHeight)*2/5, titleWidth, titleHeight), "Targeting",fontStyle);

            //Add Button and click event
            float buttonWidth = 100;
            float buttonHeight = 50;
            float spaceBetweenButton = 30;
            if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2-buttonWidth, (screenHeight-buttonHeight)*3/4, buttonWidth, buttonHeight), "Start Game"))
            {
                action.startGame();
            }
            if(GUI.Button(new Rect((screenWidth+spaceBetweenButton)/2, (screenHeight-buttonHeight)*3/4, buttonWidth, buttonHeight), "Quit"))
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
        void Update() 
        {
            if(judgement.getCurStatus()==GameStatus.Gaming)
            {
                if(Input.GetButtonDown("Fire1"))
                {
                    action.shoot();
                }
                float translationY = Input.GetAxis("Vertical");
                float translationX = Input.GetAxis("Horizontal");
                if(translationX!=0)
                {
                    action.moveBow(translationX,translationY,0);
                }

            }
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
            
            float windWidth = 320;
            float windHeight = 50;
            GUI.Label(new Rect((screenWidth-windWidth), scoreHeight,windWidth,windHeight), Ruler.getInstance().display(), scoreFontStyle);

        }
    }
}
