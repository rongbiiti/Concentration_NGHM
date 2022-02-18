using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ターン
/// </summary>
public enum Turn
{
    None = -1,
    User01,
    User02,
    Result,
};

/// <summary>
/// ゲームデータ
/// </summary>
[System.Serializable]
public class GameData
{
    /// <summary>
    /// 高さ
    /// </summary>
    public const int Height = 3;

    /// <summary>
    /// 幅
    /// </summary>
    public const int Width = 10;

    /// <summary>
    /// GameName
    /// </summary>
    [SerializeField] private string m_GameName = GameInfo.ApplicationName;
    public string GameName {
        get => m_GameName;
        set => m_GameName = value;
    }

    /// <summary>
    /// RoomID
    /// </summary>
    [SerializeField] private string m_RoomID = string.Empty;
    public string RoomID {
        get => m_RoomID;
        set => m_RoomID = value;
    }

    /// <summary>
    /// User01
    /// </summary>
    [SerializeField] private string m_UserID_01 = string.Empty;
    public string UserID_01 {
        get => m_UserID_01;
        set => m_UserID_01 = value;
    }

    /// <summary>
    /// User02
    /// </summary>
    [SerializeField] private string m_UserID_02 = string.Empty;
    public string UserID_02 {
        get => m_UserID_02;
        set => m_UserID_02 = value;
    }

    /// <summary>
    /// User1が保持しているトランプのペア数
    /// 再起動時に使用
    /// </summary>
    [SerializeField] private List<int> m_TrumpList_User1 = new List<int>();
    public List<int> TrumpList_User1 {
        get => m_TrumpList_User1;
        set => m_TrumpList_User1 = value;
    }

    /// <summary>
    /// User2が保持しているトランプのペア数
    /// 再起動時に使用
    /// </summary>
    [SerializeField] private List<int> m_TrumpList_User2 = new List<int>();
    public List<int> TrumpList_User2 {
        get => m_TrumpList_User2;
        set => m_TrumpList_User2 = value;
    }

    /// <summary>
    /// 選択されたトランプのリスト
    /// </summary>
    [SerializeField] private List<int> m_SelectTrumpList = new List<int>();
    public List<int> SelectTrumpList {
        get => m_SelectTrumpList;
        set => m_SelectTrumpList = value;
    }

    /// <summary>
    /// 盤面の情報
    /// 再起動時に使用
    /// </summary>
    [SerializeField] private int[,] m_CellArray = new int[Height, Width];
    public int[,] CellArray {
        get => m_CellArray;
        set => m_CellArray = value;
    }

    /// <summary>
    /// ターン
    /// </summary>
    [SerializeField] private Turn m_Turn = Turn.None;
    public Turn Turn {
        get => m_Turn;
        set => m_Turn = value;
    }

    /// <summary>
    /// ゲームデータが削除されるまでの制限時間
    /// </summary>
    [SerializeField] private string m_TimeLimit = string.Empty;
    public string timeLimit {
        get => m_TimeLimit;
        set => m_TimeLimit = value;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public GameData()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                m_CellArray[i, j] = (int)TrumpType.None;
            }
        }

        m_Turn = Turn.None;
    }

    /// <summary>
    /// Jsonデータからゲームデータに変換
    /// </summary>
    /// <param name="json">Jsonデータ</param>
    /// <returns>ゲームデータ</returns>
    public static GameData FromJsonConvert(JsonNode json)
    {
        SendGameData sendGameData = SendGameData.Convert(json);
        GameData gameData = new GameData();
        int index = 0;

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                gameData.m_CellArray[i, j] = sendGameData.CellList[index];
                index++;
            }
        }

        gameData.m_GameName = sendGameData.GameName;
        gameData.m_RoomID = sendGameData.RoomID;
        gameData.m_UserID_01 = sendGameData.UserID_01;
        gameData.m_UserID_02 = sendGameData.UserID_02;
        gameData.m_TrumpList_User1 = sendGameData.TrumpList_User1;
        gameData.m_TrumpList_User2 = sendGameData.TrumpList_User2;
        gameData.m_SelectTrumpList = sendGameData.SelectTrumpList;
        gameData.m_Turn = (Turn)sendGameData.Turn;
        gameData.m_TimeLimit = sendGameData.timeLimit;

        return gameData;
    }

    /// <summary>
    /// GameデータからJsonデータに変換
    /// </summary>
    /// <param name="gameData"><ゲームデータ/param>
    /// <returns>Jsonデータ</returns>
    public static string ToJsonConvert(GameData gameData)
    {
        SendGameData sendGameData = SendGameData.CreateData(gameData);
        string json = JsonUtility.ToJson(sendGameData);
        json = "[" + json + "]";
        return json;
    }

    #region Debug

    /// <summary>
    /// ゲームデータの情報をstring型にして返す
    /// </summary>
    /// <returns></returns>
    public string GetStr()
    {
        string str =
            $"RoomID: {m_RoomID}\n" +
            $"User1: {m_UserID_01}\n" +
            $"User2: {m_UserID_02}\n" +
            $"Turn: {m_Turn}\n" +
            $"timeLimit: {m_TimeLimit}\n" +
            $"User1_TrumpList: {GetStrTrumpList_User1()}\n" +
            $"User2_TrumpList: {GetStrTrumpList_User2()}\n" +
            $"SelectTrumpList: {GetStrSelectTrunpList()}\n" +
            $"CellList: {GetStrCellArray()}\n";

        return str;
    }

    /// <summary>
    /// CellArrayの情報をstring型にして返す
    /// </summary>
    /// <returns></returns>
    private string GetStrCellArray()
    {
        string result = "\n";

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (m_CellArray[i, j] == (int)TrumpType.Back)
                {
                    result += "B,";
                }
                else if (m_CellArray[i, j] == (int)TrumpType.None)
                {
                    result += "N,";
                }
                else
                {
                    result += $"{(m_CellArray[i, j] % 13) + 1},";
                }
            }
            result += "\n";
        }

        return result;
    }


    /// <summary>
    /// User1のトランプリスト情報をstring型
    /// </summary>
    /// <returns></returns>
    private string GetStrTrumpList_User1()
    {
        string info = string.Empty;

        for (int i = 0; i < m_TrumpList_User1.Count; i++)
        {
            info += $"{m_TrumpList_User1[i]},";
        }

        return info;
    }

    /// <summary>
    /// User2のトランプリスト情報をstring型
    /// </summary>
    /// <returns></returns>
    private string GetStrTrumpList_User2()
    {
        string info = string.Empty;

        for (int i = 0; i < m_TrumpList_User2.Count; i++)
        {
            info += $"{m_TrumpList_User2[i]},";
        }
        return info;
    }

    /// <summary>
    /// 選択したトランプリスト情報をstring型
    /// </summary>
    /// <returns></returns>
    private string GetStrSelectTrunpList()
    {
        string info = string.Empty;

        for (int i = 0; i < m_SelectTrumpList.Count; i++)
        {
            info += $"{m_SelectTrumpList[i]},";
        }
        return info;
    }

    #endregion
}


/// <summary>
/// 送信するゲームデータ
/// </summary>
[System.Serializable]
public class SendGameData
{
    /// <summary>
    /// GameName
    /// </summary>
    [SerializeField] private string m_GameName = GameInfo.ApplicationName;
    public string GameName => m_GameName;

    /// <summary>
    /// RoomID
    /// </summary>
    [SerializeField] private string m_RoomID = string.Empty;
    public string RoomID => m_RoomID;

    /// <summary>
    /// User01
    /// </summary>
    [SerializeField] private string m_UserID_01 = string.Empty;
    public string UserID_01 => m_UserID_01;

    /// <summary>
    /// User02
    /// </summary>
    [SerializeField] private string m_UserID_02 = string.Empty;
    public string UserID_02 => m_UserID_02;

    /// <summary>
    /// User1が保持しているトランプのペア数
    /// </summary>
    [SerializeField] private List<int> m_TrumpList_User1 = new List<int>();
    public List<int> TrumpList_User1 => m_TrumpList_User1;

    /// <summary>
    /// User2が保持しているトランプのペア数
    /// </summary>
    [SerializeField] private List<int> m_TrumpList_User2 = new List<int>();
    public List<int> TrumpList_User2 => m_TrumpList_User2;

    /// <summary>
    /// 盤面の情報
    /// </summary>
    [SerializeField] private List<int> m_CellList = new List<int>();
    public List<int> CellList => m_CellList;

    /// <summary>
    /// 選択されたトランプのリスト
    /// </summary>
    [SerializeField] private List<int> m_SelectTrumpList = new List<int>();
    public List<int> SelectTrumpList => m_SelectTrumpList;

    /// <summary>
    /// ターン
    /// </summary>
    [SerializeField] private int m_Turn = -1;
    public int Turn => m_Turn;

    /// <summary>
    /// ゲームデータが削除されるまでの制限時間
    /// </summary>
    [SerializeField] private string m_TimeLimit = string.Empty;
    public string timeLimit {
        get => m_TimeLimit;
    }

    /// <summary>
    /// ゲームデータのコンバート
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static SendGameData Convert(JsonNode json)
    {
        SendGameData sendGameData = new SendGameData();

        foreach (var data in json[0]["m_CellList"])
        {
            int num = (int)data.Get<long>();
            sendGameData.m_CellList.Add(num);
        }

        foreach (var data in json[0]["m_TrumpList_User1"])
        {
            int num = (int)data.Get<long>();
            sendGameData.m_TrumpList_User1.Add(num);
        }

        foreach (var data in json[0]["m_TrumpList_User2"])
        {
            int num = (int)data.Get<long>();
            sendGameData.m_TrumpList_User2.Add(num);
        }

        foreach (var data in json[0]["m_SelectTrumpList"])
        {
            int num = (int)data.Get<long>();
            sendGameData.m_SelectTrumpList.Add(num);
        }

        sendGameData.m_GameName = json[0]["m_GameName"].Get<string>();
        sendGameData.m_RoomID = json[0]["m_RoomID"].Get<string>();
        sendGameData.m_UserID_01 = json[0]["m_UserID_01"].Get<string>();
        sendGameData.m_UserID_02 = json[0]["m_UserID_02"].Get<string>();
        sendGameData.m_Turn = (int)json[0]["m_Turn"].Get<long>();
        sendGameData.m_TimeLimit = json[0]["m_TimeLimit"].Get<string>();

        return sendGameData;

    }

    /// <summary>
    /// 送信するゲームデータを作成する
    /// </summary>
    /// <param name="gameData"></param>
    /// <returns></returns>
    public static SendGameData CreateData(GameData gameData)
    {
        SendGameData sendGameData = new SendGameData();

        for (int i = 0; i < GameData.Height; i++)
        {
            for (int j = 0; j < GameData.Width; j++)
            {
                sendGameData.m_CellList.Add(gameData.CellArray[i, j]);
            }
        }

        sendGameData.m_GameName = gameData.GameName;
        sendGameData.m_RoomID = gameData.RoomID;
        sendGameData.m_UserID_01 = gameData.UserID_01;
        sendGameData.m_UserID_02 = gameData.UserID_02;
        sendGameData.m_TrumpList_User1 = gameData.TrumpList_User1;
        sendGameData.m_TrumpList_User2 = gameData.TrumpList_User2;
        sendGameData.m_SelectTrumpList = gameData.SelectTrumpList;
        sendGameData.m_Turn = (int)gameData.Turn;
        sendGameData.m_TimeLimit = gameData.timeLimit;

        return sendGameData;
    }
}