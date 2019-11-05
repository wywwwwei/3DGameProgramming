using UnityEngine;
using System.Collections;

namespace Halo
{
    public class ParticlePos{
        public float angle{get;set;}
        public float radius{get;set;}

        public ParticlePos(float _radius,float _angle){
            this.angle = _angle;
            this.radius = _radius;
        }
    }

    public class ParticleHalo : MonoBehaviour{

        private ParticleSystem particleSystem;
        private ParticleSystem.Particle[] particleArray;
        private ParticlePos[] particlePos;

        public int particleCount = 1000;
        public float minRadius = 4.0f;
        public float maxRadius = 7.0f;
        public Gradient colorGradient;


        private void Start() {
            particleSystem = GetComponent<ParticleSystem>();

            particleArray = new ParticleSystem.Particle[particleCount];
            particlePos = new ParticlePos[particleCount];
            
            var main = particleSystem.main;
            main.maxParticles = particleCount;

            particleSystem.Emit(particleCount);
            particleSystem.GetParticles(particleArray);

            setParticlePos();
        }

        private void setParticlePos(){

            for(int i = 0;i<particleCount;i++){
                float radius = Random.Range(minRadius,maxRadius);
                float angle = Random.Range(0f,360f);

                particlePos[i] = new ParticlePos(radius,angle);

                particleArray[i].position = new Vector3(radius*Mathf.Cos(angle),0f,radius*Mathf.Sin(angle));
            }

            particleSystem.SetParticles(particleArray,particleArray.Length);
        }

        private void Update() {

            for(int i = 0;i<particleCount;i++){
                particlePos[i].angle = (particlePos[i].angle + Random.Range(0.001f,0.006f) % 360f);
                
                particleArray[i].startColor = colorGradient.Evaluate((particlePos[i].radius - minRadius)/(maxRadius-minRadius));

                particleArray[i].position = new Vector3(particlePos[i].radius*Mathf.Cos(particlePos[i].angle),0f,particlePos[i].radius*Mathf.Sin(particlePos[i].angle));
            }

            particleSystem.SetParticles(particleArray,particleArray.Length);
        }
    }
}