using UnityEngine.UI;
using UnityEngine;
using System.Collections;

/// <summary>
/// �Q�[���f�[�^��\������
/// </summary>
public class GameDataView : UWRHelper
{
    [Header("�Q�[���f�[�^�\���p�e�L�X�g")]
    [SerializeField] private Text m_gameDataText = null;

    /// <summary>
    /// �R���[�`��
    /// </summary>
    private IEnumerator m_Coroutine = null;

    /// <summary>
    /// �\��
    /// </summary>
    private void OnEnable()
    {
        StartCoroutine();
    }

    /// <summary>
    /// ��\��
    /// </summary>
    private void OnDisable()
    {
        EndCoroutine();
    }

    /// <summary>
    /// �R���[�`���J�n
    /// </summary>
    private void StartCoroutine()
    {
        EndCoroutine();
        m_Coroutine = CoGameDataView();
        StartCoroutine(m_Coroutine);
    }

    /// <summary>
    /// �R���[�`�����I������
    /// </summary>
    private void EndCoroutine()
    {
        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
            m_Coroutine = null;
        }
    }

    /// <summary>
    /// �T�[�o�[���̃Q�[���f�[�^��\��
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoGameDataView()
    {
        while (true)
        {
            var uwr = CreateGetUrl(KeyData.GameKey);
            yield return WaitForRequest(uwr);

            if (CheckKey(uwr))
            {
                // �_�o����ł͂Ȃ��ʂ̃Q�[���f�[�^���T�[�o�[��Ɋi�[����Ă�����...
                if (CheckDifferenceGameData(uwr))
                {
                    //Debug.Log("�_�o����ł͂Ȃ��ʂ̃Q�[���f�[�^���T�[�o�[��ɂ���܂��B");
                    m_gameDataText.text = "There is another game data on the server\n not Concentration.";
                }
                // ����ȃQ�[���f�[�^���T�[�o�[��Ɋi�[����Ă�����...
                else
                {
                    GameData gameData = GameData.FromJsonConvert(JsonNode.GetValue(uwr.downloadHandler.text));
                    m_gameDataText.text = gameData.GetStr();
                    m_gameDataText.text += $"MyRoomID: {KeyData.GameKey}\n";
                    m_gameDataText.text += $"MyUserID: {GameInfo.MyUserID}\n";
                    m_gameDataText.text += $"MyTurn: {GameInfo.MyTurn}\n";
                }
            }
            else
            {
                m_gameDataText.text = "No GameData.";
            }

            yield return new WaitForSeconds(DefaultSyncSecond);
        }
    }
}
