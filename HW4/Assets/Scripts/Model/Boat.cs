using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat
{
    public static Vector3[] boatPos = new Vector3[2]{new Vector3(-1.5f,-0.2f,-5.5f),new Vector3(1.5f,-0.2f,-5.5f)};
    public bool isLeft { get;set;}
    public bool[] isOccupy;
    public int capacity;
    public GameObject boat;
    public Boat(int id){
        isOccupy = new bool[2]{false,false};
        isLeft = false;
        capacity = 2;
        Debug.Log("LoadBoat");
        boat = Object.Instantiate(Resources.Load("Prefabs/Boat"),Boat.boatPos[1],Quaternion.identity)as GameObject;
        boat.name = "Boat";
        ButtonSimulation btn = boat.AddComponent<ButtonSimulation>();
        btn.objType = Character.Type.classBoat;
        btn.id = id;
        //boat.AddComponent<ToMove>();
    }
    public int assignSeat(){
        if(!isOccupy[0]){
            return 0;
        }
        if(!isOccupy[1]){
            return 1;
        }
        return 0;
    }
    public void add(int boatSeat){
        capacity--;
        isOccupy[boatSeat] = true;
        Debug.Log(isOccupy[0]+" "+isOccupy[1]);
    }

    public void remove(int boatSeat){
        capacity++;
        isOccupy[boatSeat] = false;
    }
    public bool canMove(){
        return capacity!=2;
    }

    public int getCapacity(){
        return capacity;
    }

    public void Move(){
        if(canMove()){
            isLeft = ( isLeft ? false : true);
            bool temp = isOccupy[0];
            isOccupy[0] = isOccupy[1];
            isOccupy[1] = temp;  
        }
    }

    public Vector3 getDestination(){
        Vector3 newValue = Boat.boatPos[isLeft?0:1];
        return newValue;
    }
}
