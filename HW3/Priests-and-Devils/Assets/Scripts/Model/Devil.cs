using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character{
    public enum Status{leftLand,leftBoat,rightBoat,rightLand};
    public enum Type{classPriest,classDevil,classBoat};
}

public class Devil
{
    public float moveSpeed;
    public GameObject devil;
    public Character.Status position{get;set;}
    public Vector3 LeftPos;
    //public static Vector3[] landPos = new Vector3[3]{new Vector3(-4.4f,1.0f,-5.5f),new Vector3(-4.9f,1.0f,-5.5f),new Vector3(-5.4f,1.0f,-5.5f)};
    public static Vector3[] boatPos = new Vector3[2]{new Vector3(-1.1f,0.05f,-5.5f),new Vector3(-1.7f,0.05f,-5.5f)};
    //public static int leftNum = 0;
    //public static int rightNum = 0;
    public int boatSeat;

    public Devil(int id,Vector3 _leftPos){
        Vector3 newValue = _leftPos;
        position = Character.Status.rightLand;
        newValue.x = -newValue.x;
        devil = Object.Instantiate(Resources.Load("Prefabs/Devil"), newValue,Quaternion.identity)as GameObject;
        LeftPos = _leftPos;
        ButtonSimulation btn = devil.AddComponent<ButtonSimulation>();
        btn.objType = Character.Type.classDevil;
        btn.id = id;
        boatSeat = 0;
        moveSpeed = 2.0f;
        devil.AddComponent<ToMove>();
    }
    public void Move(bool boatIsLeft,int boatCapacity,int assignSeat){
        if(position == Character.Status.leftLand){
            if(boatIsLeft){
                if(boatCapacity>0){
                    position = Character.Status.leftBoat;
                    Vector3 newValue = Devil.boatPos[1 - assignSeat];
                    devil.GetComponent<ToMove>().setDestination(newValue);
//                    devil.transform.position = newValue;
                    boatSeat =  assignSeat;
                }
            }
        }
        else if(position == Character.Status.leftBoat){
            if(boatIsLeft){
                    position = Character.Status.leftLand;
                    moveToLeft();
            }
        }
        else if(position == Character.Status.rightBoat){
            if(!boatIsLeft){
                position = Character.Status.rightLand;
                moveToRight();
            }
        }
        else if(position == Character.Status.rightLand){
            if(!boatIsLeft){
                if(boatCapacity>0){
                    position = Character.Status.rightBoat;
                    Vector3 newValue = Devil.boatPos[1 - assignSeat];
                    newValue.x = -newValue.x;
                    devil.GetComponent<ToMove>().setDestination(newValue);
//                    devil.transform.position = newValue;
                    boatSeat =  assignSeat;
                }
            }
        }
    }
    public void moveOnBoat(bool boatIsLeft){
        position = (position == Character.Status.leftBoat ? Character.Status.rightBoat : Character.Status.leftBoat);
        Vector3 newValue = Devil.boatPos[boatSeat];
        boatSeat = 1 - boatSeat;
        if(!boatIsLeft)
            newValue.x = -newValue.x;
        devil.GetComponent<ToMove>().setDestination(newValue);
//        devil.transform.position = newValue;
    }

    public void moveToLeft(){
//        devil.transform.position = LeftPos;
        devil.GetComponent<ToMove>().setDestination(LeftPos);
    }

    public void moveToRight(){
        Vector3 newValue = LeftPos;
        newValue.x = - newValue.x;
        devil.GetComponent<ToMove>().setDestination(newValue);
//        devil.transform.position = newValue;
    }
}
