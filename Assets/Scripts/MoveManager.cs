using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pacman;

    private Tweener tweener;
    public AudioSource pacmanSound;
    public AudioSource startSound;
    public AudioSource backgroundSound;
    public Animator pacAnimationController;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        //play once for intro
        startSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
        //if pacman move to the corner, then turn around the pacman to make sure it moving clockwise
        if(pacman.transform.position.x == -7.5 && pacman.transform.position.y == 9.5)
        {
            //StartCoroutine(MoveLeft());
            if (pacman.transform.rotation.z != 0)
            {
                pacAnimationController.SetTrigger("FlipBack");
            }
            tweener.AddTween(pacman.transform, pacman.transform.position, new Vector3(-12.5f, 9.5f, 0.0f), 3.0f);
            
        }
        else if (pacman.transform.position.x == -12.5 && pacman.transform.position.y == 9.5)
        {
            //StartCoroutine(MoveUp());
            pacAnimationController.SetTrigger("Y0RightTurn");
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
            pacAnimationController.SetTrigger("Y180RightTurn");
            tweener.AddTween(pacman.transform, pacman.transform.position, new Vector3(-7.5f, 9.5f, 0.0f), 3.0f);
        }

        //play the sound
        StartCoroutine(BackgroundMusic());
        StartCoroutine(PlayAudio());

    }

    IEnumerator PlayAudio()
    {
        yield return new WaitUntil(() => startSound.isPlaying == false);
        yield return new WaitUntil(() => pacmanSound.isPlaying == false);
        pacmanSound.Play();

    }

    IEnumerator BackgroundMusic()
    {
        yield return new WaitUntil(() => startSound.isPlaying == false);
        yield return new WaitUntil(() => backgroundSound.isPlaying == false);
        backgroundSound.Play();
    }
    //pacman move left
    /*IEnumerator MoveLeft()
    {
        
        if (pacman.transform.rotation.z != 0)
        {
            pacAnimationController.SetTrigger("Flip");
            yield return new WaitForSeconds(0.2f);
        }
        
        tweener.AddTween(pacman.transform, pacman.transform.position, new Vector3(-12.5f, 9.5f, 0.0f), 3.0f);
        yield return null;
    }

    IEnumerator MoveUp()
    {
        pacAnimationController.SetTrigger("Y0RightTurn");
        yield return new WaitForSeconds(0.2f);
        tweener.AddTween(pacman.transform, pacman.transform.position, new Vector3(-12.5f, 13.5f, 0.0f), 3.0f);
        yield return null;
    }

    IEnumerator MoveRight()
    {
        pacAnimationController.SetTrigger("Flip");
        yield return new WaitForSeconds(0.2f);
        tweener.AddTween(pacman.transform, pacman.transform.position, new Vector3(-7.5f, 13.5f, 0.0f), 3.0f);
        yield return null;
    }

    IEnumerator MoveDown()
    {
        pacAnimationController.SetTrigger("Y180RightTurn");
        yield return new WaitForSeconds(0.2f);
        
        tweener.AddTween(pacman.transform, pacman.transform.position, new Vector3(-7.5f, 9.5f, 0.0f), 3.0f);
        yield return null;
    }*/


}
