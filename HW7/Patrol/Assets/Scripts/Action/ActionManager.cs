using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol{
    public class SSActionManager:MonoBehaviour,ISSActionCallback{

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

        private void FixedUpdate(){
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

        public void setCallback(ISSActionCallback _callback)
        {
            callback = _callback;
        }

        public void addAction(SSAction action){
            for(int i = 0;i<runSequence.Count;i++){
                if(runSequence[i].gameobject.Equals(action.gameobject)){
                    runSequence.Remove(runSequence[i]);
                    break;
                }
            }
            runSequence.Add(action);
        }


        public void MovePlayer(GameObject gameobj,float TranslationX,float TranslationY){
            SSAction action = PlayerMoveAction.GetSSAction(gameobj,TranslationX,TranslationY,this);
            addAction(action);
        }

        public void RandomMovePatrol(GameObject gameobj){
            SSAction action = RandomMoveAction.GetSSAction(gameobj,null);
            addAction(action);
        }

        public void DirectMove(GameObject gameobj1,GameObject gameobj2){
            SSAction action = DirectMoveAction.GetSSAction(gameobj1,gameobj2,this);
            addAction(action);
        }
        public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0,
        string strParam = null){
            runSequence.Remove(source);
        }
    }
}
