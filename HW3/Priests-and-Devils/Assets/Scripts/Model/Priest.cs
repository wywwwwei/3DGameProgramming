using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest
{
    public GameObject priest;
    public Character.Status position{get;set;}
    public Vector3 LeftPos;
    //public static Vector3[] landPos = new Vector3[3]{new Vector3(-2.9f,1.1f,-5.5f),new Vector3(-3.4f,1.1f,-5.5f),new Vector3(-3.9f,1.1f,-5.5f)};
    public static Vector3[] boatPos = new Vector3[2]{new Vector3(-1.1f,0.2f,-5.5f),new Vector3(-1.7f,0.2f,-5.5f)};
    //public static int leftNum = 0;
    //public static int rightNum = 0;
    public int boatSeat;
    public float moveSpeed;

    public Priest(int id,Vector3 _leftPos){
        Vector3 newValue = _leftPos;
        position = Character.Status.rightLand;
        newValue.x = -newValue.x;
        priest = Object.Instantiate(Resources.Load("Prefabs/Priest"), newValue,Quaternion.identity)as GameObject;
        LeftPos = _leftPos;
        ButtonSimulation btn = priest.AddComponent<ButtonSimulation>();
        btn.objType = Character.Type.classPriest;
        btn.id = id;
        boatSeat = 0;
        moveSpeed = 2.0f;
        priest.AddComponent<ToMove>();
    }
    public void Move(bool boatIsLeft,int boatCapacity,int assignSeat){
        if(position == Character.Status.leftLand){
            if(boatIsLeft){
                if(boatCapacity>0){
                    position = Character.Status.leftBoat;
                    Vector3 newValue = Priest.boatPos[1 - assignSeat];
//                    priest.transform.position = newValue;
                    priest.GetComponent<ToMove>().setDestination(newValue);
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
                    Vector3 newValue = Priest.boatPos[1 - assignSeat];
                    newValue.x = -newValue.x;
                    priest.GetComponent<ToMove>().setDestination(newValue);
//                    priest.transform.position = newValue;
                    boatSeat =  assignSeat;
                }
            }
        }
    } 
    public void moveOnBoat(bool boatIsLeft){
        position = (position == Character.Status.leftBoat ? Character.Status.rightBoat : Character.Status.leftBoat);
        Vector3 newValue = Priest.boatPos[boatSeat];
        boatSeat = 1 - boatSeat;
        if(!boatIsLeft)
            newValue.x = -newValue.x;
        priest.GetComponent<ToMove>().setDestination(newValue);
//        priest.transform.position = newValue;
    }

    public void moveToLeft(){
        priest.GetComponent<ToMove>().setDestination(LeftPos);
//        priest.transform.position = LeftPos;
    }

    public void moveToRight(){
        Vector3 newValue = LeftPos;
        newValue.x = - newValue.x;
        priest.GetComponent<ToMove>().setDestination(newValue);
//        priest.transform.position = newValue;
    }
}
