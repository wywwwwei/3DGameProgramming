using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
{
    public class checkClick:MonoBehaviour{
        public GameObject cam;
        private Judge judgement;
        void Start(){
            judgement = Judge.getInstance();
        }

        // Update is called once per frame
        void Update () {
            if (Input.GetButtonDown("Fire1")) 
            {
                Debug.Log ("Fired Pressed");
                Debug.Log (Input.mousePosition);

                Vector3 mp = Input.mousePosition; //get Screen Position

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {
                    if (hit.collider.gameObject.tag.Contains("UFO")) { //plane tag
                        Debug.Log ("hit " + hit.collider.gameObject.name +"!" ); 
                    }
                    UFO ufo = hit.transform.gameObject.GetComponent<UFO>();
                    ufo.isClicked = true;
                    judgement.addScore(ufo.score);
                    UFOFactory.getInstance().recycle(hit.transform.gameObject);
                }
            }		
        }
    }

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
    }
    
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
}
