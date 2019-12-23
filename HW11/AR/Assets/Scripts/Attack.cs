using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFactory
{
    protected static AttackFactory attackFactory;
    private List<GameObject> isFree = new List<GameObject>();       //空闲粒子系统
    private List<GameObject> isInuse = new List<GameObject>();      //正在使用的粒子系统
    public static AttackFactory getInstance(){
        if(attackFactory == null ){
            attackFactory = new AttackFactory();
        }
        return attackFactory;
    }

    public GameObject getAttack(Vector3 begin,Vector3 direction){
        GameObject need;
        if(isFree.Count<=0){
            need = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Fire_Ghost_CampFire Variant"),begin, Quaternion.Euler(direction))as GameObject;
            need.AddComponent<Attacks>();
        }else{
            need = isFree[0];
            isFree.Remove(need);
            need.SetActive(true);
            need.transform.position = begin;
            need.transform.rotation = Quaternion.Euler(direction);
        }
        isInuse.Add(need);
        return need;
    }

    public void free(GameObject toFree){
        toFree.SetActive(false);
        isInuse.Remove(toFree);
        isFree.Add(toFree);
    }
}

//挂载在人物上
public class Attack : MonoBehaviour,IUserAction
{
    private Animation _animation;
    private Transform pos;
    private AttackFactory attackFactory;
    public float attackRange = 30f;     //攻击范围，超出就回收
    private Vector3 attackPosOffset = new Vector3(0f,0.3f,0f); //发射位置相较于人物位置的偏移
    private void Start() {
        pos = this.gameObject.transform; 
        attackFactory = AttackFactory.getInstance();   
        _animation = this.gameObject.GetComponent<Animation>();
    }
    public void attack(){
        _animation.Play("sj001_skill2");
        Vector3 position = pos.position + attackPosOffset;
        Vector3 direction = pos.rotation.eulerAngles;
        Vector3 destination = position + pos.forward * attackRange;
        GameObject attack = attackFactory.getAttack(position,direction);
        attack.GetComponent<Attacks>().destination = destination;
        attack.GetComponent<Attacks>().ifMove = true;
    }
}

//挂载在发射物上
public class Attacks : MonoBehaviour {
    public Vector3 destination;             //人物位置 + 人物正前方向*攻击范围
    public bool ifMove = false;             //是否发射
    public float attackSpeed = 2f;          //移动速度
    private AttackFactory attackFactory;   
    private void Start() {
        attackFactory = AttackFactory.getInstance();     
    }
    private void Update() {
        if(ifMove){
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position,destination,attackSpeed*Time.deltaTime);
            if (Vector3.Distance(transform.position, destination) < 1f)
            {
                Debug.Log("arrive");
                ifMove = false;
                attackFactory.free(this.gameObject);
            }
        }
    }
}