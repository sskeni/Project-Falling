using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    public Text ScoreText;
    public Text HighScoreText;
    public Text JumpMultiplierText;
    public GameObject DeathUI;

    private float previousPlayerY;
    private float score;
    private float highScore;

    void Awake()
    {
        CheckSingleton();
        highScore = PlayerPrefs.GetFloat("HighScore");
    }

    void Start()
    {
        GameManager.instance.ResetGameOver();
        previousPlayerY = PlayerController.instance.transform.position.y;
    }

    void Update()
    {
        CalculateScore();
        UpdateUI();
    }

    public void AddScore(float value)
    {
        score += value;
    }

    private void CalculateScore()
    {
        if (!GameManager.instance.GameOver())
        {
            score += Mathf.Abs(PlayerController.instance.transform.position.y - previousPlayerY);
            previousPlayerY = PlayerController.instance.transform.position.y;

            if (score > highScore)
            {
                highScore = score;
            }
        }
    }

    private void UpdateUI()
    {
        ScoreText.text = "Score: " + (int)score;
        HighScoreText.text = "HighScore: " + (int)highScore;
        JumpMultiplierText.text = "Jump Multiplier: " + System.Math.Round(PlayerController.instance.getJumpMultiplier(), 2);
    }

    public void UpdateHighScore()
    {
        PlayerPrefs.SetFloat("HighScore", highScore);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOverUI()
    {
        DeathUI.SetActive(true);
    }

    private void CheckSingleton()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }
}
