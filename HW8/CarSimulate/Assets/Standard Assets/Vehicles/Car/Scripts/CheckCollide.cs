using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarSimulate{
    public class CheckCollide : MonoBehaviour{
        public float degreeOfDamage{get;set;}

        private void OnCollisionEnter(Collision other) {
            //Get the speed of this car when collision happens
            float speed = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            //The degree of damage is proportional to the speed of the collision
            degreeOfDamage += speed;
            //Debug.Log("degreeOfDamage:"+degreeOfDamage);
        }
    }
}