using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMove : MonoBehaviour
{
    public enum MoveStatus{Stationary, ToMiddle, ToDest}
    private MoveStatus moveStatus;
    private Vector3 middle;
    private Vector3 destination;
    public float moveSpeed = 15.0f;

    public void setDestination(Vector3 _destination){
        if(System.Math.Abs(this.gameObject.transform.position.y - _destination.y) < 0.0001f){
            moveStatus = MoveStatus.ToDest;
            destination = _destination;
        }
        else if(this.gameObject.transform.position.y > _destination.y){
            moveStatus = MoveStatus.ToMiddle;
            destination = _destination;
            middle = _destination;
            middle.y = this.gameObject.transform.position.y;
        }
        else{
            moveStatus = MoveStatus.ToMiddle;
            destination = _destination;
            middle = _destination;
            middle.x = this.gameObject.transform.position.x;
        }
    }

    private void Update() {
        if(moveStatus == MoveStatus.ToDest){
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, destination, moveSpeed * Time.deltaTime);
                if (this.gameObject.transform.position == destination){
                    moveStatus = MoveStatus.Stationary;
                }
        }
        if(moveStatus == MoveStatus.ToMiddle){
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, middle, moveSpeed * Time.deltaTime);
                if (this.gameObject.transform.position == middle){
                    moveStatus = MoveStatus.ToDest;
                }
        }
    }
    public void reset(){
        moveStatus = MoveStatus.Stationary;
    }
}
