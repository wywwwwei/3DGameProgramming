using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    void restart();
    void back();
    GameSceneController.GameStatus getCurStatus();
    int getTimer();
}
