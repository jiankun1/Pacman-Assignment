using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    public AudioSource startSound;

    // Start is called before the first frame update
    void Start()
    {
        //startSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(startSound.isPlaying == false)
        {
            startSound.Play();
        }
    }

    public void LoadLevelOne()
    {
        SceneManager.LoadScene(1);
    }
}
