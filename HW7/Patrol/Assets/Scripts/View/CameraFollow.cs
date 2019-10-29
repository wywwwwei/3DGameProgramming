using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float distanceAway=5.0f;			
	public float distanceUp=5.0f;
    public Transform follow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(follow!=null){
            transform.position = Vector3.Lerp(transform.position, follow.position + Vector3.up * distanceUp - follow.forward * distanceAway, 3 * Time.deltaTime);
            transform.LookAt(follow);
        }
    }
    public void setFollow(Transform _follow){
        follow = _follow;
    }
}
