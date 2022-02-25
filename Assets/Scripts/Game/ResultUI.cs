using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private GameDataSyncer m_gameDataSyncer;

    [SerializeField] private GameObject m_winUI;

    [SerializeField] private GameObject m_loseUI;

    [SerializeField] private Button m_retryButton;

    private IEnumerator Start()
    {
        m_retryButton.gameObject.SetActive(false);

        m_gameDataSyncer.StopGameSync();

        m_gameDataSyncer.StartcCheckResult();

        yield return Cort_WaitResult();

        m_retryButton.gameObject.SetActive(true);
        m_retryButton.onClick.AddListener(() => StartCoroutine(Cort_OnClick_RetryButton()));
    }

    private IEnumerator Cort_OnClick_RetryButton()
    {
        SetIntaractableButton(false);

        yield return m_gameDataSyncer.Cort_DeleteGameData();
        FadeManager.Instance.LoadScene(1, 0.25f);
    }

    private void SetIntaractableButton(bool enabled)
    {
        m_retryButton.interactable = enabled;
    }

    public IEnumerator Cort_Win()
    {
        GameInfo.WinCount++;
        gameObject.SetActive(true);
        m_winUI.SetActive(true);
        m_loseUI.SetActive(false);

        if (GameInfo.URLType != URLType.Quadra)
        {
            yield return m_gameDataSyncer.Cort_UpdateResult(
                userID: GameInfo.MyUserID,
                roomID: KeyData.GameKey,
                winCount: GameInfo.WinCount,
                loseCount: GameInfo.LoseCount,
                applicationName: GameInfo.ApplicationName);
        }
        yield break;
    }

    public IEnumerator Cort_Lose()
    {
        GameInfo.LoseCount++;
        gameObject.SetActive(true);
        m_winUI.SetActive(false);
        m_loseUI.SetActive(true);

        if (GameInfo.URLType != URLType.Quadra)
        {
            yield return m_gameDataSyncer.Cort_UpdateResult(
                userID: GameInfo.MyUserID,
                roomID: KeyData.GameKey,
                winCount: GameInfo.WinCount,
                loseCount: GameInfo.LoseCount,
                applicationName: GameInfo.ApplicationName);
        }
        yield break;
    }

    private IEnumerator Cort_WaitResult()
    {
        m_retryButton.gameObject.SetActive(false);
        yield return m_gameDataSyncer.Cort_WaitResult();
    }
}
