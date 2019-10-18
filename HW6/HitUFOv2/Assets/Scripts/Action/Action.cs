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

        public virtual void stop(){}
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

        public override void stop(){
            Debug.Log("first stop");
            this.transform.position = target;
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }

        public override void Start(){}
    }

    public class sequenceAction : SSAction,ISSActionCallback
    {
        public List<SSAction> sequence;
        public int repeat = 1;
        public int start = 0;

        public static sequenceAction GetSSAction(int repeat,int start,List<SSAction> sequence,ISSActionCallback _callback)
        {
            sequenceAction action = ScriptableObject.CreateInstance<sequenceAction>();
            action.repeat = repeat;
            action.sequence = sequence;
            action.start = start;
            action.callback = _callback;
            return action;
        }
        
        //Update is called once per frame
        public override void Update()
        {
            if(sequence.Count ==0)return;
            if(start < sequence.Count){
                sequence[start].Update();
            }
        }

        public override void Start()
        {
            foreach (SSAction action in sequence)
            {
                action.callback = this;
                action.Start();
            }
        }

        public override void stop() {
            if(sequence.Count==0)return;
            for(int i = 1;i < sequence.Count;i++)
            {
                UFOFactory.getInstance().free(sequence[i].gameobject);
            }
            for(int i = sequence.Count - 1;i >= 1;i--)
            {
                sequence.Remove(sequence[i]);
            }
            sequence[0].stop();
        }

        private void OnDestroy() 
        {
            foreach (SSAction action in sequence)
            {
                Destroy(action);
            }
        }

        public void SSActionEvent(SSAction source,SSActionEventType events = SSActionEventType.Competeted,int intParam = 0,string strParam = null)
        {
            Debug.Log("callback 0");
            source.destroy = false;
            this.start++;
            UFO ufo = source.gameobject.GetComponent<UFO>();
            if(!ufo.isClicked){
                Judge.getInstance().subScore(ufo.score);
            }
            UFOFactory.getInstance().free(source.gameobject);
            if(this.start>=sequence.Count){
                this.start = 0;
                if(repeat>0)repeat--;
                if(repeat==0){
                    destroy = true;
                    Debug.Log("callback 1");
                    this.callback.SSActionEvent(this);
                }
            }
        }
    }
}
