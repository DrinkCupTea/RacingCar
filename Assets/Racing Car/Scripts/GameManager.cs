using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private float time;
    private int score;
    private bool isGameOver;
    // Start is called before the first frame update
    void Start()
    {
        UpdateScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        time += Time.deltaTime;
        int timer = (int)time;
        int miliseconds = (int)((time - timer) * 100);
        int minutes = (int)(timer / 60);
        int seconds = timer % 60;

        string milisecondsText;
        if (miliseconds < 10)
        {
            milisecondsText = "00" + miliseconds;
        }
        else if (miliseconds < 100)
        {

            milisecondsText = "0" + miliseconds;
        }
        else
        {
            milisecondsText = miliseconds.ToString();
        }
        timeText.text = "Time: " + (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds + ":" + milisecondsText;
        // timeText.SetText("Time: " + (minutes < 10 ? "0" : "") + minutes + ":" + seconds);
    }

    public void UpdateScore(int point)
    {
        score += point;
        scoreText.text = "Score: " + score;
        // scoreText.SetText("Score: " + score);
    }

    public void CheckGameOver()
    {
        if (isGameOver)
        {
            return;
        }
        isGameOver = true;

        foreach (BasicMovement basicMovement in FindObjectsOfType<BasicMovement>())
        {
            basicMovement.SetMoveSpeed(0);
            basicMovement.SetRotateSpeed(0);
        }
    }
}
