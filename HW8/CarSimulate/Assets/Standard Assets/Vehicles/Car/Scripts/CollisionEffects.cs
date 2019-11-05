using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

namespace CarSimulate
{
    public class CollisionEffects : MonoBehaviour {

        public float engineRevs;
        public float degreeOfDamage;
        public float exhaustRate = 10f;

        ParticleSystem exhaust;

        CarController carController;
        CheckCollide checkCollide;
        


        void Start () {
            exhaust = GetComponent<ParticleSystem>();
            carController = this.transform.parent.parent.gameObject.GetComponent<CarController>();
            checkCollide = this.transform.parent.parent.gameObject.GetComponent<CheckCollide>();
        }


        void Update () {
            //The faster the speed, the more particles are emitted
            engineRevs = carController.Revs;
            //Debug.Log("Revs:"+engineRevs);
            exhaust.emissionRate = engineRevs * 100f + exhaustRate;
            Debug.Log("Rate:"+exhaust.emissionRate);

            degreeOfDamage = checkCollide.degreeOfDamage;
            float colorRatio = (degreeOfDamage % 400.0f) / 400.0f % 0.8f + 0.2f; //Avoid colorRatio greater than one
            var col = exhaust.colorOverLifetime;
            col.enabled = true;

            //The more collisions, the longer the exhaust disappears
            Gradient grad = new Gradient();
            grad.SetKeys( new GradientColorKey[] {new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, colorRatio), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] {new GradientAlphaKey(1f, 0.0f),new GradientAlphaKey(0.5f, colorRatio), new GradientAlphaKey(0f, 1.0f) } );

            col.color = grad;
        }

    }
}