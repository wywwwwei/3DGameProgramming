using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
{
    public class GameSceneController:MonoBehaviour,ISceneController,IUserAction{
        public int curTrialUFO;
        public SSActionManager actionManager;
        public List<GameObject> waitToFly=new List<GameObject>();
        public List<GameObject> haveFly=new List<GameObject>();
        public Ruler ruler = new Ruler();
        public Judge judgement;

        void Awake() {
            Director director = Director.getInstance();
            director.setFPS(60);
            director.currentSceneController = this;

            judgement = Judge.getInstance();

            loadInitSetting();
            this.gameObject.AddComponent<UserGUI>();
            actionManager = this.gameObject.AddComponent<SSActionManager>();
            this.gameObject.AddComponent<checkClick>();
            director.currentSceneController.LoadResources();
        }
        public void loadInitSetting(){
            judgement.setRound(1);
            judgement.setTrial(1);
            curTrialUFO = ruler.getUFOCount(judgement.getRound());
        }
        public void LoadResources(){
            curTrialUFO = ruler.getUFOCount(judgement.getRound());
            getUFO();
        }
        void getUFO(){
            for(int i = 0;i < curTrialUFO;i++){
                Debug.Log("fly");
                waitToFly.Add(UFOFactory.getInstance().getUFO(judgement.getRound()));
            }
            setFly();
        }
        void setFly(){
            float waitTime = 0;
            for(int i = 0;i < waitToFly.Count;i++){
                if( i == 0 ){
                    actionManager.FlyUFO(waitToFly[i],ruler,judgement.getRound());
                    haveFly.Add(waitToFly[i]);
                }else{
                    StartCoroutine(Fly(waitTime,waitToFly[i]));
                }
                waitTime += ruler.getIntervals(judgement.getRound());
            }
            StartCoroutine(Next(waitTime));
            waitToFly.Remove(waitToFly[0]);
        }
        IEnumerator Fly(float time,GameObject gameobj){
            yield return new WaitForSeconds(time);
            actionManager.FlyUFO(gameobj,ruler,judgement.getRound());
            haveFly.Add(gameobj);
            waitToFly.Remove(gameobj);
        }
        IEnumerator Next(float waitTime){
            yield return new WaitForSeconds(waitTime + 5);
            int curRound = judgement.getRound();
            int curTrial = judgement.getTrial();
            if( curTrial < 10 ){
                judgement.setTrial(curTrial+1);
                LoadResources();
            }
            else
            {
                if(curRound < 3){
                    judgement.setTrial(curTrial + 1);
                    LoadResources();
                }
                if(curRound == 3){
                    judgement.roundOver();
                    endGame();
                    judgement.check();
                }
            }
        }
        public int getRound(){
            return judgement.getRound();
        }
        public int getCurUFONum(){
            return curTrialUFO;
        }
        public void endGame(){
            StopAllCoroutines();
            recycle();
        }
        public void recycle(){
            for(int i = 0;i < waitToFly.Count;i++){
                UFOFactory.getInstance().free(waitToFly[i]);
            }
            for(int i = 0;i < haveFly.Count;i++){
                UFOFactory.getInstance().recycle(haveFly[i]);
            }
            waitToFly.Clear();
            haveFly.Clear();
        }
        public GameStatus getCurStatus(){
            return judgement.getCurStatus();
        }
        public void restart(){
            endGame();
            loadInitSetting();
            recover();
            LoadResources();
        }
        public void back(){
            Director.getInstance().LoadScene(0);
        }
        public void nextTrial(){
            endGame();
            judgement.setTrial(judgement.getTrial()+1);
            recover();
            LoadResources();
        }
        public void nextRound(){
            endGame();
            judgement.setRound(judgement.getRound()+1);
            recover();
            LoadResources();
        }
        public void menu(){
            Time.timeScale = 0;
            judgement.stop();
        }  
        public void recover(){
            Time.timeScale = 1;
            judgement.recover();
        }
    }
}
