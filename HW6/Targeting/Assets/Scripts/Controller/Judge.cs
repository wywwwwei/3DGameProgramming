using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Targeting
{
    public enum GameStatus{ Gaming , Pause , Win , Lose , Shooting};
    
    public class Judge{
        private int score;
        private int trial;
        private GameStatus curStauts;
        private static Judge _instance;

        public static Judge getInstance(){
            if(_instance == null ){
                _instance = new Judge();
            }
            return _instance;
        }

        public void init(){
            curStauts = GameStatus.Gaming;
            score = 0;
        }
        public void stop(){
            curStauts = GameStatus.Pause;
        }

        public void shoot(){
            curStauts = GameStatus.Shooting;
        }
        public void endShoot(){
            curStauts = GameStatus.Gaming;
        }
        public GameStatus getCurStatus(){
            return curStauts;
        }
        public int getScore(){
            return score;
        }
        public int getTrial(){
            return trial;
        }


        public void setTrial(int _trial){
            if(_trial <= 10){
                trial = _trial;
            }
        }

        public void addScore(int add){
            score += add;
            Debug.Log("score: "+score);
        }
        public void subScore(int sub){
            score -= sub;
        }

    }
}
