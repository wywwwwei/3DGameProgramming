using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Targeting
{
    public class GameSceneController:MonoBehaviour,ISceneController,IUserAction,ISSActionCallback{
        public SSActionManager actionManager;
        public Ruler ruler;
        public Judge judgement;
        public Arrow arrow;
        public Bow bow;
        public Target target;

        void Awake() {
            Director director = Director.getInstance();
            director.setFPS(60);
            director.currentSceneController = this;

            judgement = Judge.getInstance();
            ruler = Ruler.getInstance();

            loadInitSetting();
            actionManager = this.gameObject.AddComponent<SSActionManager>();
            actionManager.setCallback(this);

            this.gameObject.AddComponent<UserGUI>();
            director.currentSceneController.LoadResources();
        }

        public void loadInitSetting(){
            judgement.setTrial(1);
            judgement.init();
        }
        public void LoadResources(){
            arrow = new Arrow();
            bow = new Bow();
            target = new Target();
            ruler.nextWind();
        }

        public void shoot()
        {
            judgement.shoot();
            actionManager.shoot(arrow.arrow,ruler);
        }
        public void reloadArrow()
        {
            arrow.reload(bow.bow.transform.position);
            ruler.nextWind();
            judgement.endShoot();
        }
        public void endgame()
        {
            Destroy(arrow.arrow);
            Destroy(bow.bow);
            Destroy(target.target);
        }
        public void restart()
        {
            endgame();
            LoadResources();
        }
        public void back()
        {
            endgame();
            Director.getInstance().LoadScene(0);
        }

        public void moveBow(float translationX,float translationY,float translationZ){
            actionManager.moveBow(bow.bow,arrow.arrow,translationX);
        }

        public GameStatus getCurStatus()
        {
            return judgement.getCurStatus();
        }
        public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0,
        string strParam = null)
        {
            reloadArrow();
        }
    }
}
