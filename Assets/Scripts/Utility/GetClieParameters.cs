using UnityEngine;

/// <summary>
/// �p�����[�^�擾
/// UserID GameKey�擾
/// </summary>
public class GetClieParameters : MonoBehaviour
{
    /// <summary>
    /// �擾����UserID
    /// </summary>
    public static string m_UserId = null;

    /// <summary>
    /// �擾����GameKey
    /// </summary>
    public static string m_GameKey = null;

    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Application.ExternalEval("SendMessage('" + this.transform.root.name + "', 'RecieveText', window.location.search)");
            //Debug.LogError("Application.ExternalEval" + "���Ăяo����܂���");
        }
    }

    /// <summary>
    /// ��M�e�L�X�g
    /// </summary>
    /// <param name="text"></param>
    void RecieveText(string text)
    {
        GetQueryString(text);
    }

    /// <summary>
    /// �p�����[�^�[�𕪉����邽�߂̊֐�
    /// �����p�Ƀ`���[�j���O��������
    /// </summary>
    /// <returns>The query string.</returns>
    string GetQueryString(string text)
    {
        string[] getParams = text.Substring(1).Split('&');
        int len = getParams.Length;

        for (int i = 0; i < len; ++i)
        {
            string[] param = getParams[i].Split('=');

            if (param[0] == "userId")
            {
                m_UserId = param[1];
                Debug.Log("userId:" + param[1]);
            }

            if (param[0] == "progressesId")
            {
                m_GameKey = param[1];
                Debug.Log("progressesId" + param[1]);
            }
        }

        return "";
    }
}
