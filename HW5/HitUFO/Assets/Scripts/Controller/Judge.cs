using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
{
    public enum GameStatus{ Gaming , Pause , Win , Lose , Over};
    
    public class Judge  {
        private int score;
        private int round;
        private int trial;
        private GameStatus curStauts;
        private static Judge _instance;
        public static Judge getInstance(){
            if(_instance == null ){
                _instance = new Judge();
            }
            return _instance;
        }
        public void roundOver(){
            curStauts = GameStatus.Over;
        }
        public void stop(){
            curStauts = GameStatus.Pause;
        }
        public void recover(){
            check();
        }
        public void check(){
            if(curStauts!=GameStatus.Over){
                curStauts = GameStatus.Gaming;
            }
            else if(score > 30){
                curStauts = GameStatus.Win;
            }
            else{
                curStauts = GameStatus.Lose;
            }
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
        public int getRound(){
            return round;
        }
        public void setRound(int _round){
            if(_round <= 3){
                round = _round;
            }
        }
        public void setTrial(int _trial){
            if(_trial <= 10){
                trial = _trial;
            }else{
                if(round<3){
                    round++;
                    trial = 1;
                }
            }
        }

        public void addScore(int add){
            score += add;
        }
        public void subScore(int sub){
            score -= sub;
        }
    }

}
