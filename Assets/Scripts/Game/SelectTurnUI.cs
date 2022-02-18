using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTurnUI : MonoBehaviour
{
    [Header("First Player")]
    [SerializeField] private Button m_firstPlayerButtun;

    [Header("Second Player")]
    [SerializeField] private Button m_secondPlayerButton;

    private Turn m_selectTurn = Turn.None;

    /// <summary>
    /// 先攻ボタン選択時の処理
    /// </summary>
    private void FirstPlayerButton()
    {
        m_firstPlayerButtun.interactable = false;
        m_secondPlayerButton.interactable = false;
        m_selectTurn = GameInfo.MyTurn;
    }

    /// <summary>
    /// 後攻ボタン選択時の処理
    /// </summary>
    private void SecondPlayerButton()
    {
        switch (GameInfo.MyTurn)
        {
            case Turn.User01:
                m_selectTurn = Turn.User02;
                break;
            case Turn.User02:
                m_selectTurn = Turn.User01;
                break;
        }
    }

    /// <summary>
    /// 先攻か後攻を選択する、ボタン押すまでループ
    /// </summary>
    /// <returns></returns>
    public IEnumerator Cort_TurnSelect()
    {
        gameObject.SetActive(true);

        m_firstPlayerButtun.interactable = true;
        m_secondPlayerButton.interactable = true;

        m_firstPlayerButtun.onClick.AddListener(() => FirstPlayerButton());
        m_secondPlayerButton.onClick.AddListener(() => SecondPlayerButton());

        while(true)
        {
            if(m_selectTurn != Turn.None)
            {
                break;
            }
            yield return null;
        }

        GameInfo.Game.Turn = m_selectTurn;
    }
}
