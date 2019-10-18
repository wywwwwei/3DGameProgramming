using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
{
    public class ModeChoose{
        protected static ModeChoose modechoose;
        private bool PhysicMode = false;
        public static ModeChoose getInstance(){
            if(modechoose == null ){
                modechoose = new ModeChoose();
            }
            return modechoose;
        }
        public void setPhysic(){
            this.PhysicMode = true;
        }
        public void unsetPhysic(){
            this.PhysicMode = false;
        }
        public bool ifPhysic()
        {
            return PhysicMode;
        }
    }
    public class GameSceneController:MonoBehaviour,ISceneController,IUserAction,ISSActionCallback{
        public int curTrialUFO;
        public baseActionManager actionManager;
        public List<GameObject> waitToFly=new List<GameObject>();
        public Ruler ruler = new Ruler();
        public Judge judgement;
        public bool Countdown = false;
        public int countTime = 3;

        private bool autoNext;

        void Awake() {
            Debug.Log("Awake");
            Director director = Director.getInstance();
            director.setFPS(60);
            director.currentSceneController = this;

            judgement = Judge.getInstance();

            loadInitSetting();
            this.gameObject.AddComponent<UserGUI>();
            if(ModeChoose.getInstance().ifPhysic())
            {
                if(this.gameObject.GetComponent<SSActionManager>()!=null)
                {
                    Destroy(this.gameObject.GetComponent<SSActionManager>());
                }
                if(this.gameObject.GetComponent<PhysicActionManager>()==null)
                {
                    actionManager = this.gameObject.AddComponent<PhysicActionManager>();
                    Debug.Log("new py");
                }
            }
            else
            {
                if(this.gameObject.GetComponent<PhysicActionManager>()!=null)
                {
                    Destroy(this.gameObject.GetComponent<PhysicActionManager>());
                }
                if(this.gameObject.GetComponent<SSActionManager>()==null)
                {
                    actionManager = this.gameObject.AddComponent<SSActionManager>();
                    Debug.Log("new ss");
                }
            }
            actionManager.setCallback(this);
            if(this.gameObject.GetComponent<checkClick>()==null)
            {
                this.gameObject.AddComponent<checkClick>();
            }
            director.currentSceneController.LoadResources();
        }
        public void loadInitSetting(){
            judgement.setRound(1);
            judgement.setTrial(1);
            judgement.init();
            recover();
            curTrialUFO = ruler.getUFOCount(judgement.getRound());
        }
        public void LoadResources(){
            autoNext = true;
            curTrialUFO = ruler.getUFOCount(judgement.getRound());
            getUFO();
        }
        void getUFO(){
            for(int i = 0;i < curTrialUFO;i++){
                waitToFly.Add(UFOFactory.getInstance().getUFO(judgement.getRound()));
            }
            Debug.Log("fly");
            actionManager.FlyUFO(waitToFly,ruler,judgement.getRound());
            waitToFly.Clear();
        }

        public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0,
        string strParam = null){
            if(autoNext){
                judgement.setCountdown();
                StartCoroutine(Next());
            }
        }

        IEnumerator Next(){
            yield return new WaitForSeconds(3);
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
            autoNext = false;
            judgement.shutCountdown();
            StopAllCoroutines();
            actionManager.stop();
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
            endGame();
            UFOFactory.getInstance().clean();
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
