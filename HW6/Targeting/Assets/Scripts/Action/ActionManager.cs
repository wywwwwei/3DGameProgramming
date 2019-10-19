using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Targeting
{
    public class SSActionManager:MonoBehaviour,ISSActionCallback{

        private List<SSAction> runSequence;
        private List<SSAction> deleteSequence;
        private float AnimateSpeed = 5.0f;
        private ISSActionCallback callback;


        SSActionManager(){
            runSequence = new List<SSAction>();
            deleteSequence = new List<SSAction>();
        }

        private void Update() {
            for(int i = 0;i < runSequence.Count;i++)
            {
                if(runSequence[i].destroy){
                    runSequence.Remove(runSequence[i]);
                }
                else if(runSequence[i].enable){
                    runSequence[i].Update();
                }
            }
        }
        private void FixedUpdate() {
            for(int i = 0;i < runSequence.Count;i++)
            {
                if(runSequence[i].destroy){
                    runSequence.Remove(runSequence[i]);
                }
                else if(runSequence[i].enable){
                    runSequence[i].FixedUpdate();
                }
            }
        }

        public void addAction(SSAction action){
            runSequence.Add(action);
            action.Start();
        }

        public void setCallback(ISSActionCallback _callback)
        {
            callback = _callback;
        }
        
        public void shake(GameObject arrow)
        {
            SSAction action = ArrowShakeAction.GetSSAction(arrow,this);
            addAction(action);
        }
        public void shoot(GameObject arrow,Ruler ruler)
        {
            SSAction action = ArrowShootAction.GetSSAction(arrow,ruler.getWind(),this);
            addAction(action);
        }
        public void moveBow(GameObject bow,GameObject arrow,float TranslationX)
        {
            if(bow.transform.position.x > 1.4f)
            {
                bow.transform.position = new Vector3(1.4f,bow.transform.position.y,bow.transform.position.z);
                arrow.transform.position = new Vector3(1.4f,bow.transform.position.y + 0.5f,bow.transform.position.z);
                return;
            }
            if(bow.transform.position.x < -1.4f)
            {
                bow.transform.position = new Vector3(-1.4f,bow.transform.position.y,bow.transform.position.z);
                arrow.transform.position = new Vector3(-1.4f,bow.transform.position.y + 0.5f,bow.transform.position.z);
                return;
            }
            SSAction action = MoveAction.GetSSAction(bow,TranslationX,AnimateSpeed,this);
            SSAction action2 = MoveAction.GetSSAction(arrow,TranslationX,AnimateSpeed,this);
            addAction(action);
            addAction(action2);
        }
        public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0,
        string strParam = null){
            runSequence.Remove(source);
            if(events == SSActionEventType.Unfinish)
            {
                SSAction action = ArrowShakeAction.GetSSAction(source.gameobject,this);
                addAction(action);
            }
            else if(events == SSActionEventType.Reload)
            {
                this.callback.SSActionEvent(source);
            }
        }
    }
}
