using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSimulation : MonoBehaviour
{
    public Character.Type objType;
    public int id;

    void OnMouseDown(){
        Debug.Log("OK!!");
        SSDirector.getInstance().currentSceneController.getClick(objType,id);
    }

    void Start(){}

    void Update(){}
}
