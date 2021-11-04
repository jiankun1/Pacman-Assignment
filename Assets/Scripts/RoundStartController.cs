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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCountDown());
    }

    public void FinishGame()
    {
        StartCoroutine(GameFinish());
    }

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
    }

    IEnumerator GameFinish()
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = "Game Over";
        gameControlObject.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(0);
    }
}
