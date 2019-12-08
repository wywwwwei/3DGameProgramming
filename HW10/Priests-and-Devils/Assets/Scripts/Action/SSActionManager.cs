using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SSActionManager:MonoBehaviour,ISSActionCallback{

    private List<SSAction> runSequence;
    private float AnimateSpeed = 15.0f;

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

    public void MoveBoat(Boat boat){
        CCMoveToAction action = CCMoveToAction.GetSSAction(boat.boat,boat.getDestination(),AnimateSpeed,this);
        addAction(action);
    }

    public void MovePriest(Priest priest){
        Vector3 destination = priest.getDestination();
        GameObject gameObject = priest.priest;
        Vector3 startPos = gameObject.transform.position;
        Vector3 middlePos = destination;

        if(startPos.y > destination.y){
            middlePos.y = startPos.y;
        }
        else{
            middlePos.x = startPos.x;
        }
        SSAction action1 = CCMoveToAction.GetSSAction(gameObject, middlePos, AnimateSpeed,null);
        SSAction action2 = CCMoveToAction.GetSSAction(gameObject, destination, AnimateSpeed,null);
        CCSequenceAction action = CCSequenceAction.GetSSAction(1,0,new List<SSAction>{action1,action2},this);
        addAction(action);
    }
    public void MoveDevil(Devil devil){
        Vector3 destination = devil.getDestination();
        GameObject gameObject = devil.devil;
        Vector3 startPos = gameObject.transform.position;
        Vector3 middlePos = destination;

        if(startPos.y > destination.y){
            middlePos.y = startPos.y;
        }
        else{
            middlePos.x = startPos.x;
        }
        SSAction action1 = CCMoveToAction.GetSSAction(gameObject, middlePos, AnimateSpeed,null);
        SSAction action2 = CCMoveToAction.GetSSAction(gameObject, destination, AnimateSpeed,null);
        CCSequenceAction action = CCSequenceAction.GetSSAction(1,0,new List<SSAction>{action1,action2},this);
        addAction(action);
    }
    public void SSActionEvent(SSAction source,
    SSActionEventType events = SSActionEventType.Competeted,
    int intParam = 0,
    string strParam = null){
        
    }
}