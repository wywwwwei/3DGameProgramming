# 空间与运动

[TOC]

> 游戏世界是一个虚拟的世界、假想的世界、甚至其中部分是真实的世界。玩家控制部分游戏物体，按游戏世界或现实世界的规则交互，以达成设定目标。当虚拟世界与现实世界交融时，我们就称为增强现实（Augmented Reality）游戏。不管游戏世界是虚拟的或现实的，游戏世界中所有物体（游戏对象）都必须在特定的空间、时间下出现、变化、消失。因此，游戏设计必须定义空间、时间等。 
>
> - 空间维度：
>   - 自由度：2d 或 2d卷轴，3d, 2.5d(如 Aircraft)，4d（？）
>   - 尺度：游戏世界的度量单位，如米、公里、光年。特别是其他物体与玩家对象的相对大小设计
>   - 边界：玩家可以看到的地图与场景
> - 时间维度
>   - 例如：唐朝、宋朝；石器时代、铜器时代、铁器时代、火器时代、太空时代；
> - 环境维度
>   - 时代与文化背景
>   - 艺术风格与形式
>   - 场景与物体搭配
> - 情感维度
> - 道德维度

> [TransForm 部件](https://docs.unity3d.com/ScriptReference/Transform.html) 
>
> - 位置、欧拉角、比例、旋转
>   - 世界坐标：position, eulerAngles, scale, rotation
>   - 相对坐标：localposition, local…
> - 对象空间轴（单位向量）
>   - up, right, forward
> - 空间依赖
>   - parent, childCount

## 1. 简答并用程序验证

### 1.1

- 游戏对象运动的本质是什么？

  游戏运动本质就是**使用矩阵变换（平移、旋转、缩放）**改变游戏对象的**空间属性**。

  > Unity中的所有游戏对象都有一个Transform组件。 这用于存储对象的位置，旋转和缩放，我们可以通过读取Transform组件来获得这些信息，也可以通过设置Transform组件以改变场景中游戏对象的位置，旋转或缩放。

  可以参考我们课上所写的几个移动的文件

  - 每秒钟使得游戏对象向左移动一个单位  -> 平移

    > Vector3 是一个类，Vector3.left 是单位常数，还有Vector3.forward（向前）/Vector3.up（向上）
    >
    > 使用负值反转方向

    ```c#
    public class MoveLeft : MonoBehaviour {
    	public int speed = 2;
    	// Update is called once per frame
    	void Update () {
    		this.transform.position += speed * Vector3.left * Time.deltaTime;
    	}
    }
    ```

    除了直接修改position属性之外，我们也能通过函数来间接实现，参考[官方文档](https://docs.unity3d.com/ScriptReference/Transform.Translate.html)

    > **Transform.Translate** 方法允许您根据方向和距离移动对象
    >
    > public void **Translate**([Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) **translation**);
    >
    > public void **Translate**([Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) **translation**, [Space](https://docs.unity3d.com/ScriptReference/Space.html) **relativeTo** = Space.Self);
    >
    > Space参数设置是否要在本地或世界空间中移动

    这里的移动方向除了可以使用`Vector3.forward`，`Vector3.right`或`Vector3.up`等世界方向之外，还可以使用局部方向（如 `myTransform.forward`，`myTransform`）

    ```c#
    //平移的另一种写法
    public class ExampleMove : MonoBehaviour {
    	public float speed = 2;
    	// Update is called once per frame
    	void Update () {
    		transform.Translate(speed * Vector3.left * Time.deltaTime)
    	}
    }
    ```

  - 旋转

    > 旋转的几种方式：欧拉旋转、四元数、矩阵旋转。而Unity主要使用的是四元数
    >
    > - Quaternion.Euler(x,y,z) 可以得到四元数表示
    > - 一个四元数也可以使用方法 eulerAngles 的到欧拉角

    [官方文档](https://docs.unity3d.com/ScriptReference/Transform.Rotate.html)

    > public void **Rotate**([Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) **eulers**, [Space](https://docs.unity3d.com/ScriptReference/Space.html) **relativeTo** = Space.Self);
    >
    > //以欧拉角旋转,顺序是ZXY
    >
    > public void **Rotate**(float **xAngle**, float **yAngle**, float **zAngle**, [Space](https://docs.unity3d.com/ScriptReference/Space.html) **relativeTo** = Space.Self);
    >
    > //按顺序分别绕Z,X,Y轴旋转
    >
    > public void **Rotate**([Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) **axis**, float **angle**, [Space](https://docs.unity3d.com/ScriptReference/Space.html) **relativeTo** = Space.Self);
    >
    > //以给定轴旋转给定角度

    ```c#
    //Transform.Rotate example
    public class ExampleRotate : MonoBehaviour{
        public float rotateSpeed = 2;
    	void Update(){
            //以每秒rotateSpeed的速度沿z轴旋转对象
            transform.Rotate(0,0,rotateSpeed * Time.deltaTime);
        }
    }
    ```
    
  - 缩放

    > localscale （较常用）本地坐标系的大小，也就是检视（Inspector）面板显示的数值
    >
    > lossyscale 世界坐标系的大小，没有父物体时检视面板显示的数值，也就是相对于世界的实际大小

    ```c#
    public class ExampleZoom : MonoBehaviour
    {
        void Update()
        {
            // 扩大物体的x轴向0.1个单位
            transform.localScale += new Vector3(0.1F, 0, 0);
        }
    }
    ```


### 1.2

- 请用三种方法以上方法，实现物体的抛物线运动。（如，修改Transform属性，使用向量Vector3的方法…）

  1. **通过 transform.Translate()平移实现**，因为抛物线运动是通过水平方向上的匀速运动和垂直方向上的恒加速度运动组合而成，所以我们只需分别在水平和垂直方向上修改Transform即可

     ```c#
     public class ThrowSimulation : MonoBehaviour
     {
         public float verticalAcceleration = 2.0f;
         public float horizonalVelocity = 3.0f;
         public float verticalVelocity = 0;
     
         // Update is called once per frame
         void Update()
         {
             verticalVelocity += verticalAcceleration * Time.deltaTime;
             this.transform.Translate(horizonalVelocity * Vector3.left * Time.deltaTime);
             this.transform.Translate(verticalVelocity* -Vector3.up* Time.deltaTime);
         }
     }
     ```

  2.  直接通过**修改position**实现

      ```c#
      public class ThrowSimulation : MonoBehaviour
      {
          public float verticalAcceleration = 2.0f;
          public float horizonalVelocity = 3.0f;
          public float verticalVelocity = 0;
      
          // Update is called once per frame
          void Update()
          {
              verticalVelocity += verticalAcceleration * Time.deltaTime;
              this.transform.position += horizonalVelocity * Vector3.left * Time.deltaTime;
              this.transform.position += verticalVelocity* -Vector3.up* Time.deltaTime;
          }
      }
      ```

  3. 通过**添加 `刚体` 组件**

     > [Rigidbody官方文档](https://docs.unity3d.com/ScriptReference/Rigidbody.html)
     >
     > 通过物理模拟控制物体的位置。将Rigidbody组件添加到对象将使其运动受Unity的物理引擎的控制。即使没有添加任何代码，如果右侧碰撞器组件也存在，Rigidbody对象将被重力向下拉，并将对与传入对象的碰撞作出反应。

     先在 Inspector 菜单 点击  `Add Component`，选择Rigidbody

     ![RigidBody](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW3/pics/Rigidbody.PNG)

     因为添加了重力属性，所以我们可以只设置水平位置上的位移/运动

     ```C#
     public class ThrowSimulation : MonoBehaviour
     {
         public float horizonalVelocity = 3.0f;
         
         // Update is called once per frame
         void Update()
         {
             this.transform.Translate(horizonalVelocity * Vector3.left * Time.deltaTime);
         }
     }
     ```

  4. 通过 **Vector3.Slerp() 球形插值**

     > 按数量t在a和b之间插值。这种和线性插值（又名“lerp”）之间的区别在于矢量被视为方向而不是空间中的点。返回矢量的方向由角度插值，其幅度在from和to的幅度之间插值。
     >
     > 于是我们可以通过在两点（出发点和结束点）进行球形插值来形成抛物线，我们可以通过调整两向量的中心向量，如例中的center，从而改变抛物线的弧度和方向。
     
     ```c#
     public class ThrowSimulation : MonoBehaviour
     {
         public Transform start;
         public Transform end;
         // Time to move from start to end position, in seconds.
         public float animationTime = 10.0f;
         // The time at which the animation started.
         private float startTime;
     
         void Start()
         {
             // Note the time at the start of the animation.
             startTime = Time.time;
             start = this.transform;
             this.transform.position = new Vector3(0,0,0);
             GameObject empty = new GameObject("empty");
             empty.transform.position = new Vector3(-10,0,0);
             end = empty.transform;
         }
     
         void Update()
         {
             // The center of the arc
             Vector3 center = (start.position + end.position) * 0.5F;
             // move the center a bit make the arc smoother
             center -= center.normalized + new Vector3(0,5f,0);
             // Interpolate over the arc relative to center
             Vector3 startRelCenter = start.position - center;
             Vector3 endRelCenter = end.position - center;
             // The fraction of the animation that has happened so far is
             // equal to the elapsed time divided by the desired time for
             // the total journey.
             float fracComplete = (Time.time - startTime) / animationTime;
     
             transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete);
             transform.position += center;
         }
     }
     ```

### 1.3

- 写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上。

  项目及报告地址：[Github](https://github.com/wywwwwei/3DGameProgramming/tree/master/HW3/Solar%20System)
  
  - 制作预设
  
    先在百度上搜索`太阳系贴图`，将找到的贴图保存到`Assets/Resources/Material`中，然后新建GameObject -> Sphere，将图片托上去，再将该对象拖到`Assets/Resources/Prehabs`中
  
    ![prehabs](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW3/pics/Prehabs.PNG)
  
  - 编写脚本
  
    - 动态加载预设
  
      利用`Resources.Load` 默认加载`Assets/Resources`目录下的资源文件，创建并实例化刚才制作的预设
  
      ![loadResources](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW3/pics/loadResources.PNG)
  
    - 设置初始位置
  
      ![setPos](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW3/pics/setPos.PNG)
  
    - 设置初始大小 —— 不做截图，可到项目中查看
  
    - 动态添加`拖尾/Trail Renderer`组件(便于观察轨迹)
  
      ![addComponent](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW3/pics/component.PNG)
  
    - 实现自转与公转
  
      - 自转
  
        ![rotation](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW3/pics/rotation.PNG)
  
      - 公转（通过改变 Axis 的  `向量方向` 不一样达到 `不同法平面` 的效果）
  
        ![revolution](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW3/pics/revolution.PNG)
  
    - Start()和Update()，组合上面的函数
    
      ![main](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW3/pics/main.PNG)
    
  - 效果展示
  
    > 由于调的速度过快(方便截图)所以，部分轨迹不太圆滑，先将脚本中方便演示的初始值删掉，再在Inspector面板调整给出的速度变量即可。
  
    ![track](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW3/pics/track.PNG)

## 2. 编程实践

- 阅读以下游戏脚本

  > Priests and Devils
  >
  > Priests and Devils is a puzzle game in which you will help the Priests and Devils to cross the river within the time limit. There are 3 priests and 3 devils at one side of the river. They all want to get to the other side of this river, but there is only one boat and this boat can only carry two persons each time. And there must be one person steering the boat from one side to the other side. In the flash game, you can click on them to move them and click the go button to move the boat to the other direction. If the priests are out numbered by the devils on either side of the river, they get killed and the game is over. You can try it in many > ways. Keep all priests alive! Good luck!

- 程序需要满足的要求：
  - play the game ( http://www.flash-game.net/game/2535/priests-and-devils.html )
  - 列出游戏中提及的事物（Objects）
  - 用表格列出玩家动作表（规则表），注意，动作越少越好
  - 请将游戏中对象做成预制
  - 在 GenGameObjects 中创建 长方形、正方形、球 及其色彩代表游戏中的对象。
  - 使用 C# 集合类型 有效组织对象
  - 整个游戏仅 主摄像机 和 一个 Empty 对象， **其他对象必须代码动态生成！！！** 。 整个游戏不许出现 Find 游戏对象， SendMessage 这类突破程序结构的 通讯耦合 语句。 **违背本条准则，不给分**
  - 请使用课件架构图编程，**不接受非 MVC 结构程序**
  - 注意细节，例如：船未靠岸，牧师与魔鬼上下船运动中，均不能接受用户事件！
  
  项目及**报告**地址：[Github]()

## 3. 思考题

- 使用向量与变换，实现并扩展 Tranform 提供的方法，如 Rotate、RotateAround 等

  > 完全可以将 Quaternion 看成一个非常直观的旋转矩阵，利用矩阵乘向量得到旋转后的位置，矩阵连乘得到复合旋转！API 提供静态方法 Quaternion.Euler(x,y,z)，直观上按 z、x、y 轴旋转的一个序列。数学上就是 `q = qy * qx * qz`。【注意】不可交换。
  
  - Rotate
  
    ```c#
    public void Rotate(Transform myTransform,Vector3 eulers){
    	Quaternion rotation = Quaternion.Euler(eulers);
        myTransform.rotation *= rotation;
    }
    
    public void Rotate(Transform myTransform,float xAngle, float yAngle, float zAngle){
        Quaternion p1 = Quaternion.AngleAxis(zAngle, Vector3.forward);
        Quaternion p2 = Quaternion.AngleAxis(xAngle, Vector3.right);
        Quaternion p3 = Quaternion.AngleAxis(yAngle, Vector3.up);
        myTransform.rotation *= p3 * p2 * p1;
    }
    public void Rotate(Transform myTransform,Vector3 axis, float angle){
    	Quaternion p1 = Quaternion.AngleAxis(angle, axis);
    	myTransform.rotation *= p1;
    }
    ```
  
  - RotateAround
  
    ```c#
    /*Rotates the transform about axis passing through point in world coordinates by angle degrees.*/
    public void RotateAround(Transform myTransform,Vector3 point, Vector3 axis, float angle){
       	Quaternion p1 = Quaternion.AngleAxis(angle,axis);
    	Vector3 distance = myTransform.position - point;
      	myTransform.position = p1 * myTransform.position;
      	myTransform.rotation *= p1;  
    }
    ```
  
    
  
  
