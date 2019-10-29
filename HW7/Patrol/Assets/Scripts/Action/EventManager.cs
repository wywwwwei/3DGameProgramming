using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    public class EventManager:MonoBehaviour{
        public delegate void AddScore();
        public static event AddScore addScore;

        
    }
}