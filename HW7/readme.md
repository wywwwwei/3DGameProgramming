#  智能巡逻兵游戏

[TOC]

[视频演示]( https://www.bilibili.com/video/av73834383/ )

## 游戏要求

游戏设计要求：

- 创建一个地图和若干巡逻兵(使用动画)；
- 每个巡逻兵走一个3~5个边的凸多边型，位置数据是相对地址。即每次确定下一个目标位置，用自己当前位置为原点计算；
- 巡逻兵碰撞到障碍物，则会自动选下一个点为目标；
- 巡逻兵在设定范围内感知到玩家，会自动追击玩家；
- 失去玩家目标后，继续巡逻；
- 计分：玩家每次甩掉一个巡逻兵计一分，与巡逻兵碰撞游戏结束；

## 游戏设计

其实很多代码的部分与之前的项目很相似，所以只挑与要求相关的代码作展示

### 预设

本次游戏的预设，地图来源于学长的代码中的预制，人物/巡逻兵模型来源于Asset Store

要点：每一类预制都要添加Collier碰撞体组件，Monster(巡逻兵)类需要添加isTrigger，Player和Monster都添加不带重力的Rigidbody

#### 地图的门

为地图中的门添加脚本（既要避免巡逻兵走门，又为了让Player能够通过门）

首先门的Renderer要关闭掉

每次通过门，Player要判断当前的area，Monster要设置为需要转向

```C#
public class DoorCollider : MonoBehaviour
{
    public int areaNum1;
    public int areaNum2;
    public bool row;
    public Vector3 myPos;
    private void Start() {
        GameObject Parent1 = this.gameObject.transform.parent.gameObject;
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
```

#### Player

Player需要挂载的脚本

需要检测移动，并且记录当前的area，并且设置动画

```c#
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
```

#### Monster

Monster需要挂载的脚本

如果碰撞到栏杆则转向，碰撞到玩家则通过delegate通知GameSceneController游戏结束

```c#
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
```

## 实际要求的实现

#### 巡逻兵走凸多边形

- 巡逻兵走凸多边形

  这里我的实现比较简单，往一个方向走，一旦遇到碰撞（栅栏），马上换方向

  ```c#
  public class RandomMoveAction : SSAction
  {
      public static RandomMoveAction GetSSAction(GameObject gameObject,ISSActionCallback _callback){
          RandomMoveAction action = ScriptableObject.CreateInstance<RandomMoveAction>();
  
          action.gameobject = gameObject;
          action.transform = gameObject.transform;
          action.callback = _callback;
  
          return action;
      }
  
      public override void Update(){
          this.transform.Translate(new Vector3(0.5f*Time.deltaTime,0,1f*Time.deltaTime));
          if(this.gameobject.GetComponent<Patrol>().turn){
              this.gameobject.GetComponent<Patrol>().turn = false;
              this.transform.Rotate(0,90,0);
          }
          Debug.Log("Here");
      }
  
      public override void FixedUpdate(){}
      public override void Start(){}
  }
  ```

#### 自动追击

- 巡逻兵在设定范围内感知到玩家，会自动追击玩家

  放在GameSceneController的Update中检测，如果巡逻兵和玩家在同一Area，则追击玩家，使用playArea来记录上一个Area，失去玩家目标后，继续巡逻，加 1 分

  ```C#
  private void Update() {
      if(player.player.GetComponent<CheckPlayerMove>().areaNum != playArea){
          int curArea = player.player.GetComponent<CheckPlayerMove>().areaNum;
          if(curArea > 0){
              actionManager.DirectMove(monster[curArea-1],player.player);
              if(playArea > 0){
                  actionManager.RandomMovePatrol(monster[playArea-1]);
                  judgement.addScore();
              }
          }
          else{
              actionManager.RandomMovePatrol(monster[playArea-1]);
              judgement.addScore();
          }
          playArea = curArea;
      }
  }
  ```

  追击动作

  先旋转面向Player，然后MoveTowards

  ```C#
  public class DirectMoveAction : SSAction
  {
      public GameObject target;
      public float speed = 2f;
  
      public static DirectMoveAction GetSSAction(GameObject gameObject,GameObject _target,ISSActionCallback _callback){
          DirectMoveAction action = ScriptableObject.CreateInstance<DirectMoveAction>();
  
          action.target = _target;
          action.gameobject = gameObject;
          action.transform = gameObject.transform;
          action.callback = _callback;
  
          return action;
      }
  
      public override void Update(){
          this.transform.LookAt(target.transform.position);
          this.transform.position = Vector3.MoveTowards(this.transform.position,target.transform.position,speed* Time.deltaTime);
  
      }
  
      public override void FixedUpdate(){}
      public override void Start(){}
  }
  ```

#### ActionManager添加动作

- 除此之外，由于巡逻兵只同时存在一个动作，追击/巡逻，所以添加动作时需要删除已存在的该对象的动作

  ```c#
  public void addAction(SSAction action){
      for(int i = 0;i<runSequence.Count;i++){
          if(runSequence[i].gameobject.Equals(action.gameobject)){
              runSequence.Remove(runSequence[i]);
              break;
          }
      }
      runSequence.Add(action);
  }
  ```

  

