using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
{
    public class SSActionManager:MonoBehaviour,ISSActionCallback,baseActionManager{

        private List<SSAction> runSequence;
        private float AnimateSpeed = 5.0f;
        private ISSActionCallback callback;

        SSActionManager(){
            runSequence = new List<SSAction>();
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

        public void addAction(SSAction action){
            runSequence.Add(action);
            action.Start();
        }

        public void setCallback(ISSActionCallback _callback)
        {
            callback = _callback;
        }
        public void stop()
        {
            if(runSequence.Count <= 0) return;
            runSequence[0].stop();
        }

        public void FlyUFO(List<GameObject> waitToFly,Ruler ruler,int round){
            List<SSAction> toMove = new List<SSAction>();
            for(int i = 0;i < waitToFly.Count; i++)
            {
                toMove.Add(CCMoveToAction.GetSSAction(waitToFly[i],ruler.getDes(waitToFly[i].transform.position),ruler.getSpeed(round)*AnimateSpeed,null));
            }
            sequenceAction action = sequenceAction.GetSSAction(1,0,toMove,this);
            addAction(action);
        }

        public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0,
        string strParam = null){
            runSequence.Remove(source);
            if(this.callback!=null)
                this.callback.SSActionEvent(source);
        }
    }

    public class PhysicActionManager:MonoBehaviour,ISSActionCallback,baseActionManager
    {
        private List<SSAction> runSequence;
        private float AnimateSpeed = 5.0f;
        private ISSActionCallback callback;

        private List<SSAction> waitSequence;

        PhysicActionManager(){
            runSequence = new List<SSAction>();
            waitSequence = new List<SSAction>();
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

        public void addAction(SSAction action){
            runSequence.Add(action);
            action.Start();
        }

        public void addWaitAction(SSAction action){
            waitSequence.Add(action);
        }

        public void setCallback(ISSActionCallback _callback)
        {
            callback = _callback;
        }

        public void stop()
        {
            StopAllCoroutines();
            while(runSequence.Count>0)
            {
                runSequence[0].stop();
            }
            for(int i = 0;i<waitSequence.Count;i++)
            {
                UFOFactory.getInstance().free(waitSequence[i].gameobject);
            }
            waitSequence.Clear();
        }

        public void FlyUFO(List<GameObject> waitToFly,Ruler ruler,int round){

            SSAction action;
            float waitTime = 0;
            for(int i = 0;i < waitToFly.Count;i++){
                Debug.Log("Flying");
                action = CCMoveToAction.GetSSAction(waitToFly[i],ruler.getDes(waitToFly[i].transform.position),ruler.getSpeed(round)*AnimateSpeed,this);
                if( i == 0 ){
                    addAction(action);         
                }else{
                    addWaitAction(action);
                    StartCoroutine(setNextFly(waitTime,action));
                }
                waitTime += ruler.getIntervals(round);
               
            }
        }

        IEnumerator setNextFly(float time,SSAction action)
        {
            yield return new WaitForSeconds(time);
            addAction(action);
            waitSequence.Remove(action);
        }

        public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0,
        string strParam = null){
            UFO ufo = source.gameobject.GetComponent<UFO>();
            if(!ufo.isClicked){
                Judge.getInstance().subScore(ufo.score);
            }
            UFOFactory.getInstance().free(source.gameobject);
            runSequence.Remove(source);
            if(runSequence.Count<=0)
            {
                if(this.callback!=null)
                    this.callback.SSActionEvent(source);
            }
        }
    }
}
