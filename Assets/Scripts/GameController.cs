using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    [Header("UI elements")]
    public GameObject Canvas;
    public GameObject playUI;
    public GameObject startUI;
    public GameObject pauseUI;
    public GameObject scoreText;
    public GameObject pauseScoreText;
    public GameObject highScoreText;
    public GameObject resCountDown;
    public GameObject pauseButton;
    //GameObject player;

    [Header("Resume")]
    public float delay = 3;

    [HideInInspector]
    public int score;
    int highScore;

    void Awake()
    {
        SetupSingelton("GameController", this.gameObject);
        SetupSingelton("Canvas", Canvas);
    }

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        StartMenu();
    }

    void Update()
    {
        //score = player.GetComponent<PlayerController>().GetScore();
        scoreText.GetComponent<Text>().text = score.ToString();

        if (Input.GetKeyUp(KeyCode.Escape) && pauseUI.activeSelf)
            Resume();
        else if (Input.GetKeyUp(KeyCode.Escape) && playUI.activeSelf && pauseButton.activeSelf)
            Pause();
    }

    void SetupSingelton(string tag, GameObject gO)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);

        if (objs.Length > 1)
        {
            Destroy(gO);
        }

        DontDestroyOnLoad(gO);
    }

    void StartMenu()
    {
        Time.timeScale = 0f;
        playUI.SetActive(false);
        startUI.SetActive(true);
        highScoreText.GetComponent<Text>().text = "Highscore: " + highScore.ToString();
    }

    public void ReloadScene()
    {
        highScore = highScore < score ? score : highScore;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Start();
    }

    public void PlayGame()
    {
        playUI.SetActive(true);
        startUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseScoreText.GetComponent<Text>().text = score.ToString();
        pauseUI.SetActive(true);
        playUI.SetActive(false);
    }
    
    public void Resume()
    {
        playUI.SetActive(true);
        pauseUI.SetActive(false);
        StartCoroutine(ResumeCountDown());
    }

    IEnumerator ResumeCountDown()
    {
        pauseButton.SetActive(false);
        resCountDown.SetActive(true);
        resCountDown.GetComponent<Text>().text = "3";
        yield return new WaitForSecondsRealtime(delay / 3);
        resCountDown.GetComponent<Text>().text = "2";
        yield return new WaitForSecondsRealtime(delay / 3);
        resCountDown.GetComponent<Text>().text = "1";
        yield return new WaitForSecondsRealtime(delay / 3);
        resCountDown.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }
}