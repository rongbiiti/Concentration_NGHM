using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Serializable]
    public class UserInfo
    {
        [SerializeField] private Text m_pairNumText;

        [SerializeField] private GameObject m_trumpParent;
        public GameObject TrumpParent {
            get { return m_trumpParent; }
        }

        private int m_pairNum;
        public int PairNum {
            get { return m_pairNum; }
        }

        public void AddPairNumText(int add)
        {
            m_pairNum += add;
            m_pairNumText.text = m_pairNum.ToString();
        }
    }

    [SerializeField] private UserInfo[] m_userInfos;
    public UserInfo[] UserInfos => m_userInfos;

    [SerializeField] private Image m_messageImageUI;

    [SerializeField] private Sprite[] m_turnList;

    public void SetTurn(Turn turn)
    {
        if (GameInfo.MyTurn == turn)
        {
            m_messageImageUI.sprite = m_turnList[0];
        }
        else if( GameInfo.OpponentTurn == turn)
        {
            m_messageImageUI.sprite = m_turnList[1];
        }
    }


}
