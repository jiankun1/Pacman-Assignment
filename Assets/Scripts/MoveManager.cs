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
