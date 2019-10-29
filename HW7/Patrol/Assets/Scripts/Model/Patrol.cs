using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol{
    public class PatrolFactory
    {
        protected static PatrolFactory patrolfactory;
        private List<GameObject> isFree = new List<GameObject>();
        private List<GameObject> isInuse = new List<GameObject>();
        private float[] positionX={25.04f,25.04f,14.88f,14.88f,14.88f,5.57f,5.57f,5.57f};
        private float[] positionZ={16.55f,26.33f,6.95f,15.62f,26.33f,6.8f,17f,26.6f};

        public static PatrolFactory getInstance(){
            if(patrolfactory == null ){
                patrolfactory = new PatrolFactory();
            }
            return patrolfactory;
        }

        public GameObject getPatrol(int areaNum){
            GameObject need;
            if(isFree.Count <= 0){
                need = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Monster"),new Vector3(positionX[areaNum-1],8.39f,positionZ[areaNum-1]),Quaternion.identity)as GameObject;
                need.AddComponent<Patrol>();
                need.AddComponent<Rigidbody>();
                need.GetComponent<Rigidbody>().useGravity=false;
            }
            else{
                need = isFree[0];
                isFree.Remove(need);
                need.SetActive(true);
            }
            isInuse.Add(need);

            need.GetComponent<Patrol>().areaNum = areaNum;
            return need;
        }

        public void freeObject(GameObject gameobj)
        {
            isFree.Add(gameobj);
            isInuse.Remove(gameobj);
            gameobj.SetActive(false);
        }
    }

    public class Patrol:MonoBehaviour{
        public int areaNum;
        public bool turn=false;

        public delegate void GameOver();
        public static event GameOver myGameOver;

        private void Start() {
        
        }
        private void Update(){
            
        }


        private void OnTriggerEnter(Collider other) {
            if(turn==false&&other.gameObject.tag == "Wall"){
                Debug.Log("Trigger");
                turn = true;
            }
            else if(other.gameObject.tag == "Player"){
                myGameOver();
            }
        }
    }
}