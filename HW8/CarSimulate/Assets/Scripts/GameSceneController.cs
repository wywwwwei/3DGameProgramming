using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarSimulate
{
    public class GameSceneController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GameObject.Find("Camera").GetComponent<CameraFollow>().setFollow(GameObject.Find("Car").transform);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
