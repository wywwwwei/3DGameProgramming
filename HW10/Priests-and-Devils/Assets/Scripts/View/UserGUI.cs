using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction action;

    private void Start() {
        action = SSDirector.getInstance().currentSceneController as IUserAction;
    }

    private void OnGUI() {
        //In order to facilitate positioning control
        float screenWidth = UnityEngine.Screen.width;
        float screenHeight = UnityEngine.Screen.height;

        //Add Button
        float buttonWidth = 100;
        float buttonHeight = 50;
        float spaceBetweenButton = 30;
        if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2-buttonWidth, (screenHeight-buttonHeight)*19/20, buttonWidth, buttonHeight), "Back"))
        {
            action.back();
        }
        if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2+buttonWidth, (screenHeight-buttonHeight)*19/20, buttonWidth, buttonHeight), "Restart"))
        {
            action.restart();
        }

        //Add Lable
        float resultWidth = 500;
        float resultHeight = 50;
        string cur_text=" ";
        GUIStyle fontStyle= new GUIStyle();
        fontStyle.alignment = TextAnchor.MiddleCenter;
        fontStyle.fontSize = 40;
        fontStyle.normal.textColor = Color.red;
        switch(action.getCurStatus()){
                case Judge.GameStatus.Win:
                    cur_text="You Win!!!";
                    break;
                case Judge.GameStatus.Lose:
                    cur_text="You Lose!!!";
                    break;
            }
        GUI.Label(new Rect((screenWidth-resultWidth)/2, (screenHeight-resultHeight)*2/7, resultWidth, resultHeight), cur_text,fontStyle);

        //Add Timer Lable
        float timerWidth = 100;
        float timerHeight = 50;
        string timer_text="倒计时: "+action.getTimer();
        GUIStyle timerFontStyle= new GUIStyle();
        timerFontStyle.alignment = TextAnchor.MiddleCenter;
        timerFontStyle.fontSize = 20;
        timerFontStyle.normal.textColor = Color.red;
        GUI.Label(new Rect((screenWidth-timerWidth), 0, timerWidth, timerHeight), timer_text,timerFontStyle);

        //add tips
        float tipsrWidth = 250;
        float tipsHeight = 40;
        string tips_text;
        int tips = action.getTips();
        if(tips == 0b100000000){
            tips_text = "Tips:No result";
        }else{
            tips_text="Tips: Move Priest "+tips/10+" Devil:"+tips%10;
        }
        GUIStyle tipsFontStyle= new GUIStyle();
        tipsFontStyle.alignment = TextAnchor.MiddleCenter;
        tipsFontStyle.fontSize = 20;
        tipsFontStyle.normal.textColor = Color.red;
        GUI.Label(new Rect((screenWidth-tipsrWidth), (timerHeight), tipsrWidth, tipsHeight), tips_text,tipsFontStyle);
    }
}
