# HW2_离散仿真引擎基础

[TOC]

## 1. 简答题

### 1.1 
- 解释 游戏对象（GameObjects） 和 资源（Assets）的区别与联系
> 游戏中的每个对象都是一个**游戏对象 (GameObject)**。然而，游戏对象 (GameObjects) 本身不做任何事情。它们需要**特殊属性 (special properties)**才能成为一个角色、一种环境或者一种特殊效果。但是每个对象要做很多不同的事情。
> 
> 游戏对象 (GameObjects) 是一种**容器**。它们是**空盒**，能够容纳组成一个光照贴图的岛屿或物理驱动的小车的不同部分。因此，要真正理解游戏对象 (GameObjects)，就必须了解这些组成部分（称为“组件 (Components)”）。根据您要创建的对象类型，您可以添加不同的组件 (Components) **组合**到游戏对象 (GameObject) 中。您也可以使用脚本 (Scripts) 制作自己的组件 (Components)。

>Assets（资源）是可以在您的游戏或项目中使用的任何项目的表示。Assets可能来自Unity之外创建的文件，例如3D模型，音频文件，图像或Unity支持的任何其他类型的文件。还可以在Unity中创建一些资源类型，例如`Animator Controller`，`Audio Mixer`或`Render Texture`。

>区别：GameObject是游戏中实际使用的对象,而Assets则是游戏项目文件夹中所需要堆放的资源
>
>联系：GameObject是游戏中实际使用的对象，是由Asset实例化后的对象。本质上其实还是Asset的衍变，是对部分Asset的引用和复制出来的新东西，其本质还是Asset。
>Asset和Object之间的关系是一对多关系，任何Asset文件中都含有一个或多个Object。
>![relation](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW2/pic/relation.PNG)

### 1.2 
- 下载几个游戏案例，分别总结资源、对象组织的结构（指资源的目录组织结构与游戏对象树的层次结构）
到[github](https://github.com/Goddyd/SpaceShooter/tree/master/SpaceShooter/Assets)上找了几个官方的案例
可以看到
![asset](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW2/pic/asset.png)
![base](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW2/pic/base.png)
>所以说，在根目录下，是将项目中所使用的资源、设置、库等按文件夹分开，而在资源目录下，又分别根据Audio/Material/Scripts等不同资源类型来划分

### 1.3
- 编写一个代码，使用 debug 语句来验证 MonoBehaviour 基本行为或事件触发的条件
    - 基本行为包括 Awake() Start() Update() FixedUpdate() LateUpdate()
    - 常用事件包括 OnGUI() OnDisable() OnEnable()
    ```c#
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class test : MonoBehaviour
    {
        void Awake()
        {
            Debug.Log("Awake");
        }
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Start");
        }
    
        // Update is called once per frame
        void Update()
        {
            Debug.Log("Update");
        }
        void FixedUpdate () 
        {
            Debug.Log("FixedUpdate");  
        }
        void LateUpdate () 
        {
            Debug.Log("LateUpdate");  
        }
        void OnGUI () 
        {
            Debug.Log("OnGUI");  
        }
        void OnDisable () 
        {
            Debug.Log("OnDisable");  
        }
        void OnEnable () 
        {
            Debug.Log("OnEnable");  
        }
    }
    ```
    
    测试结果：
    
    ![result](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW2/pic/result.png)
    
    结合[官方文档所给图片](https://docs.unity3d.com/Manual/ExecutionOrder.html)以及课上所总结
    
    | 事件名称   | 执行条件或时机                                        |
    | ---------- | ----------------------------------------------------- |
    | Awake      | 当一个脚本实例被载入时Awake被调用。或者脚本构造时调用 |
    | Start      | 第一次进入游戏循环时调用                              |
    | FixUpdate  | 每个游戏循环，由物理引擎调用                          |
    | Update     | 所有 Start 调用完后，被游戏循环调用                   |
    | LastUpdate | 所有 Update 调用完后，被游戏循环调用                  |
    | OnGUI      | 游戏循环在渲染过程中，场景渲染之后调用                |
    | OnDisable  | 当物体被激活的时候执行                                |
    | OnEnable   | 当物体被取消激活的时候执行                            |
    
### 1.4

- 查找脚本手册，了解 [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)，Transform，Component 对象
  
    - 分别翻译官方对三个对象的描述（Description）
    
      - GameObjects
    
        > The **GameObject** is the most important concept in the Unity Editor.
        >
        > Every object in your game is a **GameObject**, from characters and collectible items to lights, **cameras** and special effects. However, a GameObject can’t do anything on its own; you need to give it properties before it can become a character, an environment, or a special effect.
        >
        > To give a GameObject the properties it needs to become a light, or a tree, or a camera, you need to add [components](https://docs.unity3d.com/Manual/Components.html) to it. Depending on what kind of object you want to create, you add different combinations of components to a GameObject.
        >
        > You can think of a GameObject as an empty cooking pot, and components as different ingredients that make up the recipe of your game. Unity has lots of different built-in component types, and you can also make your own components using the [Unity Scripting API](https://docs.unity3d.com/Manual/CreatingComponents.html).
        >
        > **游戏对象 (GameObjects)** 是 Unity 中最重要的对象。
        >
        > 游戏中的每个对象，从角色和收藏品到灯光，相机和特效，都是GameObject。然而，游戏对象 (GameObjects) 本身不做任何事情。它们需要特殊属性 (special properties) 才能成为一个角色、一种环境或者一种特殊效果。
        >
        > 根据您要创建的对象类型，您可以添加不同的组件 (Components) 组合到游戏对象 (GameObject) 中。
        >
        > 想象一个游戏对象 (GameObject) 是一口空烹饪锅，组件 (Components) 是不同的作料，它们构成了您的游戏食谱。您也可以使用脚本 (Scripts) 制作自己的组件 (Components)。
    
      - Transform
    
        > The **Transform** component determines the **Position**, **Rotation**, and **Scale** of each object in the **scene**. Every **GameObject** has a Transform.
        >
        > ![transform](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW2/pic/transform.png)
        >
        > “变换”（Transform）组件确定场景中每个对象的“位置”，“旋转”和“缩放”。每个GameObject有变换组件。
    
      - Component
    
        > **Components** are the nuts & bolts of objects and behaviors in a game. They are the functional pieces of every **GameObject**. If you don’t yet understand the relationship between Components and GameObjects, read the [GameObjects](https://docs.unity3d.com/Manual/GameObjects.html) page before going any further.
        >
        > A GameObject is a container for many different Components. By default, all GameObjects automatically have a **Transform** Component. This is because the Transform dictates where the GameObject is located, and how it is rotated and scaled. Without a Transform Component, the GameObject wouldn’t have a location in the world. Try creating an empty GameObject now as an example. Click the **GameObject->Create Empty** menu item. Select the new GameObject, and look at the **Inspector**.
        >
        > Remember that you can always use the Inspector to see which Components are attached to the selected GameObject. As Components are added and removed, the Inspector will always show you which ones are currently attached. You will use the Inspector to change all the properties of any Component (including scripts).
        >
        > 组件是游戏中对象和行为的基本要素。它们是每个GameObject的功能部分。
        >
        > GameObject是许多不同组件的容器。默认情况下，所有GameObjects都自动拥有一个Transform Component。这是因为Transform指示了GameObject的位置，以及它是如何旋转和缩放的。如果没有变换组件，GameObject将没有世界上的位置。
        >
        > 请记住，您始终可以使用Inspector查看哪些组件附加到选定的GameObject。随着组件的添加和删除，Inspector将始终显示当前连接的组件。您将使用Inspector更改任何Component（包括脚本）的所有属性。
    
      - 描述下图中 table 对象（实体）的属性、table 的 Transform 的属性、 table 的部件
    
      - 本题目要求是把可视化图形编程界面与 Unity API 对应起来，当你在 Inspector 面板上每一个内容，应该知道对应 API。
      - 例如：table 的对象是 GameObject，第一个选择框是 activeSelf 属性。
    
    - 用 UML 图描述 三者的关系（请使用 UMLet 14.1.1 stand-alone版本出图）
      
      
### 1.5

  - 资源预设（Prefabs）与 对象克隆 (clone)
    - 预设（Prefabs）有什么好处？
    
      > **预设 (Prefab)** 是一种资源 - 存储在**工程视图 (Project View)** 中可重复使用的**游戏对象 (GameObject)**。预设 (Prefabs) 可放入到多个场景中，且每个场景可使用多次。向场景添加一个预设 (Prefab) 时，就会创建它的一个**实例**。所有预设 (Prefab) 实例都链接到原始预设 (Prefab)，实质上是原始预设的克隆。不管您的工程中有多少个实例，您对预设 (Prefab) 作薄出任何更改时，您会看到这些更改应用于所有实例。
    
    - 预设与对象克隆 (clone or copy or Instantiate of Unity Object) 关系？
    
    - 制作 table 预制，写一段代码将 table 预制资源实例化成游戏对象

## 2. 编程实践，小游戏

- 游戏内容： 井字棋 或 贷款计算器 或 简单计算器 等等
- 技术限制： 仅允许使用 **IMGUI** 构建 UI
- 作业目的：
  - 了解 OnGUI() 事件，提升 debug 能力
  - 提升阅读 API 文档能力

作业和具体报告已上传至[GitHub]()

## 3.思考题

- 微软 XNA 引擎的 Game 对象屏蔽了游戏循环的细节，并使用一组虚方法让继承者完成它们，我们称这种设计为“模板方法模式”。

  - 为什么是“模板方法”模式而不是“策略模式”呢？

  > 模板方法模式
  >
  > 定义一个操作中的算法的骨架，而将一些步骤延迟到子类中。模板方法使得子类可以不改变一个算法的结构即可重定义该算法的某些特定步骤。主要用于解决 ”一些方法通用，却在每一个子类都重新写了这一方法“ 的问题。

  > 策略模式
  >
  > 在策略模式（Strategy Pattern）中，一个类的行为或其算法可以在运行时更改。这种类型的设计模式属于行为型模式。在策略模式中，我们创建表示各种策略的对象和一个行为随着策略对象改变而改变的 context 对象。策略对象改变 context 对象的执行算法。
  >
  > 主要体现为：定义一系列的算法,把它们一个个封装起来, 并且使它们可相互替换。
  >
  > 用于解决”在有多种算法相似的情况下，使用 if...else 所带来的复杂和难以维护“的问题

  而对于微软的XNA引擎中的Game对象只是使用了一组虚方法让继承者们完成，即将实现延迟到子类中实现，符合模板方式模式的定义，而策略模式指的是已经实现的方法可以随着运行的改变而改变。

- 将游戏对象组成树型结构，每个节点都是游戏对象（或数）。

  - 尝试解释组合模式（Composite Pattern / 一种设计模式）。

    > 组合模式（Composite Pattern），又叫部分整体模式，是用于把一组相似的对象当作一个单一的对象。组合模式依据树形结构来组合对象，用来表示部分以及整体层次。这种类型的设计模式属于结构型模式，它创建了对象组的树形结构。这种模式创建了一个包含自己对象组的类。该类提供了修改相同对象组的方式。
    >
    > 主要体现为将对象组合成树形结构以表示"部分-整体"的层次结构。组合模式使得用户对单个对象和组合对象的使用具有一致性。

  - 使用 BroadcastMessage() 方法，向子对象发送消息。你能写出 BroadcastMessage() 的伪代码吗

- 一个游戏对象用许多部件描述不同方面的特征。我们设计坦克（Tank）游戏对象不是继承于GameObject对象，而是 GameObject 添加一组行为部件（Component）

  - 这是什么设计模式？

    > Decorator Pattern
    >
    > 允许向一个现有的对象添加新的功能，同时又不改变其结构。创建了一个装饰类，用来包装原有的类，并在保持类方法签名完整性的前提下，提供了额外的功能。

  - 为什么不用继承设计特殊的游戏对象？

    继承虽然可以很方便得复用父类的方法，但是如果出现十分相近的工作时，即使能通过复用来减少工作量，但随着需求的增加，也很容易导致代码量快速增加。

    继承的缺点：

    - 侵入性：继承是侵入性的，子类强制继承父类的方法和属性
    - 灵活性：降低代码的灵活性，子类必须拥有父类的属性和方法，子类收到了父类的约束，这是从子类的角度讲得
    - 耦合性：增强了耦合性，父类的属性和方法被修改时，还需要顾及其子类，可能会带来大量的重构，这是从父类的角度讲的