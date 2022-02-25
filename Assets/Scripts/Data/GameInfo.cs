
/// <summary>
/// �g�p����URL�̃^�C�v
/// </summary>
public enum URLType
{
    /// <summary>
    /// Info��
    /// </summary>
    Info,

    /// <summary>
    /// �J����
    /// </summary>
    Develop,

    /// <summary>
    /// �N�A�h���̃����^���T�[�o�[��
    /// </summary>
    Quadra,

    /// <summary>
    /// �{�Ԋ�
    /// </summary>
    StudyCompas,
}

/// <summary>
/// �Q�[�����
/// </summary>
public class GameInfo
{
    /// <summary>
    /// �g�����v����\���i���ʃg�����v�̐��l��������j
    /// </summary>
    public static bool IsTrumpInfo = false;

    /// <summary>
    /// �t���[�����[�g
    /// </summary>
    public const int FrameRate = 60;

    /// <summary>
    /// ���g�̃��[�U�[ID
    /// </summary>
    public static string MyUserID = "893598";

    /// <summary>
    /// ���g�̃^�[��
    /// </summary>
    public static Turn MyTurn = Turn.None;

    /// <summary>
    /// �Q�[���f�[�^
    /// </summary>
    public static GameData Game = default;

    /// <summary>
    /// �g�p����URL�̃^�C�v
    /// </summary>
    public static URLType URLType = URLType.Quadra;

    /// <summary>
    /// �ċN�����ꂽ���H
    /// </summary>
    public static bool IsRestart = false;

    /// <summary>
    /// ������
    /// </summary>
    public static int WinCount = 0;

    /// <summary>
    /// �s�k��
    /// </summary>
    public static int LoseCount = 0;

    /// <summary>
    /// �A�v���P�[�V������
    /// </summary>
    public static string ApplicationName = "concentration";

    /// <summary>
    /// �ΐ푊��̃^�[��
    /// </summary>
    public static Turn OpponentTurn {
        get => GameInfo.MyTurn == Turn.User01 ? Turn.User02 : Turn.User01;
    }
}