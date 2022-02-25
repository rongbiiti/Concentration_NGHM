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

    [SerializeField] private ResultUI m_resultUI;

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
        StartCoroutine(Cort_GameInit());
    }

    private IEnumerator Cort_GameInit()
    {
        m_gameState = GameState.Init;

        if (GameInfo.IsRestart)
        {
            // 前回の状態から再開
            yield return Cort_ReStart();
        }
        else
        {
            // 通常のゲーム開始
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
            yield return Cort_InitUser1();
        }
        else if(GameInfo.MyTurn == Turn.User02)
        {
            yield return Cort_InitUser2();
        }

        yield return Cort_TrumpDeal();

        yield return Cort_Opening();

        m_HUD.SetTurn(GameInfo.Game.Turn);

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
            yield return Cort_Result();
            yield break;
        }

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
                obj.GetComponent<RectTransform>().localPosition = pos;
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
                    GameObject obj = Instantiate(m_trumpPrefab.gameObject, m_HUD.UserInfos[0].TrumpParent.transform);
                    obj.SetActive(true);
                    m_HUD.UserInfos[0].AddPairNumText(2);
                    obj.GetComponent<Trump>().Acquired((TrumpType)GameInfo.Game.TrumpList_User1[i]);
                }

                for (int i = 0; i < GameInfo.Game.TrumpList_User2.Count; i++)
                {
                    GameObject obj = Instantiate(m_trumpPrefab.gameObject, m_HUD.UserInfos[1].TrumpParent.transform);
                    obj.SetActive(true);
                    m_HUD.UserInfos[1].AddPairNumText(2);
                    obj.GetComponent<Trump>().Acquired((TrumpType)GameInfo.Game.TrumpList_User2[i]);
                }
                break;

            case Turn.User02:
                for (int i = 0; i < GameInfo.Game.TrumpList_User1.Count; i++)
                {
                    GameObject obj = Instantiate(m_trumpPrefab.gameObject, m_HUD.UserInfos[1].TrumpParent.transform);
                    obj.SetActive(true);
                    m_HUD.UserInfos[1].AddPairNumText(2);
                    obj.GetComponent<Trump>().Acquired((TrumpType)GameInfo.Game.TrumpList_User1[i]);
                }

                for (int i = 0; i < GameInfo.Game.TrumpList_User2.Count; i++)
                {
                    GameObject obj = Instantiate(m_trumpPrefab.gameObject, m_HUD.UserInfos[0].TrumpParent.transform);
                    obj.SetActive(true);
                    m_HUD.UserInfos[0].AddPairNumText(2);
                    obj.GetComponent<Trump>().Acquired((TrumpType)GameInfo.Game.TrumpList_User2[i]);
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

    public bool CheckSelectTrump(Trump trump)
    {
        if(m_gameState == GameState.Init || m_gameState == GameState.Opening)
        {
            Debug.Log("トランプ選択: ゲーム準備中");
            return false;
        }

        if (trump.Type == TrumpType.None)
        {
            Debug.Log("トランプ選択: 種類が何も設定されていない");
            return false;
        }

        if(trump.Face != TrumpFace.Back)
        {
            Debug.Log("トランプ選択: 表面を選択しています");
            return false;
        }

        if(GameInfo.MyTurn != GameInfo.Game.Turn)
        {
            Debug.Log("トランプ選択: 自身のターンではない");
            return false;
        }

        if(m_selectTrumpCount >= 2)
        {
            Debug.Log("選択無効: 既に二つのトランプを選択しています");
            return false;
        }

        if(m_selectPairList.Count >= 2)
        {
            Debug.Log("選択無効: ペアとなるトランプを選択しています");
            return false;
        }

        if(!m_syncCompletedFlg)
        {
            Debug.Log("選択無効: 全ての同期が完了していません。\n");
            return false;
        }

        return true;
    }

    private bool CheckPair(TrumpType type1, TrumpType type2)
    {
        if(type1 == TrumpType.Back || type2 == TrumpType.Back)
        {
            return false;
        }

        if(type1 == type2)
        {
            return true;
        }

        return false;
    }

    private IEnumerator Cort_ChangeTurn()
    {
        switch (GameInfo.Game.Turn)
        {
            case Turn.User01:
                GameInfo.Game.Turn = Turn.User02;
                break;
            case Turn.User02:
                GameInfo.Game.Turn = Turn.User01;
                break;
        }
        yield break;
    }

    private IEnumerator Cort_Pair()
    {
        int index = GameInfo.Game.Turn == GameInfo.MyTurn ? 0 : 1;

        int counter = 0;

        foreach(var data in m_selectPairList)
        {
            m_trumpList[data.IndexY, data.IndexX].Type = TrumpType.None;
        }

        foreach(var selectTrump in m_selectPairList)
        {
            selectTrump.transform.SetParent(m_HUD.UserInfos[index].TrumpParent.transform);
            StartCoroutine(selectTrump.Cort_Move(Vector3.zero, _trumpMoveTime, () => ++counter));
        }

        yield return new WaitUntil(() => counter == 2);

        m_HUD.UserInfos[index].AddPairNumText(m_selectPairList.Count);
    }

    private IEnumerator Cort_FaceDown()
    {
        int counter = 0;

        foreach( var selectTrump in m_selectPairList)
        {
            StartCoroutine(selectTrump.Cort_FaceDown(_trumpRotateTime, () => ++counter));
        }

        yield return new WaitUntil(() => counter == 2);
        yield return new WaitForSeconds(0.1f);

        foreach(var data in m_selectPairList)
        {
            m_trumpList[data.IndexY, data.IndexX].TrumpEventTrigger.enabled = true;
        }
    }

    private IEnumerator Cort_SelectTrump(Trump trump)
    {
        m_selectTrumpCount++;
        trump.TrumpEventTrigger.enabled = false;

        yield return trump.Cort_FaceUp(_trumpRotateTime);
        m_selectPairList.Add(trump);

        m_selectTrumpList.Add(trump.Number);

        if (m_selectPairList.Count == 2 && CheckPair(m_selectPairList[0].Type, m_selectPairList[1].Type))
        {
            int type = (int)m_selectPairList[1].Type;
            switch (GameInfo.MyTurn)
            {
                case Turn.User01:
                    GameInfo.Game.TrumpList_User1.Add(type);
                    break;
                case Turn.User02:
                    GameInfo.Game.TrumpList_User2.Add(type);
                    break;
            }

            foreach (var data in m_selectPairList)
            {
                GameInfo.Game.CellArray[data.IndexY, data.IndexX] = (int)TrumpType.None;
            }
        }
        else if(m_selectPairList.Count == 2 && !CheckPair(m_selectPairList[0].Type, m_selectPairList[1].Type))
        {
            yield return Cort_ChangeTurn();
        }

        yield return Cort_SendGameData();

        if (m_selectPairList.Count == 2)
        {
            yield return new WaitForSeconds(_trumpDisplayTime);

            if(CheckPair(m_selectPairList[0].Type, m_selectPairList[1].Type))
            {
                yield return Cort_Pair();
            }
            else
            {
                yield return Cort_FaceDown();
                m_HUD.SetTurn(GameInfo.Game.Turn);
                m_syncCompletedFlg = false;
            }

            m_selectTrumpCount = 0;
            m_selectPairList.Clear();

            if(CheckGameEnd())
            {
                yield return Cort_Result();
            }
        }
    }

    private IEnumerator Cort_SelectTrumpSync(Trump trump)
    {
        trump.TrumpEventTrigger.enabled = false;

        yield return trump.Cort_FaceUp(_trumpRotateTime);
        m_selectPairList.Add(trump);

        if(m_selectPairList.Count == 2)
        {
            yield return new WaitForSeconds(_trumpDisplayTime);

            if (CheckPair(m_selectPairList[0].Type, m_selectPairList[1].Type))
            {
                yield return Cort_Pair();
            }
            else
            {
                yield return Cort_FaceDown();
                m_HUD.SetTurn(GameInfo.Game.Turn);
                m_syncCompletedFlg = true;
            }
            m_selectPairList.Clear();

            if (CheckGameEnd())
            {
                yield return Cort_Result();
            }
        }
    }

    public void OnClick_SelectTrump(Trump trump)
    {
        if(CheckSelectTrump(trump))
        {
            StartCoroutine(Cort_SelectTrump(trump));
        }
    }

    public IEnumerator Cort_ScreenSync(GameData gameData)
    {
        if(!m_syncCompletedFlg)
        {
            GameInfo.Game = gameData;

            if (GameInfo.Game.SelectTrumpList.Count > m_selectTrumpList.Count)
            {
                for(int i = m_selectTrumpList.Count; i < GameInfo.Game.SelectTrumpList.Count; i++)
                {
                    int index = GameInfo.Game.SelectTrumpList[i];
                    int indexH = index / GameData.Width;
                    int indexW = index % GameData.Width;
                    yield return Cort_SelectTrumpSync(m_trumpList[indexH, indexW]);
                    m_selectTrumpList.Add(index);
                }
            }
        }
    }

    private IEnumerator Cort_SendGameData()
    {
        if (GameInfo.Game.SelectTrumpList.Count < m_selectTrumpList.Count)
        {
            for (int i =GameInfo.Game.SelectTrumpList.Count; i < m_selectTrumpList.Count; i++)
            {
                GameInfo.Game.SelectTrumpList.Add(m_selectTrumpList[i]);
            }
        }

        yield return m_gameDataSyncer.Cort_UpdateGameData(GameInfo.Game);
    }

    private IEnumerator Cort_Result()
    {
        m_gameState = GameState.Result;

        m_gameDataSyncer.StopGameSync();

        if(m_HUD.UserInfos[0].PairNum > m_HUD.UserInfos[1].PairNum)
        {
            yield return m_resultUI.Cort_Win();
        }
        else
        {
            yield return m_resultUI.Cort_Lose();
        }
    }
}
