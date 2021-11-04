using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        RaycastHit2D hit2d = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), 0.5f);
        if(hit2d)
        {
            Debug.Log("2d cast: " + hit2d.collider.name);
            //Debug.Log("distance: " + hit2d.distance);
            //Debug.Log("point: " + hit2d.point);
            //Debug.Log("transform: " + hit2d.transform);
        }

        
        
    }

    
}
