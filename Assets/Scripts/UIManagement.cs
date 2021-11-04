using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManagement : MonoBehaviour
{
    public AudioSource startSound;
    public SaveGameManager saver;
    public Text scoreTimeText;

    // Start is called before the first frame update
    void Start()
    {
        //startSound.Play();
        int scoreValue = saver.LoadScore();
        string timeValue = saver.LoadTime();
        scoreTimeText.text = "High score: " + scoreValue.ToString() + "\n" + "Time: " + timeValue;
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
