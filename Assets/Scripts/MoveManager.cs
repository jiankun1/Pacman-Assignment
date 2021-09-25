using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pacman;

    private Tweener tweener;
    public AudioSource theSound;
    public Animator pacAnimationController;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pacman.transform.position.x == -7.5 && pacman.transform.position.y == 9.5)
        {
            //StartCoroutine(MoveLeft());
            if (pacman.transform.rotation.z != 0)
            {
                pacAnimationController.SetTrigger("Flip");
            }
            tweener.AddTween(pacman.transform, pacman.transform.position, new Vector3(-12.5f, 9.5f, 0.0f), 3.0f);
            
        }
        else if (pacman.transform.position.x == -12.5 && pacman.transform.position.y == 9.5)
        {
            //StartCoroutine(MoveUp());
            pacAnimationController.SetTrigger("RightTurn");
            tweener.AddTween(pacman.transform, pacman.transform.position, new Vector3(-12.5f, 13.5f, 0.0f), 3.0f);
        }
        else if (pacman.transform.position.x == -12.5 && pacman.transform.position.y == 13.5)
        {
            //StartCoroutine(MoveRight());
            pacAnimationController.SetTrigger("Flip");
            tweener.AddTween(pacman.transform, pacman.transform.position, new Vector3(-7.5f, 13.5f, 0.0f), 3.0f);
        }
        else if (pacman.transform.position.x == -7.5 && pacman.transform.position.y == 13.5)
        {
            //StartCoroutine(MoveDown());
            pacAnimationController.SetTrigger("RightTurn");
            tweener.AddTween(pacman.transform, pacman.transform.position, new Vector3(-7.5f, 9.5f, 0.0f), 3.0f);
        }


        StartCoroutine(PlayAudio());

    }

    //pacman move left
    IEnumerator MoveLeft()
    {
        /*if (pacman.transform.rotation.z != 90)
        {
            pacAnimationController.SetTrigger("DowntoLeft");
        }*/

        
        tweener.AddTween(pacman.transform, pacman.transform.position, new Vector3(-12.5f, 9.5f, 0.0f), 3.0f);
        yield return null;
    }

    IEnumerator MoveUp()
    {
        pacAnimationController.SetTrigger("LefttoUp");
        tweener.AddTween(pacman.transform, pacman.transform.position, new Vector3(-12.5f, 13.5f, 0.0f), 3.0f);
        yield return null;
    }

    IEnumerator MoveRight()
    {
        pacAnimationController.SetTrigger("UptoRight");
        tweener.AddTween(pacman.transform, pacman.transform.position, new Vector3(-7.5f, 13.5f, 0.0f), 3.0f);
        yield return null;
    }

    IEnumerator MoveDown()
    {
        pacAnimationController.SetTrigger("RighttoDown");
        yield return new WaitForSeconds(1.0f);
        tweener.AddTween(pacman.transform, pacman.transform.position, new Vector3(-7.5f, 9.5f, 0.0f), 3.0f);
        yield return null;
    }
    
    IEnumerator PlayAudio()
    {
        yield return new WaitUntil(() => theSound.isPlaying == false);
        theSound.Play();
        
    }
}
