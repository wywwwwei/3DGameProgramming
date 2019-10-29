using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol{
    public enum GameStatus{ Gaming , Pause , Win , Lose , Over};

    public class Judge{
        private int score = 0;
        private GameStatus curStauts = GameStatus.Gaming;

        private static Judge _instance;
        public static Judge getInstance(){
            if(_instance == null ){
                _instance = new Judge();
            }
            return _instance;
        }

        public GameStatus getCurStatus(){
            return this.curStauts;
        }
        public void setCurStatus(){
            this.curStauts = GameStatus.Lose;
        }

        public void addScore(){
            this.score += 1;
        } 

        public int getScore(){
            return this.score;
        }
    }
}