using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public interface IUserAction
{
    void attack();
}

//挂载在人物上
public class VirtualButtonEventHandler : MonoBehaviour,IVirtualButtonEventHandler {
    //virtual button
    public GameObject btn;
    public IUserAction action;
    private void Start() {
        action = this.gameObject.GetComponent<Attack>();
        VirtualButtonBehaviour vbb = btn.GetComponent<VirtualButtonBehaviour>();
        if (vbb){
            vbb.RegisterEventHandler(this);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb){
        Debug.Log("attack");
        action.attack();
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb){
        Debug.Log("attacks");
    }
}