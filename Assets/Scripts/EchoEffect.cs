using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    //public float timeBtwSpawns;
    //public float startBtwSpawns;
    public GameObject fadingObject;

    private float timePeriod = 0.1f;
    private float accumulatedTime = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(accumulatedTime >= timePeriod)
        {
            GameObject theObject = Instantiate(fadingObject, transform.position, Quaternion.identity);
            Destroy(theObject, 1.0f);
            accumulatedTime = 0.0f;
        }
        else
        {
            accumulatedTime += Time.deltaTime;
        }

        
    }
}
