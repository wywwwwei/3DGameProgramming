using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol{
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
            throw new System.NotImplementedException();
        }
    }

    public class RandomMoveAction : SSAction
    {
        public static RandomMoveAction GetSSAction(GameObject gameObject,ISSActionCallback _callback){
            RandomMoveAction action = ScriptableObject.CreateInstance<RandomMoveAction>();

            action.gameobject = gameObject;
            action.transform = gameObject.transform;
            action.callback = _callback;

            return action;
        }

        public override void Update(){
            this.transform.Translate(new Vector3(0.5f*Time.deltaTime,0,1f*Time.deltaTime));
            if(this.gameobject.GetComponent<Patrol>().turn){
                this.gameobject.GetComponent<Patrol>().turn = false;
                this.transform.Rotate(0,90,0);
            }
            Debug.Log("Here");
        }

        public override void FixedUpdate(){}
        public override void Start(){}
    }

    public class DirectMoveAction : SSAction
    {
        public GameObject target;
        public float speed = 2f;

        public static DirectMoveAction GetSSAction(GameObject gameObject,GameObject _target,ISSActionCallback _callback){
            DirectMoveAction action = ScriptableObject.CreateInstance<DirectMoveAction>();

            action.target = _target;
            action.gameobject = gameObject;
            action.transform = gameObject.transform;
            action.callback = _callback;

            return action;
        }

        public override void Update(){
            this.transform.LookAt(target.transform.position);
            this.transform.position = Vector3.MoveTowards(this.transform.position,target.transform.position,speed* Time.deltaTime);
            
        }

        public override void FixedUpdate(){}
        public override void Start(){}
    }

    public class PlayerMoveAction : SSAction{
        public float TranslationX;
        public float TranslationY;
        private float rspeed = 90;
        private float speed = 10;

        public static PlayerMoveAction GetSSAction(GameObject gameObject,float _TranslationX,float _TranslationY,ISSActionCallback _callback){
            PlayerMoveAction action = ScriptableObject.CreateInstance<PlayerMoveAction>();

            action.TranslationX = _TranslationX;
            action.TranslationY = _TranslationY;
            action.gameobject = gameObject;
            action.transform = gameObject.transform;
            action.callback = _callback;

            return action;
        }

        public override void Update(){

            gameobject.transform.Translate(0, 0,TranslationY*speed*Time.deltaTime);
            gameobject.transform.Rotate(0, TranslationX * rspeed * Time.deltaTime, 0);

            this.destroy = true;
            this.callback.SSActionEvent(this);
        }

        public override void FixedUpdate() {
            
        }

        public override void Start(){}
    }
}