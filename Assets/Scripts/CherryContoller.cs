using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryContoller : MonoBehaviour
{
    private float initialTime;
    //private float duration = 0.0f;

    public GameObject cherry;

    private Tween activeTween;

    private GameObject cherryObject = null;

    // Start is called before the first frame update
    void Start()
    {
        initialTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if((Time.time - initialTime) >= 10.0f)
        {
            float ranNum = Random.Range(-26.5f, 26.5f);
            cherryObject = Instantiate(cherry, new Vector3(ranNum, 16.5f, 0.0f),Quaternion.identity);

            AddTween(cherryObject.transform, cherryObject.transform.position, new Vector3(-ranNum, -16.5f, 0.0f), 8.0f);
            //Reset the initial time
            //duration = initialTime;
            //Debug.Log("duration: " + (Time.time - initialTime));
            initialTime = Time.time;

        }
        /*else
        {
            duration += Time.deltaTime;
        }*/

        if (activeTween != null)
        {
            //distance to end position
            float dist = Vector3.Distance(activeTween.Target.position, activeTween.EndPos);
            // the fraction t, using the duration divided by elapsed time
            float t = (Time.time - activeTween.StartTime) / activeTween.Duration;
            //float cubicT = t * t * t;
            if (dist > 0.1f)
            {

                activeTween.Target.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, t);

            }
            else if (dist <= 0.1f)
            {

                activeTween.Target.position = activeTween.EndPos;
                activeTween = null;
                if (cherryObject != null)
                {
                    Destroy(cherryObject);
                    cherryObject = null;
                }
                initialTime = Time.time;
            }
        }

    }

    public void AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if (activeTween == null)
        {
            activeTween = new Tween(targetObject, startPos, endPos, Time.time, duration);
        }
    }


}
