using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;
    public bool loggedIn = false;

    // Start is called before the first frame update
    void Start()
    {
        CheckSingleton();
        Login();
    }

    private void CheckSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess (LoginResult result)
    {
        Debug.Log("Successful login/account creation!");
        string name = null;
        if (result.InfoResultPayload.PlayerProfile != null)
            name = result.InfoResultPayload.PlayerProfile.DisplayName;

        if (name == null)
            MainMenu.instance.OpenNamePrompt();

        MainMenu.instance.UpdateDisplayName(name);
        loggedIn = true;
    }

    void OnError (PlayFabError error)
    {
        Debug.Log("Error while logging in/creaing account~");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate
                {
                    StatisticName = "HighScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Leaderboard update sent successfully!");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet (GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            if (LeaderboardManager.instance != null)
            {
                LeaderboardManager.instance.AddRow(item.Position, item.DisplayName, item.StatValue);
            }

            Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);
        }
    }

    public void SubmitName(string name)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnDisplayNameError);
    }

    void OnDisplayNameUpdate (UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name!");
        MainMenu.instance.UpdateDisplayName(result.DisplayName);
        MainMenu.instance.CloseNamePrompt();
        MainMenu.instance.CloseUsernameError();
    }

    void OnDisplayNameError(PlayFabError error)
    {
        Debug.Log("Error changing display name");
        Debug.Log(error.GenerateErrorReport());
        MainMenu.instance.ShowUsernameError();
    }
}
