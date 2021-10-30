using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject movingObject;

    private Tweener tweener;
    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingObject.transform.position.x == -10.5f && movingObject.transform.position.y == 4.7f)
        {
            tweener.AddTween(movingObject.transform, movingObject.transform.position, new Vector3(10.5f, 4.7f, 0.0f), 3.0f);
        }
        else if(movingObject.transform.position.x == 10.5f && movingObject.transform.position.y == 4.7f)
        {
            tweener.AddTween(movingObject.transform, movingObject.transform.position, new Vector3(10.5f, -4.7f, 0.0f), 3.0f);
        }
        else if (movingObject.transform.position.x == 10.5f && movingObject.transform.position.y == -4.7f)
        {
            tweener.AddTween(movingObject.transform, movingObject.transform.position, new Vector3(-10.5f, -4.7f, 0.0f), 3.0f);
        }
        else if (movingObject.transform.position.x == -10.5f && movingObject.transform.position.y == -4.7f)
        {
            tweener.AddTween(movingObject.transform, movingObject.transform.position, new Vector3(-10.5f, 4.7f, 0.0f), 3.0f);
        }
    }
}
