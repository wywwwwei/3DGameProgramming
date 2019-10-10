using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
{
    public class UFOFactory{
        protected static UFOFactory ufofactory;
        protected Ruler ruler = new Ruler();
        private List<GameObject> isFree = new List<GameObject>();
        private List<GameObject> isInuse = new List<GameObject>();
        public static UFOFactory getInstance(){
            if(ufofactory == null ){
                ufofactory = new UFOFactory();
            }
            return ufofactory;
        }
        public GameObject getUFO(int round){
            GameObject need;
            if(isFree.Count <= 0){
                need = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UFO"),ruler.getStart(), Quaternion.identity)as GameObject;
                need.AddComponent<UFO>();
                Rigidbody rigidbody = need.AddComponent<Rigidbody>();
                rigidbody.useGravity = false;
            }
            else{
                need = isFree[0];
                isFree.Remove(need);
            }
            isInuse.Add(need);

            int index = ruler.getColor();
            Material material = UnityEngine.Object.Instantiate(Resources.Load<Material>("Materials/" + Enum.GetName(typeof(Ruler.Color),index)));
            need.GetComponent<MeshRenderer>().material = material;

            UFO ufo = need.GetComponent<UFO>();
            ufo.setScale(ruler.getScale(Judge.getInstance().getRound()));
            need.transform.position = ruler.getStart();
            ufo.setDespos(ruler.getDes(need.transform.position));
            ufo.setScore(index+1);

            return need;
        }
        public void recycle(GameObject toRecycle){
            if(isInuse.Contains(toRecycle)){
                toRecycle.transform.position = toRecycle.GetComponent<UFO>().desPosition;
            }
        }
        public void free(GameObject toFree){
            isFree.Add(toFree);
            isInuse.Remove(toFree);
        }
    }
    
    public class UFO : MonoBehaviour {
        public int score = 1;
        public Vector3 desPosition;
        public Vector3 baseScale = new Vector3(1.5f,0.5f,1.5f);
        public bool isClicked;
        public void setScale(float multi){
            this.gameObject.transform.localScale = new Vector3(baseScale.x * multi,baseScale.y*multi,baseScale.z*multi);
        }
        public void setScore(int _score){
            score = _score;
        }
        public void setDespos(Vector3 desPos){
            desPosition = desPos;
        }
    }
    public class Ruler{
        public enum Color{ White, Red , Green , Blue };
        private System.Random random = new System.Random();
        public Color color;
        public float scale;
        public float speed;
        public float desPosition;
        public float direction;

        public int getUFOCount(int round){
            return round * 3;
        }
        public int getColor(){
            var index = random.Next(Enum.GetValues(typeof(Color)).Length);
            return index;
        }
        public float getScale(int round){
            return 1.3f - 0.1f * (random.Next(1) + 1)* round;
        }
        public float getSpeed(int round){
            return 1.0f + 0.1f * round;
        }
        public Vector3 getStart(){
            Vector3 newValue = new Vector3();
            newValue.x = ((random.Next() & 2) - 1)*(random.Next(10) + 14);
            newValue.y = ((random.Next() & 2) - 1)*(random.Next(10) + 8);
            newValue.z = 0;
            return newValue;
        }
        public Vector3 getDes(Vector3 temp){
            temp.x = -temp.x;
            temp.y = -temp.y;
            return temp;
        }
        public float getIntervals(int round){
            return 1.0f - 0.1f * round * (random.Next(3)+1);
        }
    }
}
