using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundStartController : MonoBehaviour
{
    //Round start
    private int countdownTime = 3;
    public Text countdownText;
    public GameObject gameControlObject;
    public GameObject ghostController;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCountDown());
    }

    public void FinishGame()
    {
        StartCoroutine(GameFinish());
    }

    //set the countdown text for beginning of game
    IEnumerator StartCountDown()
    {
        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString();
            yield return new WaitForSeconds(1.0f);
            countdownTime -= 1;
        }
        countdownText.text = "GO!";

        yield return new WaitForSeconds(1.0f);

        countdownText.gameObject.SetActive(false);
        gameControlObject.SetActive(true);
        ghostController.SetActive(true);
    }

    //when the game finish
    IEnumerator GameFinish()
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = "Game Over";
        gameControlObject.SetActive(false);
        ghostController.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(0);
    }
}
