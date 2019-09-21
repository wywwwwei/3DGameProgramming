## Priests-and-Devils - Unity3D作业

[TOC]

视频地址：[HERE](https://www.bilibili.com/video/av68546717/)

博客地址：[HERE](https://blog.csdn.net/WeiXiaoAssassin/article/details/101059789)

原游戏地址：[HERE](http://www.flash-game.net/game/2535/priests-and-devils.html)

> 作业要求：
>
> 整个游戏仅 主摄像机 和 一个 Empty 对象， **其他对象由代码动态生成**。
>
> 整个游戏不许出现 Find 游戏对象， SendMessage 这类突破程序结构的 通讯耦合 语句

### 列出游戏中提及的事物（Objects）

- Priest
- Devil
- Boat
- Other environmental objects  —— `River` 、 `Riverbank`

### 用表格列出玩家动作表（规则表），注意，动作越少越好

| 动作                 | 参数 | 结果                 |
| -------------------- | ---- | -------------------- |
| 启动游戏             |      | 初始界面             |
| 开始游戏（初始界面） |      | 游戏界面             |
| 重新开始（游戏界面） |      | 重新加载游戏界面资源 |
| 点击对象（游戏界面） | 对象 | 对象根据当前位置移动 |
| 返回（游戏界面）     |      | 初始界面             |

### 请将游戏中对象做成预制

![Prefabs](https://img-blog.csdnimg.cn/20190922000223510.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L1dlaVhpYW9Bc3Nhc3Npbg==,size_16,color_FFFFFF,t_70)

- Boat  -> Boat对象
- Devil -> Devil对象
- Priest -> Priest对象
- River -> River对象
- Solid -> Riverbank对象

### MVC 结构程序

> 面向对象设计的核心：**基于职责的设计**
>
> 即：模拟人类组织管理社会的方法，根据不同人拥有资源、知识与技能的不同，赋予不同人（或对象）特定的职责。再按一定结构（如设计模式），将它们组织起来。

先直接给出游戏框架设计:

![frame](https://img-blog.csdnimg.cn/20190922000245548.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L1dlaVhpYW9Bc3Nhc3Npbg==,size_16,color_FFFFFF,t_70)

整个程序都是基于该架构图实现

### 结构分析

- 导演 （SSDriector） —— 1个

  ```c#
  //把握全局；控制场景；负责场景的切换
  public class SSDirector : System.Object
  {
      private static SSDirector _instance;
      
      public ISceneController currentSceneController { get;set;}
  
      public bool running{ get; set;}
  
      public static SSDirector getInstance(){
          if(_instance == null ){
              _instance = new SSDirector();
          }
          return _instance;
      }
  
      public void LoadScene(int num){
          SceneManager.LoadScene(num);
      }
      public int getFPS(){
          return Application.targetFrameRate;
      }
  
      public void setFPS(int fps){
          Application.targetFrameRate = fps;
      }
  }
  ```

- 场景（Scene）-> 场记（ISceneController）—— 2个

  ![scenes](https://img-blog.csdnimg.cn/20190922000258606.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L1dlaVhpYW9Bc3Nhc3Npbg==,size_16,color_FFFFFF,t_70)

  抽象对象：ISceneController

  ```c#
  //加载资源，接受点击事件
  public interface ISceneController
  {
      void LoadResources();
      void getClick(Character.Type objType,int id);
  }
  ```

  具体对象：`GameSceneController `  和 `HomeSceneController`

   ```c#
  //负责游戏资源的加载，游戏进程胜负的判断，计时器等资源，点击事件管理
  //提供接口给GUI界面（View）
  public class GameSceneController : MonoBehaviour,ISceneController,IUserAction{
      void Awake() {
          SSDirector director = SSDirector.getInstance();
          director.setFPS(60);
          director.currentSceneController = this;
          this.gameObject.AddComponent<UserGUI>();
          director.currentSceneController.LoadResources();
      }
      void LoadResources(){}
      void getClick(Character.Type objType,int id){}
      void checkMove(Character.Type objType,int id){}
      bool checkLose(){}
      bool checkWin(){}
      void restart(){}
      void back(){}
      GameSceneController.GameStatus getCurStatus(){}
      IEnumerator waitForOneSecond(){}
      int getTimer(){}
  }
   ```

  ```c#
  //负责添加初始界面（HomeGUI）组件,接受按钮事件进行场景的切换
  public class HomeSceneController : MonoBehaviour,ISceneController,IHomeAction{
      void Awake() {
          SSDirector director = SSDirector.getInstance();
          director.setFPS(60);
          director.currentSceneController = this;
          director.currentSceneController.LoadResources();
          this.gameObject.AddComponent<HomeGUI>();
      }
      void startGame(){}
      void finish(){}
      void LoadResources(){}
      void getClick(Character.Type objType,int id){}
  }
  ```

- GameObject对象（Model）的管理

  - 共同点：

    位置 / 对象属于哪种GameObject：

    ```C#
    public class Character{
        public enum Status{leftLand,leftBoat,rightBoat,rightLand};
        public enum Type{classPriest,classDevil,classBoat};
    }
    ```

    移动：

    通过事先设置的 Vector3 确定该对象在不同状态的位置

    ```C#
    public class Devil{
        public static Vector3[] boatPos = new Vector3[2]{new Vector3(-1.1f,0.05f,-5.5f),new Vector3(-1.7f,0.05f,-5.5f)};
    }
    
    public class Priest{
        public static Vector3[] boatPos = new Vector3[2]{new Vector3(-1.1f,0.2f,-5.5f),new Vector3(-1.7f,0.2f,-5.5f)};
    }
    
    public class Boat{
        public static Vector3[] boatPos = new Vector3[2]{new Vector3(-1.5f,-0.2f,-5.5f),new Vector3(1.5f,-0.2f,-5.5f)};
    }
    ```

  - 区别：

    - `Priest` 和`Devil`都需要设置从船上到岸上/岸上到船上的移动
    - `Boat` 需要负责为`Priest`和`Devil`分配船上的座位

  根据它们职责的不同在 `Model` 层写好对应的函数

- View管理

  在`View`界面需要定义好用户动作的接口，再实现对应的GUi页面

  - `IUserAction`/`IHomeAction`

    ```c#
    public interface IUserAction
    {
        void restart();			//重新开始
        void back();			//回到初始界面
        GameSceneController.GameStatus getCurStatus();	//返回当前游戏状态
        int getTimer();			//计时器显示 接口
    }
    ```

    ```c#
    public interface IHomeAction
    {
        void startGame();		//开始游戏
        void finish();			//退出游戏
    }
    ```

  - 对应的GUI页面 `UserGUI`/`HomeGUI`

    共同点：

    - 在OnGUI()中增加`button`/`Label`的GUI控件
    - 根据获取`Controller`中的游戏状态（接口），达到显示不同界面的效果
    - 通过获取`SSDirector`单实例实现将当前场景的控制器作为`IuserAction`/`IHomeAction`从而达到获取控制器的接口的效果

- 自定义组件

  `ToMove`组件，使游戏对象的移动不是瞬移，而是渐进的(`MoveTowards`)

  其中最重要的Update()函数

  ```c#
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
  ```

  `ButtonSimulation` 组件，使游戏对象可以被点击，从而使控制器接收到点击事件

  ```c#
  public class ButtonSimulation : MonoBehaviour
  {
      public Character.Type objType;
      public int id;
  
      void OnMouseDown(){
          Debug.Log("OK!!");
          SSDirector.getInstance().currentSceneController.getClick(objType,id);
      }
  
      void Start(){}
  
      void Update(){}
  }
  ```

### 游戏界面

![game](https://img-blog.csdnimg.cn/20190922000314530.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L1dlaVhpYW9Bc3Nhc3Npbg==,size_16,color_FFFFFF,t_70)