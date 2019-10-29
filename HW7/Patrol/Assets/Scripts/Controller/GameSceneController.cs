using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    public class GameSceneController:MonoBehaviour,ISceneController,IUserAction{
        private SSActionManager actionManager;
        private Judge judgement;
        private Player player;
        private int playArea = 0;
        private PatrolFactory factory;
        private GameObject[] monster = new GameObject[8];

        private void Awake() {
            Director director = Director.getInstance();
            director.setFPS(60);
            director.currentSceneController = this;

            judgement = Judge.getInstance();
            factory = PatrolFactory.getInstance();
            actionManager = this.gameObject.AddComponent<SSActionManager>();
            this.gameObject.AddComponent<UserGUI>();

            director.currentSceneController.LoadResources();
        }

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
        public void LoadResources(){
            GameObject map = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Plane"))as GameObject;
            player = new Player();
            GameObject.Find("Camera").GetComponent<CameraFollow>().setFollow(player.player.transform);
            for(int i = 0;i<8;i++){
                monster[i] = factory.getPatrol(i+1);
                Patrol.myGameOver += GameOver;
                actionManager.RandomMovePatrol(monster[i]);
            }
        }

        public void MovePlayer(float TranslationX,float TranslationY){
            actionManager.MovePlayer(player.player,TranslationX,TranslationY);
        }

        public GameStatus getCurStatus(){
            return judgement.getCurStatus();
        }

        public void restart(){}
        public void GameOver(){
            player.player.GetComponent<Animator>().SetInteger("toDo",5);
            judgement.setCurStatus();
            //Time.timeScale = 0f;
        }
        public void back(){
            Director.getInstance().LoadScene(0);
        }
    }
}