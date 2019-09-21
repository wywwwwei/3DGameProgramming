using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHomeAction
{
    void startGame();
    void finish();
}

public class HomeGUI : MonoBehaviour
{
    private IHomeAction action;
    private bool state;
    private void Start() {
        action = SSDirector.getInstance().currentSceneController as IHomeAction;
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
        GUI.Label(new Rect((screenWidth-titleWidth)/2, (screenHeight-titleHeight)*2/5, titleWidth, titleHeight), "Priests and Devils",fontStyle);

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