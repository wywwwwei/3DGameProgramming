using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
{
    public enum SSActionEventType:int{Started,Competeted}

    public interface ISceneController
    {
        void LoadResources();
    }

    public interface IUserAction
    {
        GameStatus getCurStatus();
        void restart();
        void back();
        void menu();
        void recover();  
        void nextTrial();
        void nextRound();      
    }
    public interface IHomeAction
    {
        void startGame();
        void finish();
    }
    public interface ISSActionCallback
    {
        void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0,
        string strParam = null);
    }
}
