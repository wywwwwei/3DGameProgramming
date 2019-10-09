using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : MonoBehaviour,ISceneController,IUserAction
{
    SSActionManager actionManager;
    Judge judgement;
    private Priest[] objPriest;
    private Devil[] objDevil;
    private GameObject groundLeft;       
    private GameObject groundRight;         
    private GameObject river;            
    private Boat boat;
    GameSceneController(){
        objPriest = new Priest[3];
        objDevil = new Devil[3];
    }
    void Awake() {
        SSDirector director = SSDirector.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;
        this.gameObject.AddComponent<UserGUI>();
        actionManager = this.gameObject.AddComponent<SSActionManager>();
        judgement = this.gameObject.AddComponent<Judge>();
        director.currentSceneController.LoadResources();
    }

    public void LoadResources()
    {
        //Load some landscaping static resources
        groundLeft = Object.Instantiate(Resources.Load("Prefabs/Solid"), new Vector3(-3.75f, -0.4f, -5.5f), Quaternion.identity) as GameObject;
        groundLeft.name = "GroundLeft";
        groundRight = Object.Instantiate(Resources.Load("Prefabs/Solid"), new Vector3(3.75f, -0.4f, -5.5f), Quaternion.identity) as GameObject;
        groundRight.name = "GroundRight";
        river = Object.Instantiate(Resources.Load("Prefabs/River"), new Vector3(0, -0.81f, -5.5f), Quaternion.identity) as GameObject;
        river.name = "River";

        //Load important game objects
        boat = new Boat(1);
        Vector3[] priestLandPos = new Vector3[3]{new Vector3(-2.9f,1.1f,-5.5f),new Vector3(-3.4f,1.1f,-5.5f),new Vector3(-3.9f,1.1f,-5.5f)};
        for(int i = 0;i < 3;i++)
        {
            objPriest[i] = new Priest(i,priestLandPos[i]);
        }
        Vector3[] devilLandPos = new Vector3[3]{new Vector3(-4.4f,1.0f,-5.5f),new Vector3(-4.9f,1.0f,-5.5f),new Vector3(-5.4f,1.0f,-5.5f)};
        for(int i = 0;i < 3;i++)
        {
            objDevil[i] = new Devil(i,devilLandPos[i]);
        }
        judgement.startTimer();
        //StartCoroutine(waitForOneSecond());
    }

    public void getClick(Character.Type objType,int id){
        if(judgement.getCurStatus()==Judge.GameStatus.Gaming){
            checkMove(objType,id);
        }
    }

    public void checkMove(Character.Type objType,int id){
        if(objType == Character.Type.classBoat)
        {
            if(boat.canMove())
            {
                boat.Move();
                actionManager.MoveBoat(boat);
                for(int i = 0;i < 3;i++)
                {
                    if(objPriest[i].position == Character.Status.leftBoat||objPriest[i].position == Character.Status.rightBoat){
                        objPriest[i].moveOnBoat(boat.isLeft);
                        actionManager.MovePriest(objPriest[i]);
                    }
                }
                for(int i = 0;i < 3;i++)
                {
                    if(objDevil[i].position == Character.Status.leftBoat||objDevil[i].position == Character.Status.rightBoat)
                        objDevil[i].moveOnBoat(boat.isLeft);
                        actionManager.MoveDevil(objDevil[i]);
                }
                judgement.checkWinOrLose(getPriestNum(Character.Status.leftLand)+getPriestNum(Character.Status.leftBoat),getPriestNum(Character.Status.rightLand)+getPriestNum(Character.Status.rightBoat),getDevilNum(Character.Status.leftLand)+getDevilNum(Character.Status.leftBoat),getDevilNum(Character.Status.rightLand)+getDevilNum(Character.Status.rightBoat));
            }
        }
        else if(objType == Character.Type.classPriest)
        {
            if(objPriest[id].position == Character.Status.leftLand||objPriest[id].position == Character.Status.rightLand)
            {
                if(boat.getCapacity()>0)
                {
                    objPriest[id].Move(boat.isLeft,boat.getCapacity(),boat.assignSeat());
                    actionManager.MovePriest(objPriest[id]);
                    boat.add(objPriest[id].boatSeat);
                }
            }
            else
            {
                objPriest[id].Move(boat.isLeft,boat.getCapacity(),0);
                actionManager.MovePriest(objPriest[id]);
                boat.remove(objPriest[id].boatSeat);
            }
            judgement.checkWinOrLose(getPriestNum(Character.Status.leftLand)+getPriestNum(Character.Status.leftBoat),getPriestNum(Character.Status.rightLand)+getPriestNum(Character.Status.rightBoat),getDevilNum(Character.Status.leftLand)+getDevilNum(Character.Status.leftBoat),getDevilNum(Character.Status.rightLand)+getDevilNum(Character.Status.rightBoat));
        }
        else if(objType == Character.Type.classDevil){
            if(objDevil[id].position == Character.Status.leftLand||objDevil[id].position == Character.Status.rightLand)
            {
                if(boat.getCapacity()>0)
                {
                    objDevil[id].Move(boat.isLeft,boat.getCapacity(),boat.assignSeat());
                    actionManager.MoveDevil(objDevil[id]);
                    boat.add(objDevil[id].boatSeat);
                }
            }
            else
            {
                objDevil[id].Move(boat.isLeft,boat.getCapacity(),0);
                actionManager.MoveDevil(objDevil[id]);
                boat.remove(objDevil[id].boatSeat);
            }
            judgement.checkWinOrLose(getPriestNum(Character.Status.leftLand)+getPriestNum(Character.Status.leftBoat),getPriestNum(Character.Status.rightLand)+getPriestNum(Character.Status.rightBoat),getDevilNum(Character.Status.leftLand)+getDevilNum(Character.Status.leftBoat),getDevilNum(Character.Status.rightLand)+getDevilNum(Character.Status.rightBoat));
        }
    }

    public int getDevilNum(Character.Status cur_charac_status){
        int count = 0;
        for (int i = 0;i <  3;i++){
            if(objDevil[i].position == cur_charac_status){
                count++;
            }
        }
        return count;
    }

    public int getPriestNum(Character.Status cur_charac_status){
        int count = 0;
        for (int i = 0;i <  3;i++){
            if(objPriest[i].position == cur_charac_status){
                count++;
            }
        }
        return count;
    }

    public void restart(){
        for(int i = 0;i < 3;i++)
        {
            DestroyImmediate(objDevil[i].devil);
            DestroyImmediate(objPriest[i].priest);
        }
        DestroyImmediate(groundLeft);
        DestroyImmediate(groundRight);
        DestroyImmediate(boat.boat);
        DestroyImmediate(river);
        judgement.stopTimer();
        LoadResources();
    }

    public void back(){
        SSDirector.getInstance().LoadScene(0);
    }
    public Judge.GameStatus getCurStatus(){
        return judgement.getCurStatus();
    }
    //     public IEnumerator waitForOneSecond()
    // {
    //     while (cur_status==Judge.GameStatus.Gaming && timer > 0)
    //     {
    //         yield return new WaitForSeconds(1);
    //         timer--;
    //         if(timer==0){
    //             cur_status = Judge.GameStatus.Lose;
    //         }   
    //     }
    // }
    public int getTimer(){
        return judgement.getTimer();
    }
}
