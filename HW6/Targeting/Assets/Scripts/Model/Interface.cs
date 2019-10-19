using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Targeting
{
    public enum SSActionEventType:int{Started,Competeted,Unfinish,Reload}

    public interface ISceneController
    {
        void LoadResources();
    }

    public interface IUserAction
    {
        GameStatus getCurStatus();
        void shoot();
        void moveBow(float translationX,float translationY,float translationZ);
        void restart();
        void back(); 
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
