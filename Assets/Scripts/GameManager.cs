using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool gameOver;

    private void Awake()
    {
        CheckSingleton();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void CheckSingleton()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnGameOver()
    {
        gameOver = true;
        UIHandler.instance.UpdateHighScore();
        UIHandler.instance.GameOverUI();
        PlayfabManager.instance.SendLeaderboard((int)PlayerPrefs.GetFloat("HighScore"));
    }

    public void ResetGameOver()
    {
        gameOver = false;
    }

    public bool GameOver()
    {
        return gameOver;
    }
}
