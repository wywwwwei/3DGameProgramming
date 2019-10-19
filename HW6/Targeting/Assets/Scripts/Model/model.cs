using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Targeting
{
    public class Arrow 
    {
        public GameObject arrow;
        public Vector3 startPos = new Vector3(0f,0.5f,-7.5f);
        public Vector3 startRot = new Vector3(-90f,0f,0f);
        public Arrow()
        {
            arrow = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Arrow"), startPos,Quaternion.Euler(startRot))as GameObject;
            arrow.AddComponent<checkCollision>();
            init();
        }
        public void init()
        {
            Rigidbody rigidbody = this.arrow.AddComponent<Rigidbody>();  
            rigidbody.useGravity = false;
            rigidbody.isKinematic = false;
            arrow.tag="arrow";
        }
        public void reload(Vector3 bowPos)
        {
            Vector3 newVal = bowPos;
            newVal.y += 0.5f;
            this.arrow.transform.position = newVal;
            this.arrow.GetComponent<Rigidbody>().isKinematic = false;
            arrow.tag="arrow";
        }
    }

    public class Bow
    {
        public GameObject bow;
        public Vector3 startPos = new Vector3(0f,0f,-7.5f);
        public Vector3 startRot = new Vector3(-90f,0f,0f);
        public Bow()
        {
            bow = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Bow"), startPos,Quaternion.Euler(startRot))as GameObject;
        }

    }

    public class Target
    {
        public GameObject target;
        Vector3 startPos = new Vector3(0f,0f,0f);
        public Target()
        {
            target = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Target"), startPos,Quaternion.identity)as GameObject;
        }
    }

    public class Ruler
    {
        private static Ruler _instance;

        private System.Random random = new System.Random();
        private float windDirectionX;
        private float windDirectionY;

        public static Ruler getInstance(){
            if(_instance == null ){
                _instance = new Ruler();
            }
            return _instance;
        }
        Ruler()
        {
            nextWind();
        }
        public void nextWind()
        {
            this.windDirectionX = ((this.random.Next() & 2) - 1) * (this.random.Next(30) + 1) * 0.1f;
            this.windDirectionY = ((this.random.Next() & 2) - 1) * (this.random.Next(30) + 1) * 0.1f;
        }
        public Vector3 getWind()
        {
            return new Vector3(this.windDirectionX,this.windDirectionY,0);
        }

        public string display()
        {
            string Horizontal = "", Vertical = "", level = "";
            if (this.windDirectionX > 0)
            {
                Horizontal = "west";
            }
            else if (this.windDirectionX <= 0)
            {
                Horizontal = "east";
            }
            if (this.windDirectionY > 0)
            {
                Vertical = "sounth";
            }
            else if (this.windDirectionY <= 0)
            {
                Vertical = "north";
            }
            if ((Math.Abs(this.windDirectionX) + Math.Abs(this.windDirectionY)) / 2 <= 1)
            {
                level = "Level: 1";
            }
            else if ((Math.Abs(this.windDirectionX) + Math.Abs(this.windDirectionY)) / 2  <= 2)
            {
                level = "Level: 2";
            }
            else if ((Math.Abs(this.windDirectionX) + Math.Abs(this.windDirectionY)) / 2 <= 3)
            {
                level = "Level: 3";
            }
            return ("Wind direction:" + Vertical + Horizontal+ "  " + level);
        }
    }
}
