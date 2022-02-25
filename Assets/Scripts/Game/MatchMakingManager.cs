using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class MatchMakingManager : UWRHelper
{
    [SerializeField] private GameObject m_messageBoxPrefab;
    [SerializeField] private GameObject m_getUserDataPanel;
    [SerializeField] private GameObject m_failedUserDataPanel;
    [SerializeField] private GameObject m_waitingConnectPanel;
    [SerializeField] private GameObject m_waitingSelectTurnPanel;
    [SerializeField] private SelectTurnUI m_selectTurnPanel;
    [SerializeField] private Text m_userNameText;

    private IEnumerator m_coroutine;

    protected void Start()
    {
        StartMatching();
        
    }

    private void StartMatching()
    {
        m_getUserDataPanel.SetActive(true);
        StopMatching();
        m_coroutine = Cort_Init();
        StartCoroutine(Cort_Init());
    }

    private void StopMatching()
    {
        if (m_coroutine != null)
        {
            StopCoroutine(m_coroutine);
            m_coroutine = null;
        }
    }

    private IEnumerator Cort_Init()
    {
        yield return new WaitForSeconds(1f);

        int rndUserID = UnityEngine.Random.Range(10000, 999999);
        string strUserID = rndUserID.ToString();
        GameInfo.MyUserID = strUserID;

        yield return Cort_GetGameKey();

        yield return Cort_GetUserID();

        yield return Cort_CheckRestart();

        yield return Cort_EntryRoom();

        yield return Cort_GotoGameScene();


    }

    /// <summary>
    /// GameKey取得
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cort_GetGameKey()
    {

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (GetClieParameters.m_GameKey != null)
            {
                KeyData.GameKey = GetClieParameters.m_GameKey;
            }
            else
            {

                GameObject msgObj = Instantiate(m_messageBoxPrefab);
                msgObj.GetComponent<MessageBox>().Initialize_Ok("Communication error", $"Failed to get progresses ID\nReturn to the title.", () =>
                {
                    FadeManager.Instance.LoadScene(0, 0.25f);
                    StopMatching();
                });
                while (true) { yield return null; }

            }
        }


        yield break;
    }

    private IEnumerator Cort_GetUserID()
    {


        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (GetClieParameters.m_UserId != null)
            {
                m_getUserDataPanel.SetActive(false);
                m_waitingConnectPanel.SetActive(true);

                GameInfo.MyUserID = GetClieParameters.m_UserId;

            }
            else
            {

                m_getUserDataPanel.SetActive(false);
                m_failedUserDataPanel.SetActive(true);

                GameObject msgObj = Instantiate(m_messageBoxPrefab);
                msgObj.GetComponent<MessageBox>().Initialize_Ok("Communication error", $"Failed to get user ID\nReturn to the title.", () =>
                {
                    FadeManager.Instance.LoadScene(0, 0.25f);
                    StopMatching();
                });
                while (true) { yield return null; }
            }
        }
        else
        {
            m_getUserDataPanel.SetActive(false);
            m_waitingConnectPanel.SetActive(true);
        }

        yield break;
    }

    private IEnumerator Cort_EntryRoom()
    {
        yield return Cort_CheckGame();

        StartCoroutine(Cort_CheckDeleteGame());

        yield return Cort_EntryUser();
        Debug.Log("ユーザ登録完了");
        yield return Cort_Matching();
        Debug.Log("マッチング完了");
        yield return Cort_SetGameInfoGame();
        Debug.Log("ゲームデータ取得完了");
        yield return Cort_SelectTurn();
        Debug.Log("先行/後攻選択完了");
    }

    private IEnumerator Cort_CheckGame()
    {
        var uwr = CreateGetUrl(KeyData.GameKey);
        yield return WaitForRequest(uwr);

        // ゲームデータ作成済みならこの処理を抜ける
        if (CheckKey(uwr)) { yield break; }

        yield return Cort_CreateGameData();
    }

    private IEnumerator Cort_CheckDeleteGame()
    {
        while (true)
        {
            var uwr = CreateGetUrl(KeyData.GameKey);
            yield return WaitForRequest(uwr);

            if (!CheckKey(uwr))
            {
                GameObject msgObj = Instantiate(m_messageBoxPrefab);
                msgObj.GetComponent<MessageBox>().Initialize_Ok("Communication error", $"There is no response from the opponent.\nReturn to the title.", () =>
                {
                    FadeManager.Instance.LoadScene(0, 0.25f);
                    StopMatching();
                });
                while (true) { yield return null; }
            }

            yield return new WaitForSeconds(DefaultSyncSecond);
        }

        
    }

    private IEnumerator Cort_EntryUser()
    {
        UnityWebRequest uwr = CreateGetUrl(KeyData.GameKey);
        yield return WaitForRequest(uwr);
        GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));

        if (string.IsNullOrEmpty(gameData.UserID_01))
        {
            GameInfo.MyTurn = Turn.User01;
            m_userNameText.text = "User1";
            gameData.UserID_01 = GameInfo.MyUserID;

            gameData.timeLimit = DateTime.Now.AddHours(24).ToString();
            yield return Cort_UpdateGameData(gameData);
        }
        else if(string.IsNullOrEmpty(gameData.UserID_02) && GameInfo.MyUserID != gameData.UserID_01)
        {
            GameInfo.MyTurn = Turn.User02;
            m_userNameText.text = "User2";
            gameData.UserID_02 = GameInfo.MyUserID;
            yield return Cort_UpdateGameData(gameData);
        }
        else
        {
            GameObject msgObj = Instantiate(m_messageBoxPrefab);
            msgObj.GetComponent<MessageBox>().Initialize_Ok("Communication error", $"I'm trying to register the same user ID. \nBack to the title.", () =>
            {
                FadeManager.Instance.LoadScene(0, 0.25f);
                
            });
            while (true) { yield return null; }
        }
    }

    public IEnumerator Cort_Matching()
    {
        while(true)
        {
            var uwr = CreateGetUrl(KeyData.GameKey);
            yield return WaitForRequest(uwr);

            if (CheckKey(uwr))
            {
                GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));

                if (GameInfo.MyTurn == Turn.User01 && gameData.UserID_01 != GameInfo.MyUserID)
                {
                    if(string.IsNullOrEmpty(gameData.UserID_02) && GameInfo.MyUserID != gameData.UserID_01)
                    {
                        GameInfo.MyTurn = Turn.User02;
                        m_userNameText.text = "User2";
                        gameData.UserID_02 = GameInfo.MyUserID;
                        yield return Cort_UpdateGameData(gameData);
                    }
                    else
                    {
                        GameObject msgObj = Instantiate(m_messageBoxPrefab);
                        msgObj.GetComponent<MessageBox>().Initialize_Ok("Communication error", $"I'm trying to register the same user ID. \nBack to the title.", () =>
                        {
                            FadeManager.Instance.LoadScene(0, 0.25f);
                            
                        });
                        while (true) { yield return null; }
                    }
                }

                if(!string.IsNullOrEmpty(gameData.UserID_01) && !string.IsNullOrEmpty(gameData.UserID_02))
                {
                    yield break;
                }
            }

            yield return new WaitForSeconds(DefaultSyncSecond);
        }
    }

    private IEnumerator Cort_SelectTurn()
    {
        m_waitingConnectPanel.SetActive(false);

        switch (GameInfo.MyTurn)
        {
            case Turn.User01:
                StartCheckTimeLimit(DefaultTimeLimitSecond);
                yield return m_selectTurnPanel.Cort_TurnSelect();

                EndCheckTimeLimit();
                yield return Cort_UpdateGameData(GameInfo.Game);
                break;
            case Turn.User02:
                m_waitingSelectTurnPanel.SetActive(true);
                yield return Cort_SettingTurnSync();
                break;
        }
    }

    public IEnumerator Cort_SetGameInfoGame()
    {
        while(true)
        {
            var uwr = CreateGetUrl(KeyData.GameKey);
            yield return WaitForRequest(uwr);

            if(CheckKey(uwr))
            {
                GameInfo.Game = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));
                break;
            }
        }
    }

    public IEnumerator Cort_SettingTurnSync()
    {
        StartCheckTimeLimit(DefaultTimeLimitSecond);

        while(true)
        {
            var uwr = CreateGetUrl(KeyData.GameKey);
            yield return WaitForRequest(uwr);

            if(CheckKey(uwr))
            {
                GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));

                if(gameData.Turn != Turn.None)
                {
                    GameInfo.Game = gameData;
                    break;
                }
            }

            yield return new WaitForSeconds(DefaultSyncSecond);
        }

        EndCheckTimeLimit();
    }

    private IEnumerator Cort_GotoGameScene()
    {
        if(GameInfo.MyUserID != string.Empty)
        {
            FadeManager.Instance.LoadScene(2, 0.25f);
        }
        yield break;
    }

    private IEnumerator Cort_CreateGameData()
    {
        GameData data = new GameData();
        data.RoomID = KeyData.GameKey;
        yield return Cort_CreateGameData(data);
    }

    private IEnumerator Cort_CreateGameData(GameData gameData)
    {
        var uwr = CreateSetUrl(KeyData.GameKey, GameData.ToJsonConvert(gameData));
        yield return WaitForRequest(uwr);
    }

    private IEnumerator Cort_UpdateGameData(GameData gameData)
    {
        var uwr = CreateSetUrl(KeyData.GameKey, GameData.ToJsonConvert(gameData));
        yield return WaitForRequest(uwr);

        if (!CheckKey(uwr))
        {
            GameObject msgObj = Instantiate(m_messageBoxPrefab);
            msgObj.GetComponent<MessageBox>().Initialize_Ok("Communication error", $"No game information found. \nBack to the title.", () =>
            {
                FadeManager.Instance.LoadScene(0, 0.25f);
                
            });
            while (true) { yield return null; }
        }
    }

    private IEnumerator Cort_CheckRestart()
    {
        var uwr = CreateGetUrl(KeyData.GameKey);
        yield return WaitForRequest(uwr);

        if(CheckKey(uwr))
        {
            if(CheckDifferenceGameData(uwr))
            {
                Debug.Log("神経衰弱以外のゲームデータがサーバー上にあったので削除。");
                yield return Cort_DeleteGameData();
            }
            else
            {
                GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));

                if(!string.IsNullOrEmpty(gameData.timeLimit))
                {
                    DateTime timeLimit = DateTime.Parse(gameData.timeLimit);
                    if(DateTime.Now > timeLimit)
                    {
                        yield return Cort_DeleteGameData();
                        Debug.Log("24時間経ってたのでゲームデータ削除");
                        yield break;
                    }
                }


                if(!string.IsNullOrEmpty(gameData.UserID_01) && !string.IsNullOrEmpty(gameData.UserID_02))
                {
                    if(KeyData.GameKey == gameData.RoomID && gameData.Turn != Turn.None)
                    {
                        if(GameInfo.MyUserID == gameData.UserID_01)
                        {
                            GameInfo.MyTurn = Turn.User01;
                            m_userNameText.text = "User1";
                            GameInfo.IsRestart = true;
                            yield return Cort_SetGameInfoGame();
                            yield return Cort_GotoGameScene();
                            while(true) { yield return null; }
                        }
                        else if (GameInfo.MyUserID == gameData.UserID_02)
                        {
                            GameInfo.MyTurn = Turn.User02;
                            m_userNameText.text = "User2";
                            GameInfo.IsRestart = true;
                            yield return Cort_SetGameInfoGame();
                            yield return Cort_GotoGameScene();
                            while (true) { yield return null; }
                        }
                    }
                    yield return Cort_DeleteGameData();
                    yield return new WaitForSeconds(DefaultSyncSecond);
                }

                
            }
        }
    }

    private IEnumerator Cort_DeleteGameData()
    {
        var uwr = CreateDeleteUrl(KeyData.GameKey);
        yield return WaitForRequest(uwr);
        Debug.Log("ゲームデータ削除");
    }
}
