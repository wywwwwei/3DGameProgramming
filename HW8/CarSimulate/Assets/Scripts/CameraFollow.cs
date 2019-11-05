using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarSimulate{
    public class CameraFollow : MonoBehaviour
    {
        public float distanceAway=8.0f;			
        public float distanceUp=8.0f;
        public Transform follow;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if(follow!=null){
                Vector3 desPos = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;
                if(Mathf.Approximately(transform.position.x,desPos.x)&&Mathf.Approximately(transform.position.y,desPos.y)&&Mathf.Approximately(transform.position.z,desPos.z))
                {
                    return;
                }
                transform.position = Vector3.Lerp(transform.position, follow.position + Vector3.up * distanceUp - follow.forward * distanceAway, 3 * Time.deltaTime);
                transform.LookAt(follow);
            }
        }
        public void setFollow(Transform _follow){
            follow = _follow;
        }
    }
}
