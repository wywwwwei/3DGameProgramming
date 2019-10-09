using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CCMoveToAction : SSAction
{
    public Vector3 target;
    public float speed;

    public static CCMoveToAction GetSSAction(GameObject gameObject,Vector3 _target,float speed,ISSActionCallback _callback){
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();

        action.target = _target;
        action.speed = speed;
        action.gameobject = gameObject;
        action.transform = gameObject.transform;
        action.callback = _callback;

        return action;
    }

    public override void Update(){
        this.transform.position = Vector3.MoveTowards(this.transform.position,target,speed* Time.deltaTime);
        if(this.transform.position == target){
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
        
    }

    public override void Start(){}
}