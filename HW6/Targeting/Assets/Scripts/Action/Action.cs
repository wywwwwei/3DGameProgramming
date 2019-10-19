using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Targeting
{

    public class SSAction : ScriptableObject{
        public bool enable = true;
        public bool destroy = false;

        public GameObject gameobject{get;set;}
        public Transform transform{get;set;}
        public ISSActionCallback callback{get;set;}

        protected SSAction(){}

        //Use this for initialization
        public virtual void Start(){
            throw new System.NotImplementedException();
        }

        //Update is called once per frame
        public virtual void Update(){
            throw new System.NotImplementedException();
        }
        public virtual void FixedUpdate(){
        }

        public virtual void stop(){}
    }
    
    public class MoveAction : SSAction
    {
        public float TranslationX;
        public float speed;

        public static MoveAction GetSSAction(GameObject gameObject,float _TranslationX,float speed,ISSActionCallback _callback){
            MoveAction action = ScriptableObject.CreateInstance<MoveAction>();

            action.TranslationX = _TranslationX;
            action.speed = speed;
            action.gameobject = gameObject;
            action.transform = gameObject.transform;
            action.callback = _callback;

            return action;
        }

        public override void Update(){
            this.gameobject.transform.Translate(TranslationX*speed*Time.deltaTime,0,0);
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }

        public override void Start(){}
    }
    public class ArrowShootAction : SSAction
    {
        public Vector3 force;                  
        public Vector3 wind;
        public Rigidbody rigidbody;

        public static ArrowShootAction GetSSAction(GameObject gameObject,Vector3 wind,ISSActionCallback _callback)
        {
            ArrowShootAction action = ScriptableObject.CreateInstance<ArrowShootAction>();
            action.gameobject = gameObject;
            action.force = new Vector3(0, 0, 20);
            action.wind = wind;
            action.rigidbody = gameObject.GetComponent<Rigidbody>();
            action.callback = _callback;
            return action;
        }
        public override void Start()
        {
            gameobject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameobject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
        public override void Update(){}

        public override void FixedUpdate()
        {
            this.rigidbody.AddForce(wind, ForceMode.Force);
            if (this.gameobject.transform.position.z > 30 )
            {
                this.gameobject.GetComponent<Rigidbody>().isKinematic = true;
                Debug.Log("callback 0");
                this.destroy = true;
                this.callback.SSActionEvent(this,SSActionEventType.Reload);
            }
            else if(this.gameobject.tag == "head")
            {
                this.gameobject.GetComponent<Rigidbody>().isKinematic = true;
                Debug.Log("callback 1");
                this.destroy = true;
                this.callback.SSActionEvent(this,SSActionEventType.Unfinish);
            }
        }
    }
    public class ArrowShakeAction : SSAction
    {
        public Vector3 startPos;
        public int times = 3;
        public float radian = 0;
        public float radius = 0.1f;
        public float deltaRadian = 5f;
        public float actionTime = 1.0f;
        public static ArrowShakeAction GetSSAction(GameObject gameObject,ISSActionCallback _callback)
        {
            ArrowShakeAction action = ScriptableObject.CreateInstance<ArrowShakeAction>();
            action.gameobject = gameObject;
            action.startPos = gameObject.transform.position;
            action.callback = _callback;
            return action;
        }
        public override void Start(){}
        public override void FixedUpdate()
        {
            if(actionTime <= 0)
            {
                this.destroy = true;
                Debug.Log("callback 2");
                this.callback.SSActionEvent(this,SSActionEventType.Reload);
            }
            actionTime -= Time.deltaTime;

            radian = radian + deltaRadian;
            this.gameobject.transform.position = startPos + new Vector3(0,Mathf.Cos(radian)*radius,0);
        }
        public override void Update()
        {}
    }
}
