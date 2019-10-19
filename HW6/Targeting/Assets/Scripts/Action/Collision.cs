using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Targeting{
    public class checkCollision : MonoBehaviour
    {
        public GameSceneController gameSceneController;
        private void Start() {
            gameSceneController = Director.getInstance().currentSceneController as GameSceneController;
        }
        void OnTriggerEnter(Collider other)
        {
            if(this.gameObject.tag == "arrow")
            {
                //this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                //this.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                this.gameObject.tag = "head";

                Judge.getInstance().addScore(other.transform.gameObject.GetComponent<SetScore>().score);
            }
        }
    }
}
