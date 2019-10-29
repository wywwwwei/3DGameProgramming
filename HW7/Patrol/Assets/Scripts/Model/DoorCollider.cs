using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol{
    public class DoorCollider : MonoBehaviour
    {
        public int areaNum1;
        public int areaNum2;
        public bool row;
        public Vector3 myPos;
        private void Start() {
            GameObject Parent1 = this.gameObject.transform.parent.gameObject;
            //GameObject Parent2 = Parent1.transform.parent.gameObject;
            //Debug.Log(areaNum1+":"+Parent1.transform.position);
            //myPos = Parent2.transform.TransformPoint(Parent1.transform.position);
            myPos = Parent1.transform.position;
        }
        // Start is called before the first frame update
        private void OnCollisionEnter(Collision other) {
            if(other.gameObject.tag == "Player"){
                Debug.Log("enter");
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        private void OnCollisionExit(Collision other) {
            Debug.Log("Exit:"+other.gameObject.transform.position);
            if(other.gameObject.tag == "Player"){
                other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                if(row){
                    if(other.gameObject.transform.position.z<myPos.z){
                        other.gameObject.GetComponent<CheckPlayerMove>().areaNum = areaNum1;
                    }
                    else{
                        other.gameObject.GetComponent<CheckPlayerMove>().areaNum = areaNum2;
                    }
                }
                else{
                    if(other.gameObject.transform.position.x>=myPos.x){
                        other.gameObject.GetComponent<CheckPlayerMove>().areaNum = areaNum1;
                    }
                    else{
                        other.gameObject.GetComponent<CheckPlayerMove>().areaNum = areaNum2;
                    }
                }
            }
        }
    }
}
