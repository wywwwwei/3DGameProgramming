using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
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
            GUI.Label(new Rect((screenWidth-titleWidth)/2, (screenHeight-titleHeight)*2/5, titleWidth, titleHeight), "Hit UFO",fontStyle);

            //Add Button and click event
            float buttonWidth = 100;
            float buttonHeight = 50;
            float spaceBetweenButton = 30;
            if(GUI.Button(new Rect((screenWidth-buttonWidth)/2-buttonWidth-spaceBetweenButton, (screenHeight-buttonHeight)*3/4, buttonWidth, buttonHeight), "Start Game"))
            {
                action.startGame(false);
            }
            if(GUI.Button(new Rect((screenWidth-buttonWidth)/2, (screenHeight-buttonHeight)*3/4, buttonWidth, buttonHeight), "Physic Mode"))
            {
                action.startGame(true);
            }
            if(GUI.Button(new Rect((screenWidth+buttonWidth)/2+spaceBetweenButton, (screenHeight-buttonHeight)*3/4, buttonWidth, buttonHeight), "Quit"))
            {
                action.finish();
            }        
        }
    }

    public class UserGUI : MonoBehaviour{

        private IUserAction action;
        private Judge judgement;
        private int count;

        private void Start() {
            action = Director.getInstance().currentSceneController as IUserAction;
            judgement = Judge.getInstance();
        }

        private void OnGUI() {
            //In order to facilitate positioning control
            float screenWidth = UnityEngine.Screen.width;
            float screenHeight = UnityEngine.Screen.height;
            if(judgement.getCurStatus() == GameStatus.Gaming)
            {
                float buttonWidth = 100;
                float buttonHeight = 50;
                if(GUI.Button(new Rect(0, (screenHeight-buttonHeight), buttonWidth, buttonHeight), "PAUSE"))
                {
                    action.menu();
                }
                if(judgement.getCountdown())
                {
                    count++;
                    if(count==1){
                        StartCoroutine(CountOneSecond());
                    }
                    //Add Lable
                    float countWidth = 500;
                    float countHeight = 100;
                    GUIStyle fontStyle= new GUIStyle();
                    fontStyle.alignment = TextAnchor.MiddleCenter;
                    fontStyle.fontSize = 40;
                    fontStyle.normal.textColor = Color.red;
                    string cur_text = ""+judgement.getCountTime();
                    GUI.Label(new Rect((screenWidth-countWidth)/2, (screenHeight-countHeight)/2, countWidth, countHeight), cur_text,fontStyle);
                }
                else
                {
                    if(count!=0)
                    {
                        StopAllCoroutines();
                        count=0;
                    }
                }
            }
            else
            {
                //Add Button
                float buttonWidth = 100;
                float buttonHeight = 50;
                float spaceBetweenButton = 30;
                if(GUI.Button(new Rect(0, (screenHeight-buttonHeight), buttonWidth, buttonHeight), "CONTINUE"))
                {
                    action.recover();
                }
                if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2-buttonWidth, (screenHeight-buttonHeight)*19/20, buttonWidth, buttonHeight), "Back"))
                {
                    action.back();
                }
                if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2+buttonWidth, (screenHeight-buttonHeight)*19/20, buttonWidth, buttonHeight), "Restart"))
                {
                    action.restart();
                }
                if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2-buttonWidth, (screenHeight-buttonHeight)*3/4, buttonWidth, buttonHeight), "Next Round"))
                {
                    action.nextRound();
                }
                if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2+buttonWidth, (screenHeight-buttonHeight)*3/4, buttonWidth, buttonHeight), "Next Trial"))
                {
                    action.nextTrial();
                }

                //Add Lable
                float resultWidth = 500;
                float resultHeight = 50;
                string cur_text=" ";
                GUIStyle fontStyle= new GUIStyle();
                fontStyle.alignment = TextAnchor.MiddleCenter;
                fontStyle.fontSize = 40;
                fontStyle.normal.textColor = Color.red;
                switch(judgement.getCurStatus()){
                        case GameStatus.Win:
                            cur_text="You Win!!!";
                            break;
                        case GameStatus.Lose:
                            cur_text="You Lose!!!";
                            break;
                    }
                GUI.Label(new Rect((screenWidth-resultWidth)/2, (screenHeight-resultHeight)*2/7, resultWidth, resultHeight), cur_text,fontStyle);
            }
            //Add Score Lable
            float scoreWidth = 100;
            float scoreHeight = 50;
            GUIStyle scoreFontStyle= new GUIStyle();
            scoreFontStyle.alignment = TextAnchor.MiddleCenter;
            scoreFontStyle.fontSize = 20;
            scoreFontStyle.normal.textColor = Color.red;
            GUI.Label(new Rect((screenWidth-scoreWidth), scoreHeight,scoreWidth,scoreHeight), "Round: " + judgement.getRound(), scoreFontStyle);
            GUI.Label(new Rect((screenWidth-scoreWidth), scoreHeight*2,scoreWidth,scoreHeight), "Trial: " + judgement.getTrial(), scoreFontStyle);
            GUI.Label(new Rect((screenWidth-scoreWidth), scoreHeight*3,scoreWidth,scoreHeight), "Score: " + judgement.getScore(), scoreFontStyle);
        }

        IEnumerator CountOneSecond()
        {
            for(int i = 0;i<3;i++)
            {
                yield return new WaitForSeconds(1);
                judgement.reduceTime();
            }
        }
    }
}
