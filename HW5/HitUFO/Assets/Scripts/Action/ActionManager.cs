using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
{
    public class SSActionManager:MonoBehaviour,ISSActionCallback{

        private List<SSAction> runSequence;
        private float AnimateSpeed = 5.0f;

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

        public void FlyUFO(GameObject gameObject,Ruler ruler,int round){
            CCMoveToAction action = CCMoveToAction.GetSSAction(gameObject,ruler.getDes(gameObject.transform.position),ruler.getSpeed(round)*AnimateSpeed,this);
            addAction(action);
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
        }
    }
}
