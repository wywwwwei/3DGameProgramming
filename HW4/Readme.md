# 牧师与魔鬼 动作分离版

> 场记（控制器）管的事太多，不仅要处理用户交互事件，还要游戏对象加载、游戏规则实现、运动实现等等，而显得非常臃肿。一个最直观的想法就是让更多的人（角色）来管理不同方面的工作。显然，这就是面向对象基于职责的思考，例如：用专用的对象来管理运动，专用的对象管理播放视频，专用的对象管理规则。就和踢足球一样，自己踢5人场，一个裁判就够了，如果是国际比赛，就需要主裁判、边裁判、电子裁判等角色通过消息协同来完成更为复杂的工作。

[TOC]

## 动作管理器的设计

为了用一组简单的动作组合成复杂的动作，设计思路如下：

- 设计一个抽象类作为游戏动作的基类
- 设计一个动作管理器类管理一组游戏动作的实现类
- 通过回调，实现动作完成时的通知

这样的目的是让程序可以方便的定义动作并实现动作的自由组合，使得：

- 程序更能适应需求变化
- 对象更容易被 **复用**
- 程序更易于维护

![action-design](pics/action-design.png)

## 核心代码与设计解读

由于这里我们需要实现的AciotnManager只有一个，所以不分别创建SSActionManager  和CCActionManager  

首先我们要确定好 **事件的调用和返回顺序**

1. SSActionManager  创建并添加一个 CCMoveToAction事件 

   -> SSActionManager执行单个CCMoveToAction

   -> CCMoveToAction执行完成 

   -> 通知SSActionManager继续下一个事件

   

   所以此时  CCMoveToAction 的 回调对象 是 SSActionManager

   

2. SSActionManager  创建多个 CCMoveToAction事件 

   -> SSActionManger创建并添加一个CCSequenceAction  

   -> SSActionManager执行单个CCSequenceAction 

   -> CCSequenceAction 执行序列中单个CCMoveToAction  

   -> CCMoveToAction执行完成 

   -> 通知CCSequenceAction 继续下一个事件 -> …… 

   -> CCSequenceAction 执行完成 

   -> 通知SSActionManager继续下一个事件

   

   所以此时  CCMoveToAction 的 回调对象 是 CCSequenceAction ,CCSequenceAction 的 回调对象是 SSActionManager  

### 动作基类（SSAction）

目的是为了创建一个接口，便于多种事件的实现不会影响事件的处理

要点：

- [ScriptableObject](https://docs.unity3d.com/ScriptReference/ScriptableObject.html) 是不需要绑定 GameObject 对象的可编程基类。这些对象受 Unity 引擎场景管理
- `protected` 防止用户自己 new 对象
- 使用 `virtual` 申明虚方法，通过重写实现多态。这样继承者就明确使用 Start 和 Update 编程游戏对象行为
- 利用接口（`ISSACtionCallback`）实现消息通知，避免与动作管理者直接依赖

这里代码和老师博客所给一致

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SSAction : ScriptableObject{
    public bool enable = true;
    public bool destroy = false;

    public GameObject gameobject{get;set;}
    public Transform transform{get;set;}
    public ISSActionCallback callback{get;set;}

    protected SSAction(){}

    //Use this for initialization
    public virtual void Start(){
        throw new System.NotImplementedException();
    }

    //Update is called once per frame
    public virtual void Update(){
        throw new System.NotImplementedException();
    }
}
```

### 简单动作实现

实现具体动作，将一个物体移动到目标位置，并通知任务完成：

依然是和老师博客大致一致，修改GetSSAction的参数，增加一个手动设置的回调对象

要点：

- 让 Unity 创建动作类，确保内存正确回收。
- 多态。C++ 语言必申明重写，Java则默认重写
- 似曾相识的运动代码。动作完成，则期望管理程序自动回收运行对象，并发出事件通知管理者。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CCMoveToAction : SSAction
{
    public Vector3 target;
    public float speed;

    public static CCMoveToAction GetSSAction(GameObject gameObject,Vector3 _target,float speed,ISSActionCallback _callback){
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();

        action.target = _target;
        action.speed = speed;
        action.gameobject = gameObject;
        action.transform = gameObject.transform;
        action.callback = _callback;

        return action;
    }

    public override void Update(){
        this.transform.position = Vector3.MoveTowards(this.transform.position,target,speed* Time.deltaTime);
        if(this.transform.position == target){
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
        
    }

    public override void Start(){}
}
```

### 顺序动作组合类实现

实现一个动作组合序列，顺序播放动作：

要点：

- CCMoveToAction 的 回调对象 是 CCSequenceAction ,CCSequenceAction 的 回调对象是 SSActionManager  
- 让动作组合继承抽象动作，能够被进一步组合；实现回调接受，能接收被组合动作的事件
- 创建一个动作顺序执行序列，-1 表示无限循环，start 开始动作。
- `Update`方法执行执行当前动作
- `SSActionEvent` 收到当前动作执行完成，推下一个动作，如果完成一次循环，减次数。如完成，通知该动作的管理者
- `Start` 执行动作前，为每个动作注入当前动作游戏对象，并将自己作为动作事件的接收者
- `OnDestory` 如果自己被注销，应该释放自己管理的动作。

这是标准的组合设计模式。被组合的对象和组合对象属于同一种类型。通过组合模式，我们能实现几乎满足所有越位需要、非常复杂的动作管理。

大致和老师博客代码一致

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CCSequenceAction : SSAction,ISSActionCallback
{
    public List<SSAction> sequence;
    public int repeat = 1;
    public int start = 0;

    public static CCSequenceAction GetSSAction(int repeat,int start,List<SSAction> sequence,ISSActionCallback _callback){
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        action.callback = _callback;
        return action;
    }

    //Update is called once per frame
    public override void Update(){
        if(sequence.Count ==0)return;
        if(start < sequence.Count){
            sequence[start].Update();
        }
    }

    public void SSActionEvent(SSAction source,SSActionEventType events = SSActionEventType.Competeted,int intParam = 0,string strParam = null)
    {
        source.destroy = false;
        this.start++;
        if(this.start>=sequence.Count){
            this.start = 0;
            if(repeat>0)repeat--;
            if(repeat==0){
                destroy = true;
                this.callback.SSActionEvent(this);
            }
        }
    }

    public override void Start(){
        foreach (SSAction action in sequence)
        {
            action.callback = this;
            action.Start();
        }
    }

    private void OnDestroy() {
        foreach (SSAction action in sequence){
            Destroy(action);
        }
    }
}

```

### 动作事件接口定义

和老师博客代码一致，删除参数中的Object（Unity不知道为什么报错）

```
public enum SSActionEventType:int{Started,Competeted}

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source,
    SSActionEventType events = SSActionEventType.Competeted,
    int intParam = 0,
    string strParam = null);
}
```

### 动作管理基类 – SSActionManager

这里我并不将他作为基类，因为所要实现的东西太少了，所以只有一个动作管理类

建立 任务队列，实现 添加任务 ， 移动船 ， 移动牧师，移动恶魔 （其实这里如果牧师与恶魔实现多态，可以致谢一个函数）

```c#
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
```

### 裁判类

裁判类里除了根据Controller的游戏状态判断胜负之外，还负责控制计时器

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameStatus { Gaming , Win , Lose};

public class Judge: MonoBehaviour {
    public enum GameStatus { Gaming , Win , Lose};
    private int timer = 60;
    private GameStatus cur_status;
    private Coroutine runTimer;

    public GameStatus getCurStatus(){
        return cur_status;
    }
    public void checkWinOrLose(int priestLeftNum ,int priestRightNum,int devilLeftNum,int devilRightNum)
    {
        if(priestLeftNum + devilLeftNum == 6){
            cur_status = GameStatus.Win;
        }
        if(priestLeftNum != 0 && priestLeftNum<devilLeftNum)
        {
            cur_status = GameStatus.Lose;
        }
        if(priestRightNum!=0 && priestRightNum<devilRightNum){
            cur_status = GameStatus.Lose;
        }
        else{
            cur_status = GameStatus.Gaming;
        }
    }
    public IEnumerator waitForOneSecond()
    {
        while (cur_status==Judge.GameStatus.Gaming && timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer--;
            if(timer==0){
                cur_status = Judge.GameStatus.Lose;
            }   
        }
    }
    public int getTimer(){
        return timer;
    }

    public void startTimer(){
        cur_status = GameStatus.Gaming;
        timer = 60;
        runTimer = StartCoroutine(waitForOneSecond());
    }
    public void stopTimer(){
        if(runTimer!=null){
            StopCoroutine(runTimer);
        }
    }
}
```

