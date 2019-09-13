using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    private GamePlay controller;

    void Awake() {
        controller = new GamePlay();
        controller.restart();
    }

    void OnGUI(){
        //In order to facilitate positioning control
        float screenWidth = UnityEngine.Screen.width;
        float screenHeight = UnityEngine.Screen.height;

        //Add Button
        float buttonWidth = 100;
        float buttonHeight = 50;
        float spaceBetweenButton = 30;
        if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2-buttonWidth, (screenHeight-buttonHeight)*4/5, buttonWidth, buttonHeight), "Back"))
        {
            OnBackClick();
        }
        if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2+buttonWidth, (screenHeight-buttonHeight)*4/5, buttonWidth, buttonHeight), "Restart"))
        {
            OnRestartClick();
        }

        //Render result
        GamePlay.Result cur_res = controller.ifWin();
        if(cur_res!=GamePlay.Result.Gaming)
        {
            controller.setIngame(false);
            float resultWidth = 500;
            float resultHeight = 50;
            string cur_text=" ";
            GUIStyle fontStyle= new GUIStyle();
            fontStyle.alignment = TextAnchor.MiddleCenter;
            fontStyle.fontSize = 40;
            fontStyle.normal.textColor = Color.red;
            switch(cur_res){
                case GamePlay.Result.Win:
                    cur_text="You !!!";
                    break;
                case GamePlay.Result.Lose:
                    cur_text="You Lose!!!";
                    break;
                case GamePlay.Result.Draw:
                    cur_text="Draw!!!";
                    break;
            }
            GUI.Label(new Rect((screenWidth-resultWidth)/2, (screenHeight-resultHeight)*2/7, resultWidth, resultHeight), cur_text,fontStyle);
            GUI.enabled = false;
        }

        //Render nine palace map
        float perGridWidth = 70;
        float perGridHeight = 70;
        float baseX = (screenWidth-perGridWidth)/2-perGridWidth;
        float baseY = screenHeight/6;
        for(int i = 0;i < 3;i++)
        {
            for(int j = 0;j < 3;j++)
            {
                string text;
                GamePlay.Status temp = controller.getMap(i,j);
                if(temp==GamePlay.Status.Player1)
                    text = "X";
                else if(temp==GamePlay.Status.Player2)
                    text = "O";
                else
                    text = " ";
                if(GUI.Button(new Rect(baseX+j*perGridWidth, baseY+i*perGridHeight, perGridWidth, perGridHeight), text))
                {
                    OnGridClick(i,j);
                }
            }
        } 
        GUI.enabled = true;
    }

    void OnBackClick()
    {
        SceneManager.LoadScene("Home");
    }

    void OnRestartClick()
    {
        GUI.enabled = true;
        controller.restart();
    }

    void OnGridClick(int i,int j)
    {
        if(!controller.ifIngame())return;
        if(controller.getMap(i,j)==GamePlay.Status.Empty)
        {
            controller.setMap(i,j,GamePlay.Status.Player1);
            controller.setFirst(false);
            controller.autoOperate();
        }
    }
}
