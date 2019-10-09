using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameStatus { Gaming , Win , Lose};

public class Judge: MonoBehaviour {
    public enum GameStatus { Gaming , Win , Lose};
    private int timer = 60;
    private GameStatus cur_status;
    private Coroutine runTimer;

    public GameStatus getCurStatus(){
        return cur_status;
    }
    public void checkWinOrLose(int priestLeftNum ,int priestRightNum,int devilLeftNum,int devilRightNum)
    {
        if(priestLeftNum + devilLeftNum == 6){
            cur_status = GameStatus.Win;
        }
        if(priestLeftNum != 0 && priestLeftNum<devilLeftNum)
        {
            cur_status = GameStatus.Lose;
        }
        if(priestRightNum!=0 && priestRightNum<devilRightNum){
            cur_status = GameStatus.Lose;
        }
        else{
            cur_status = GameStatus.Gaming;
        }
    }
    public IEnumerator waitForOneSecond()
    {
        while (cur_status==Judge.GameStatus.Gaming && timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer--;
            if(timer==0){
                cur_status = Judge.GameStatus.Lose;
            }   
        }
    }
    public int getTimer(){
        return timer;
    }

    public void startTimer(){
        cur_status = GameStatus.Gaming;
        timer = 60;
        runTimer = StartCoroutine(waitForOneSecond());
    }
    public void stopTimer(){
        if(runTimer!=null){
            StopCoroutine(runTimer);
        }
    }
}