using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameDataSyncer : UWRHelper
{

    private IEnumerator m_coroutine;

    private void Start()
    {
        StartCheckConnecting();
    }

    private IEnumerator Cort_CheckConnecting()
    {
        while(true)
        {
            UnityWebRequest uwr = CreateGetUrl(KeyData.GameKey);
            yield return WaitForRequest(uwr);

            if(!CheckKey(uwr))
            {
                GameObject msgObj = Instantiate((GameObject)Resources.Load("MessageBox"));
                msgObj.GetComponent<MessageBox>().Initialize_Ok("Communication error", $"Failed to get progresses ID\nReturn to the title.", () =>
                {
                    FadeManager.Instance.LoadScene(1, 1f);
                    
                });
                while (true) { yield return null; }
            }
            yield return new WaitForSeconds(DefaultSyncSecond);
        }
    }

    private void StartCheckConnecting()
    {
        EndCoroutine();
        m_coroutine = Cort_CheckConnecting();
        StartCoroutine(m_coroutine);
    }

    public void StartGameSync()
    {
        EndCoroutine();
        m_coroutine = Cort_GameDataSync();
        StartCoroutine(m_coroutine);
    }

    public void StartcCheckResult()
    {
        EndCoroutine();
        m_coroutine = Cort_CheckResult();
        StartCoroutine(m_coroutine);
    }

    public void StopGameSync()
    {
        EndCoroutine();
    }

    public void EndCoroutine()
    {
        if(m_coroutine != null)
        {
            StopCoroutine(m_coroutine);
            m_coroutine = null;
        }
    }

    private IEnumerator Cort_GameDataSync()
    {
        while(true)
        {
            UnityWebRequest uwr = CreateGetUrl(KeyData.GameKey);
            yield return WaitForRequest(uwr);

            if(CheckKey(uwr))
            {
                GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));

                // yield return GameManager.Instance.Cort_ScreenSync(gameData);
            }
            else
            {
                GameObject msgObj = Instantiate((GameObject)Resources.Load("MessageBox"));
                msgObj.GetComponent<MessageBox>().Initialize_Ok("Communication error", $"Failed to get progresses ID\nReturn to the title.", () =>
                {
                    FadeManager.Instance.LoadScene(1, 1f);

                });
                while (true) { yield return null; }
            }
            yield return new WaitForSeconds(DefaultSyncSecond);
        }
        
    }

    private IEnumerator Cort_CheckResult()
    {
        while(true)
        {
            UnityWebRequest uwr = CreateGetUrl(KeyData.GameKey);
            yield return WaitForRequest(uwr);

            if(!CheckKey(uwr))
            {
                FadeManager.Instance.LoadScene(1, 1f);
                yield break;
            }

            yield return new WaitForSeconds(DefaultSyncSecond);
        }
    }

    public IEnumerator Cort_UpdateGameData(GameData gameData)
    {
        UnityWebRequest uwr = CreateSetUrl(KeyData.GameKey, GameData.ToJsonConvert(gameData));
        yield return WaitForRequest(uwr);
    }

    public IEnumerator Cort_DeleteGameData()
    {
        UnityWebRequest uwr = CreateDeleteUrl(KeyData.GameKey);
        yield return WaitForRequest(uwr);
    }

    public IEnumerator Cort_SetUpDataSync_User2()
    {
        while(true)
        {
            UnityWebRequest uwr = CreateGetUrl(KeyData.GameKey);
            yield return WaitForRequest(uwr);

            if(CheckKey(uwr))
            {
                GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));

                int counter = 0;

                for(int i = 0; i < GameData.Height; i++)
                {
                    for(int j = 0; j < GameData.Width; j++)
                    {
                        if(gameData.CellArray[i, j] != (int)TrumpType.None)
                        {
                            counter++;
                        }
                    }
                }

                if(counter == GameData.Height * GameData.Width)
                {
                    GameInfo.Game = gameData;
                    break;
                }
            }

            yield return new WaitForSeconds(DefaultSyncSecond);
        }
    }

}
