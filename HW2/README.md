## 井字棋 - Unity3D作业

相信大家对于 `井字棋` 这个游戏一定不陌生，今天我们就用Unity3D做一款简单的井字棋小游戏。

[TOC]

## 游戏介绍

实现了简单的 player VS computer 的井字棋对战

## 游戏界面

![home](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW2/pic/home.PNG)

![Game](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW2/pic/game.PNG)

![playing](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW2/pic/playing.PNG)

![end](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW2/pic/end.png)

## 游戏要素

### 场景Scene

这里我设想的是两个界面——分别是首页(Home)和游戏界面(Game)

![scenes](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW2/pic/scenes.PNG)

### 脚本Scripts

![scripts](https://raw.githubusercontent.com/wywwwwei/3DGameProgramming/master/HW2/pic/scripts.PNG)

每个场景对应一个负责OnGUI()的脚本

Home -> LoadHome

Game -> LoadGame

GamePlay 则是负责实现一些游戏的控制（电脑的移动/胜利的检测等等）

### Scene 0 —— Home 

首先Home界面比较简单，就由

- 标题(TicTacToe)

  > 由于初始字体比较小，所以需要通过GUIStyle来设置字体的属性

  ```objective-c
  //Add Title
  float titleWidth = 100;
  float titleHeight = 50;
  GUIStyle fontStyle= new GUIStyle();
  fontStyle.alignment = TextAnchor.MiddleCenter;
  fontStyle.fontSize = 40;
  fontStyle.normal.textColor = Color.red;
  GUI.Label(new Rect((screenWidth-titleWidth)/2, (screenHeight-titleHeight)*2/5, 		  titleWidth, titleHeight), "Tic Tac Toe",fontStyle);
  ```

- 开始游戏/退出 按键

  > 值得注意一点是，Unity3D现在建议使用SceneManagement中的SceneManager来实现场景的切换，如果依然使用Application.LoadLevel，Unity就会发出警告
  >
  > ![warn](pic/Warn-1568395292091.PNG)

  ```objective-c
  //Add Button and click event
  float buttonWidth = 100;
  float buttonHeight = 50;
  float spaceBetweenButton = 30;
  if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2-buttonWidth, (screenHeight-buttonHeight)*3/4, buttonWidth, buttonHeight), "Start Game"))
  {
  		OnStartClick();
  }
  if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2+buttonWidth, (screenHeight-buttonHeight)*3/4, buttonWidth, buttonHeight), "Quit"))
  {
  		OnQuitClick();
  }
  
   //Click event: scene switch
  void OnStartClick()
  {
  		SceneManager.LoadScene("Game");
  }
  
  void OnQuitClick()
  {
  		Application.Quit();
  }
  ```

### Scene 1 —— Game

- 返回主页 / 重新游戏 按键

  ```objective-c
  //Add Button
  float buttonWidth = 100;
  float buttonHeight = 50;
  float spaceBetweenButton = 30;
  if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2-buttonWidth, (screenHeight-buttonHeight)*4/5, buttonWidth, buttonHeight), "Back"))
  {
  		OnBackClick();
  }
  if(GUI.Button(new Rect((screenWidth-spaceBetweenButton)/2+buttonWidth, (screenHeight-buttonHeight)*4/5, buttonWidth, buttonHeight), "Restart"))
  {
  		OnRestartClick();
  }
  
  void OnBackClick()
  {
  		SceneManager.LoadScene("Home");
  }
  
  void OnRestartClick()
  {
  		GUI.enabled = true;
  		controller.restart();
  }
  ```

- 结束提示

  > 根据GamePlay.cs的结果检测的返回值判定输出结果，并且在游戏结束后通过`GUI.enabled=false`禁用GUI控件（根据GUI.enabled的启用和禁用放在不同位置，可起到禁用部分控件的作用，比如这里只禁用了井字棋盘）

  ```objective-c
  //Render result
  GamePlay.Result cur_res = controller.ifWin();
  if(cur_res!=GamePlay.Result.Gaming)
  {
          controller.setIngame(false);
          float resultWidth = 500;
          float resultHeight = 50;
          string cur_text=" ";
          GUIStyle fontStyle= new GUIStyle();
          fontStyle.alignment = TextAnchor.MiddleCenter;
          fontStyle.fontSize = 40;
          fontStyle.normal.textColor = Color.red;
  		switch(cur_res){
  			case GamePlay.Result.Win:
  				cur_text="You Win!!!";
  				break;
  			case GamePlay.Result.Lose:
  				cur_text="You Lose!!!";
  				break;
  			case GamePlay.Result.Draw:
  				cur_text="Draw!!!";
  				break;
  		}
  		GUI.Label(new Rect((screenWidth-resultWidth)/2, (screenHeight-resultHeight)*2/7, resultWidth, resultHeight), cur_text,fontStyle);
  		GUI.enabled = false;
  }
  ```

- 九宫格绘制

  > 根据GamePlay.cs的棋盘（多维数组）返回值，依照数组中所存储的枚举值确认是属于`Player1`/`Player2`/`Empty`，然后对应符号绘制棋盘

  ```objective-c
  //Render nine palace map
  float perGridWidth = 70;
  float perGridHeight = 70;
  float baseX = (screenWidth-perGridWidth)/2-perGridWidth;
  float baseY = screenHeight/6;
  for(int i = 0;i < 3;i++)
  {
  		for(int j = 0;j < 3;j++)
  		{
              string text;
              GamePlay.Status temp = controller.getMap(i,j);
              if(temp==GamePlay.Status.Player1)
              	text = "X";
              else if(temp==GamePlay.Status.Player2)
              	text = "O";
              else
              	text = " ";
              if(GUI.Button(new Rect(baseX+j*perGridWidth, baseY+i*perGridHeight, perGridWidth, perGridHeight), text))
             	{
  				OnGridClick(i,j);
  			}
  		}
  } 
  GUI.enabled = true;//起到部分禁用的效果
  ```

- 九宫格点击事件

  > 先判断所点击棋盘是否为空，非空则无反应，空则将所点击按键对应的数组值改为`Player1`，在调用GamePlay.cs中的autoOperate()，走出computer即`Player2`的一步。

  ```objective-c
  void OnGridClick(int i,int j)
  {
          if(!controller.ifIngame())return;
          if(controller.getMap(i,j)==GamePlay.Status.Empty)
          {
          	controller.setMap(i,j,GamePlay.Status.Player1);
          	controller.setFirst(false);//该变量本来想用于先/后手，结果没时间实现……
          	controller.autoOperate();
          }
  }
  ```

### Scripts —— GamPlay

- 存储棋盘信息

  通过枚举值

  ```objective-c
  public enum Status {Empty,Player1,Player2};
  ```

  建立一个二维3*3的数组记录

  Empty -> 尚未放置 

  Player1 -> 玩家放置

  Player2 -> 电脑放置

- 检测是否结束

  通过枚举值返回当前结果

  ```objective-c
  public enum Result {Gaming,Draw,Win,Lose};
  ```

  Gaming -> 游戏中（尚未结束）

  Draw -> 平局

  Win -> 玩家胜出

  Lose -> Computer胜出

  检测方法：先检每一行/每一列/对角线是否有全由`Player1`或`Player2`组成的，有则返回胜利者`Win`/`Lose`，否则继续检测棋盘是否已满，是则返回`Draw`，否则返回`Gaming`

- 模拟Computer

  电脑模拟的走法很简单，就是如果能凑到三个（即一条线上已经有2个己方位置），则优先进攻（放置趋向于赢的位置），如果找不到这样的位置，则趋向于防守（不让玩家胜出），如果既没有进攻，也没有防守的位置，则通过随机函数确定下一位置。