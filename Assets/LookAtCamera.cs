using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    public Camera cameraToLookAt;

    void Start()
    {
        cameraToLookAt = Camera.main;
        //transform.Rotate( 180,0,0 );
    }

    void Update()
    {
        Vector3 v = cameraToLookAt.transform.position - transform.position;
        //v.x = v.z = 0.0f;
        //v.x = 0;
        //v.x = 0;
        v.y = 0;
        v.z = 0;
        v.x = 0;
        transform.LookAt(cameraToLookAt.transform.position - v);
        //transform.Rotate(0, 180, 0);
    }
}
