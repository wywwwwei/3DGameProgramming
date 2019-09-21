using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : MonoBehaviour,ISceneController,IUserAction
{
    private int timer = 60;
    public enum GameStatus { Gaming , Win , Lose};
    private Priest[] objPriest;
    private Devil[] objDevil;
    private GameObject groundLeft;       
    private GameObject groundRight;         
    private GameObject river;            
    private Boat boat;
    private GameStatus cur_status;
    GameSceneController(){
        objPriest = new Priest[3];
        objDevil = new Devil[3];
    }
    void Awake() {
        SSDirector director = SSDirector.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;
        this.gameObject.AddComponent<UserGUI>();
        director.currentSceneController.LoadResources();
    }

    public void LoadResources()
    {
        cur_status =  GameStatus.Gaming;

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
        StartCoroutine(waitForOneSecond());
    }

    public void getClick(Character.Type objType,int id){
        checkMove(objType,id);
    }

    public void checkMove(Character.Type objType,int id){
        if(objType == Character.Type.classBoat)
        {
            if(boat.canMove())
            {
                boat.Move();
                for(int i = 0;i < 3;i++)
                {
                    if(objPriest[i].position == Character.Status.leftBoat||objPriest[i].position == Character.Status.rightBoat)
                        objPriest[i].moveOnBoat(boat.isLeft);
                }
                for(int i = 0;i < 3;i++)
                {
                    if(objDevil[i].position == Character.Status.leftBoat||objDevil[i].position == Character.Status.rightBoat)
                        objDevil[i].moveOnBoat(boat.isLeft);
                }
                if(checkLose()==true)
                    cur_status = GameSceneController.GameStatus.Lose;
            }
        }
        else if(objType == Character.Type.classPriest)
        {
            if(objPriest[id].position == Character.Status.leftLand||objPriest[id].position == Character.Status.rightLand)
            {
                if(boat.getCapacity()>0)
                {
                    objPriest[id].Move(boat.isLeft,boat.getCapacity(),boat.assignSeat());
                    boat.add(objPriest[id].boatSeat);
                }
            }
            else
            {
                objPriest[id].Move(boat.isLeft,boat.getCapacity(),0);
                boat.remove(objPriest[id].boatSeat);
            }
            if(checkWin()==true)
                    cur_status = GameSceneController.GameStatus.Win;
        }
        else if(objType == Character.Type.classDevil){
            if(objDevil[id].position == Character.Status.leftLand||objDevil[id].position == Character.Status.rightLand)
            {
                if(boat.getCapacity()>0)
                {
                    objDevil[id].Move(boat.isLeft,boat.getCapacity(),boat.assignSeat());
                    boat.add(objDevil[id].boatSeat);
                }
            }
            else
            {
                objDevil[id].Move(boat.isLeft,boat.getCapacity(),0);
                boat.remove(objDevil[id].boatSeat);
            }
            if(checkWin()==true)
                    cur_status = GameSceneController.GameStatus.Win;
        }
    }

    public bool checkLose(){
        int priestLeftNum = 0;
        int priestRightNum = 0;
        int devilLeftNum = 0;
        int devilRightNum = 0;
        for(int i = 0;i < 3;i++)
        {
            if(objPriest[i].position == Character.Status.leftBoat||objPriest[i].position == Character.Status.leftLand)
                priestLeftNum++;
            else
                priestRightNum++;
        }
        for(int i = 0;i < 3;i++)
        {
            if(objDevil[i].position == Character.Status.leftBoat||objDevil[i].position == Character.Status.leftLand)
                devilLeftNum++;
            else
                devilRightNum++;
        }
        if(priestLeftNum!=0&&priestLeftNum<devilLeftNum)return true;
        if(priestRightNum!=0&&priestRightNum<devilRightNum)return true;
        return false;
    }

    public bool checkWin(){
        int number = 0;
        for(int i = 0;i < 3;i++)
        {
            if(objPriest[i].position == Character.Status.leftLand)
                number++;
        }
        for(int i = 0;i < 3;i++)
        {
            if(objDevil[i].position == Character.Status.leftLand)
                number++;
        }
        if(number == 6)
            return true;
        return false;
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
        LoadResources();
    }

    public void back(){
        SSDirector.getInstance().LoadScene(0);
    }
    public GameSceneController.GameStatus getCurStatus(){
        return cur_status;
    }
     public IEnumerator waitForOneSecond()
    {
        while (cur_status==GameStatus.Gaming && timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer--;
            if(timer==0){
                cur_status = GameStatus.Lose;
            }   
        }
    }
    public int getTimer(){
        return timer;
    }
}
