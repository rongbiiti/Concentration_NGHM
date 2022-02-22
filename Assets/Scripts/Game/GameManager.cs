using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ゲーム状態
/// </summary>
enum GameState
{
    Init,
    Opening,
    Game,
    Result,
}

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    /// <summary>
    /// トランプの生成リスト
    /// </summary>
    private TrumpType[,] CreateTrumpList = new TrumpType[,]
    {
        {
            TrumpType.S_1,  TrumpType.S_1,  TrumpType.S_2,  TrumpType.S_2,
            TrumpType.S_3,  TrumpType.S_3,  TrumpType.S_4,  TrumpType.S_4,
            TrumpType.S_5,  TrumpType.S_5,  TrumpType.S_6,  TrumpType.S_6,
            TrumpType.S_7,  TrumpType.S_7,  TrumpType.S_8,  TrumpType.S_8,
            TrumpType.S_9,  TrumpType.S_9,  TrumpType.S_10, TrumpType.S_10,
            TrumpType.S_11, TrumpType.S_11, TrumpType.S_12, TrumpType.S_12,
            TrumpType.S_13, TrumpType.S_13, TrumpType.C_1,  TrumpType.C_1,
            TrumpType.C_2,  TrumpType.C_2,  TrumpType.C_3,  TrumpType.C_3,
        },

        {
            TrumpType.C_1,  TrumpType.C_1,  TrumpType.C_2,  TrumpType.C_2,
            TrumpType.C_3,  TrumpType.C_3,  TrumpType.C_4,  TrumpType.C_4,
            TrumpType.C_5,  TrumpType.C_5,  TrumpType.C_6,  TrumpType.C_6,
            TrumpType.C_7,  TrumpType.C_7,  TrumpType.C_8,  TrumpType.C_8,
            TrumpType.C_9,  TrumpType.C_9,  TrumpType.C_10, TrumpType.C_10,
            TrumpType.C_11, TrumpType.C_11, TrumpType.C_12, TrumpType.C_12,
            TrumpType.C_13, TrumpType.C_13, TrumpType.D_1,  TrumpType.D_1,
            TrumpType.D_2,  TrumpType.D_2,  TrumpType.D_3,  TrumpType.D_3,
        },

        {
            TrumpType.D_1,  TrumpType.D_1,  TrumpType.D_2,  TrumpType.D_2,
            TrumpType.D_3,  TrumpType.D_3,  TrumpType.D_4,  TrumpType.D_4,
            TrumpType.D_5,  TrumpType.D_5,  TrumpType.D_6,  TrumpType.D_6,
            TrumpType.D_7,  TrumpType.D_7,  TrumpType.D_8,  TrumpType.D_8,
            TrumpType.D_9,  TrumpType.D_9,  TrumpType.D_10, TrumpType.D_10,
            TrumpType.D_11, TrumpType.D_11, TrumpType.D_12, TrumpType.D_12,
            TrumpType.D_13, TrumpType.D_13, TrumpType.S_1,  TrumpType.S_1,
            TrumpType.S_2,  TrumpType.S_2,  TrumpType.S_3,  TrumpType.S_3,
        },

        {
            TrumpType.H_1,  TrumpType.H_1,  TrumpType.H_2,  TrumpType.H_2,
            TrumpType.H_3,  TrumpType.H_3,  TrumpType.H_4,  TrumpType.H_4,
            TrumpType.H_5,  TrumpType.H_5,  TrumpType.H_6,  TrumpType.H_6,
            TrumpType.H_7,  TrumpType.H_7,  TrumpType.H_8,  TrumpType.H_8,
            TrumpType.H_9,  TrumpType.H_9,  TrumpType.H_10, TrumpType.H_10,
            TrumpType.H_11, TrumpType.H_11, TrumpType.H_12, TrumpType.H_12,
            TrumpType.H_13, TrumpType.H_13, TrumpType.S_1,  TrumpType.S_1,
            TrumpType.S_2,  TrumpType.S_2,  TrumpType.S_3,  TrumpType.S_3,
        },
    };

    [SerializeField] private GameDataSyncer m_gameDataSyncer;

    [SerializeField] private GameObject m_trumpParent;

    [SerializeField] private Trump m_trumpPrefab;

    [SerializeField] private HUD m_HUD;

    [SerializeField] private StartUI m_startUI;

    //[SerializeField] private ResultUI m_resultUI;

    [SerializeField] private float _trumpRotateTime = 0.5f;

    [SerializeField] private float _trumpMoveTime = 1f;

    [SerializeField] private float _trumpDisplayTime = 0.5f;

    [SerializeField] private float _trumpDealTime = 0.1f;

    private Trump[,] m_trumpList = new Trump[GameData.Height, GameData.Width];

    private Vector2[,] m_trumpPosArray = new Vector2[GameData.Height, GameData.Width];

    private List<Trump> m_selectPairList = new List<Trump>();

    private List<int> m_selectTrumpList = new List<int>();

    private GameState m_gameState = GameState.Init;

    private bool m_syncCompletedFlg;

    private int m_selectTrumpCount;

    private void Start()
    {
        Debug.Log("Start");
        StartCoroutine(Cort_GameInit());
    }

    private IEnumerator Cort_GameInit()
    {
        m_gameState = GameState.Init;

        if (GameInfo.IsRestart)
        {
            // 前回の状態から再開
            yield return Cort_GameStart();
        }
        else
        {
            // 通常のゲーム開始
            Debug.Log("Cort_GameStart");
            yield return Cort_GameStart();
        }
    }

    private IEnumerator Cort_GameStart()
    {
        if(GameInfo.MyTurn == GameInfo.Game.Turn)
        {
            m_syncCompletedFlg = true;
        }
        else
        {
            m_syncCompletedFlg = false;
        }

        
        if (GameInfo.MyTurn == Turn.User01)
        {
            Debug.Log("User1初期化開始");
            yield return Cort_InitUser1();
            Debug.Log("User1初期化");
        }
        else if(GameInfo.MyTurn == Turn.User02)
        {
            
            yield return Cort_InitUser2();
            Debug.Log("User2初期化");
        }

        yield return Cort_TrumpDeal();

        yield return Cort_Opening();

        m_gameDataSyncer.StartGameSync();

        m_gameState = GameState.Game;
    }

    private IEnumerator Cort_InitUser1()
    {
        SetUpTrumpArray();
        CreateTrumpObj();
        yield return m_gameDataSyncer.Cort_UpdateGameData(GameInfo.Game);
        
    }

    private void SetUpTrumpArray()
    {
        int aryNum = Random.Range(0, 4);
        List<TrumpType> tmpTrumpList = new List<TrumpType>();
        int index = 0;

        for (int i = 0; i < GameData.Height; i++)
        {
            for (int j = 0; j < GameData.Width; j++)
            {
                tmpTrumpList.Add(CreateTrumpList[aryNum, index]);
                index++;
            }
        }

        System.Random random = new System.Random();
        TrumpType[] shuffleList = tmpTrumpList.OrderBy(item => random.Next()).ToArray();

        index = 0;
        for (int i = 0; i < GameData.Height; i++)
        {
            for (int j = 0; j < GameData.Width; j++)
            {
                GameInfo.Game.CellArray[i, j] = (int)shuffleList[index];
                index++;
            }
        }

        Debug.Log("SetUpTrumpArray : OK");
    }

    private void CreateTrumpObj()
    {
        Vector2 cellSize = m_trumpPrefab.GetComponent<RectTransform>().sizeDelta;
        Vector2 startPos = new Vector2(-284f, 112.5f);
        Vector2 offset = new Vector2(63f, 94f);

        int index = 0;
        for (int i = 0; i < GameData.Height; i++)
        {
            for(int j = 0; j < GameData.Width; j++)
            {
                GameObject obj = (GameObject)Instantiate(m_trumpPrefab.gameObject, m_trumpParent.transform);
                obj.SetActive(true);
                Vector2 pos = startPos + new Vector2(offset.x * j, -offset.y * i);
                obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
                m_trumpPosArray[i, j] = pos;
                m_trumpList[i, j] = obj.GetComponent<Trump>();
                m_trumpList[i, j].SetUp(j, i, index, (TrumpType)GameInfo.Game.CellArray[i, j]);
                index++;
            }
        }
    }

    private IEnumerator Cort_InitUser2()
    {
        
        yield return m_gameDataSyncer.Cort_SetUpDataSync_User2();
        Debug.Log("CreateTrump");
        CreateTrumpObj();
    }

    private IEnumerator Cort_TrumpDeal()
    {
        for(int i = 0; i < GameData.Height; i++)
        {
            for (int j = 0; j < GameData.Width; j++)
            {
                
                yield return m_trumpList[i, j].Cort_DealMove(m_trumpPosArray[i, j], _trumpDealTime, null);
                
            }
        }
    }

    private IEnumerator Cort_Opening()
    {
        m_gameState = GameState.Opening;
        yield return m_startUI.Cort_Start(0.25f, 0.25f);
    }

    private IEnumerator Cort_ReStart()
    {
        GameInfo.IsRestart = false;

        if (GameInfo.MyTurn == GameInfo.Game.Turn)
        {
            m_syncCompletedFlg = true;
        }
        else
        {
            m_syncCompletedFlg = false;
        }

        GetCreateTrumpList();

        m_HUD.SetTurn(GameInfo.Game.Turn);

        if (CheckGameEnd())
        {
            //yield return Cort_Result();
            yield break;
        }

        GetCreateTrumpList();

        m_gameDataSyncer.StartGameSync();

        m_gameState = GameState.Game;
    }

    private void GetCreateTrumpList()
    {
        Vector2 cellSize = m_trumpPrefab.GetComponent<RectTransform>().sizeDelta;
        Vector2 startPos = new Vector2(-284f, 112.5f);
        Vector2 offset = new Vector2(63f, 94f);

        int index = 0;
        for (int i = 0; i < GameData.Height; i++)
        {
            for (int j = 0; j < GameData.Width; j++)
            {
                GameObject obj = (GameObject)Instantiate(m_trumpPrefab.gameObject, m_trumpParent.transform);
                obj.SetActive(true);
                Vector2 pos = startPos + new Vector2(offset.x * j, -offset.y * i);
                obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
                m_trumpPosArray[i, j] = pos;
                m_trumpList[i, j] = obj.GetComponent<Trump>();
                m_trumpList[i, j].SetUp(j, i, index, (TrumpType)GameInfo.Game.CellArray[i, j]);

                if(GameInfo.Game.CellArray[i,j] == (int)TrumpType.None)
                {
                    m_trumpList[i, j].Acquired(TrumpType.None);
                    m_trumpList[i, j].gameObject.SetActive(false);
                }

                index++;
            }
        }

        foreach(var data in GameInfo.Game.SelectTrumpList)
        {
            m_selectTrumpList.Add(data);
        }

        if(m_selectTrumpList.Count % 2 != 0)
        {
            int indexT = m_selectTrumpList[m_selectTrumpList.Count - 1];
            int indexH = indexT / GameData.Width;
            int indexW = indexT % GameData.Width;
            m_trumpList[indexH, indexW].FaceUp();
            m_selectPairList.Add(m_trumpList[indexH, indexW]);

            if(GameInfo.MyTurn == GameInfo.Game.Turn)
            {
                m_selectTrumpCount++;
            }
        }

        switch(GameInfo.MyTurn)
        {
            case Turn.User01:
                for(int i = 0; i < GameInfo.Game.TrumpList_User1.Count; i++)
                {

                }

                for (int i = 0; i < GameInfo.Game.TrumpList_User2.Count; i++)
                {

                }
                break;

            case Turn.User02:
                for (int i = 0; i < GameInfo.Game.TrumpList_User1.Count; i++)
                {

                }

                for (int i = 0; i < GameInfo.Game.TrumpList_User2.Count; i++)
                {

                }
                break;
        }
    }

    private bool CheckGameEnd()
    {
        for(int i = 0; i < GameData.Height; i++)
        {
            for(int j = 0; j < GameData.Width; j++)
            {
                if(m_trumpList[i,j].Type != TrumpType.None)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
