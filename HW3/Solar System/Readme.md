# Solar System

- 注意

  **请将C#脚本挂载到Main Camera上**

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

  > 由于调的速度过快(方便截图)所以，部分轨迹不太圆滑，在Inspector面板调整给出的速度变量即可

  ![track](pic/track.PNG)