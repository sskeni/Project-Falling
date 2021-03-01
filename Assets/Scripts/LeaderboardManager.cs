using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;
    public GameObject RowPrefab;
    public Transform RowsParent;

    private void Start()
    {
        CheckSingleton();
        ReloadLeaderboard();
    }

    private void CheckSingleton()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    private void ReloadLeaderboard()
    {
        foreach (Transform item in RowsParent)
        {
            Destroy(item.gameObject);
        }
        PlayfabManager.instance.GetLeaderboard();
    }

    public void AddRow(int position, string name, int score)
    {
        GameObject newGo = Instantiate(RowPrefab, RowsParent);
        Text[] texts = newGo.GetComponentsInChildren<Text>();
        texts[0].text = (position + 1).ToString();
        texts[1].text = name;
        texts[2].text = score.ToString();
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
