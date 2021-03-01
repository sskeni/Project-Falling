using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public GameObject NamePromptUI;
    public InputField NameInputField;
    public Text DisplayNameText;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void OpenLeaderboard()
    {
        SceneManager.LoadScene("HighScoreLeaderboard");
    }

    public void OpenNamePrompt()
    {
        NamePromptUI.SetActive(true);
    }

    public void CloseNamePrompt()
    {
        NamePromptUI.SetActive(false);
    }

    public void SubmitDisplayName()
    {
        PlayfabManager.instance.SubmitName(NameInputField.text);
        CloseNamePrompt();
    }

    public void UpdateDisplayName(string name)
    {
        DisplayNameText.text = "Logged In As: " + name;
    }
}
