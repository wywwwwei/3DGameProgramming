using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol{
    public class Player{

        public GameObject player;


        private Rigidbody rigidbody;

        public Player(){
            player = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Player"))as GameObject;
            rigidbody = player.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.constraints = RigidbodyConstraints.FreezePositionY|RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationY|RigidbodyConstraints.FreezeRotationZ;
            //rigidbody.isKinematic = true;
            player.AddComponent<CheckPlayerMove>();
        }


    }

    public class CheckPlayerMove : MonoBehaviour{

        private GameSceneController gameSceneController;
        public int areaNum = 0;
        private bool once = false;
        private void Start() {
            gameSceneController = Director.getInstance().currentSceneController as GameSceneController;
        }

        private void Update() {
            float translationY = Input.GetAxis("Vertical");
		    float translationX = Input.GetAxis("Horizontal");

            if(translationX!=0||translationY!=0){
                once = true;
                this.gameObject.GetComponent<Animator>().SetInteger("toDo",6);
                gameSceneController.MovePlayer(translationX,translationY);
            }
            else{
                if(once)
                {
                    this.gameObject.GetComponent<Animator>().SetInteger("toDo",10);
                    once = false;
                }
            }
        }
    }
}