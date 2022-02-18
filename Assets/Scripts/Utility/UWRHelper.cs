using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/// <summary>
///�@�ʐM����ۂɌp�����Ďg���⏕�֐��Q
///�@UnityWebRequest�ʐM���s��
/// </summary>
public class UWRHelper : MonoBehaviour
{
    /// <summary>
    /// �f�[�^�������ɑҋ@����f�t�H���g�̎���
    /// </summary>
    protected static float DefaultSyncSecond = 0.5f;

    /// <summary>
    /// �^�C�����~�b�g�̃f�t�H���g����
    /// </summary>
    protected static float DefaultTimeLimitSecond = 180f;

    /// <summary>
    /// �^�C�����~�b�g�p�R���[�`��
    /// </summary>
    private IEnumerator m_Coroutine = null;

    /// <summary>
    /// �f�[�^�擾�pURL
    /// </summary>
    /// <param name="key">�擾���Ă���f�[�^�̃L�[������</param>
    /// <returns>(�ʐM�p)�f�[�^</returns>
    public UnityWebRequest CreateGetUrl(string key)
    {
        UnityWebRequest uwr = default;

        switch (GameInfo.URLType)
        {
            case URLType.Develop:
                uwr = UnityWebRequest.Get(KeyValueHelper.CreateGetUrl_Develop(key));
                break;
            case URLType.Info:
                uwr = UnityWebRequest.Get(KeyValueHelper.CreateGetUrl_Info(key));
                break;
            case URLType.Quadra:
                uwr = UnityWebRequest.Get(KeyValueHelper.CreateGetUrl_Quadra(key));
                break;
            case URLType.StudyCompas:
                uwr = UnityWebRequest.Get(KeyValueHelper.CreateGetUrl_StudyCompass(key));
                break;
        }
        return uwr;
    }

    /// <summary>
    /// �f�[�^�ǉ����X�V�pURL
    /// </summary>
    /// <param name="key">�ύX�������f�[�^�̃L�[������</param>
    /// <returns>(�ʐM�p)�f�[�^</returns>
    public UnityWebRequest CreateSetUrl(string key, string text)
    {
        UnityWebRequest uwr = default;

        switch (GameInfo.URLType)
        {
            case URLType.Develop:
                WWWForm form = new WWWForm();
                form.AddField("key", key);
                form.AddField("value", text);
                uwr = UnityWebRequest.Post(KeyValueHelper.CreateUpdateUrl_Develop(), form);
                break;
            case URLType.Info:
                WWWForm formInfo = new WWWForm();
                formInfo.AddField("key", key);
                formInfo.AddField("value", text);
                uwr = UnityWebRequest.Post(KeyValueHelper.CreateUpdateUrl_Info(), formInfo);
                break;
            case URLType.Quadra:
                uwr = UnityWebRequest.Get(KeyValueHelper.CreateUpdateUrl_Quadra(key, text));
                break;
            // �{�Ԋ�
            case URLType.StudyCompas:
                WWWForm formStudy = new WWWForm();
                formStudy.AddField("key", key);
                formStudy.AddField("value", text);
                uwr = UnityWebRequest.Post(KeyValueHelper.CreateUpdateUrl_StudyCompass(), formStudy);
                break;
        }

        return uwr;
    }

    /// <summary>
    /// �f�[�^�̍폜�pURL
    /// </summary>
    /// <param name="key">>�ύX�������f�[�^�̃L�[<</param>
    /// <returns></returns>
    public UnityWebRequest CreateDeleteUrl(string key)
    {
        UnityWebRequest uwr = default;

        switch (GameInfo.URLType)
        {
            case URLType.Develop:
                WWWForm form = new WWWForm();
                form.AddField("key", key);
                uwr = UnityWebRequest.Post(KeyValueHelper.CreateDeleteUrl_Develop(), form);
                break;
            case URLType.Info:
                WWWForm formInfo = new WWWForm();
                formInfo.AddField("key", key);
                uwr = UnityWebRequest.Post(KeyValueHelper.CreateDeleteUrl_Info(), formInfo);
                break;
            case URLType.Quadra:
                uwr = UnityWebRequest.Get(KeyValueHelper.CreateDeleteUrl_Quadra(key));
                break;
            case URLType.StudyCompas:
                WWWForm formStudy = new WWWForm();
                formStudy.AddField("key", key);
                uwr = UnityWebRequest.Post(KeyValueHelper.CreateDeleteUrl_StudyCompass(), formStudy);
                break;
        }

        return uwr;
    }


    /// <summary>
    /// ���s���ǉ����X�V�pURL
    /// </summary>
    /// <param name="usrId">���[�U�[ID</param>
    /// <param name="roomId">���[��ID</param>
    /// <param name="winCount">������</param>
    /// <param name="loseCount">�s�k��</param>
    /// <param name="applicationName">�A�v���P�[�V�����̖��O</param>
    /// <returns></returns>
    public UnityWebRequest ResultSetUrl(int usrId, int roomId, int winCount, int loseCount, string applicationName)
    {
        WWWForm form = new WWWForm();
        form.AddField("uid", usrId);
        form.AddField("roomId", roomId);
        form.AddField("winCount", winCount);
        form.AddField("loseCount", loseCount);
        form.AddField("appName", applicationName);
        UnityWebRequest request = UnityWebRequest.Post(KeyValueHelper.ResultSetUrl(), form);
        return request;
    }

    /// <summary>
    /// �ʐM�p�R���[�`��
    /// </summary>
    /// <param name="request">CreateGetUrl�Ȃǂō����UnityWebRequest�f�[�^������</param>
    /// <returns></returns>
    protected IEnumerator WaitForRequest(UnityWebRequest request)
    {
        // �^�C���A�E�g�ݒ�
        //request.timeout = 10;

        // �ʐM�҂�...
        yield return request.SendWebRequest();

        // HTTP�X�e�[�^�X�R�[�h���G���[�������Ă�����...
        if (request.isHttpError)
        {
            GameObject msgBox = (GameObject)Instantiate((GameObject)Resources.Load("MessageBox"));
            msgBox.GetComponent<MessageBox>().Initialize_Ok("Communication Error", $"Response Code : {request.responseCode}\nReturn to the matching screen.", () => FadeManager.Instance.LoadScene(1,1f));
            while (true) { yield return null; }
        }
        // UnityWebRequest�̃V�X�e���G���[������������...
        // ��̓I�ɂ́ADNS�������ł��Ȃ��A���_�C���N�g�G���[�A�^�C���A�E�g�Ȃǂ�����ɓ�����܂��B
        else if (request.isNetworkError)
        {
            // �^�C���A�E�g���Ă�����...
            if (request.error == "Request timeout")
            {
                GameObject msgBox = (GameObject)Instantiate((GameObject)Resources.Load("MessageBox"));
                msgBox.GetComponent<MessageBox>().Initialize_Ok("Communication Error", $"No response.\nReturn to the matching screen.", () => FadeManager.Instance.LoadScene(1,1f));
                while (true) { yield return null; }
            }
            else
            {
                GameObject msgBox = (GameObject)Instantiate((GameObject)Resources.Load("MessageBox"));
                msgBox.GetComponent<MessageBox>().Initialize_Ok("Communication Error", $"Unable to connect to the network.\nReturn to the matching screen.", () => FadeManager.Instance.LoadScene(1,1f));
                while (true) { yield return null; }
            }
        }
    }

    /// <summary>
    /// ���X�|���X�f�[�^������Ȃ̂��m�F
    /// </summary>
    /// <param name="www">���X�|���X�f�[�^</param>
    /// <returns>true : ����, false : �G���[</returns>
    protected bool CheckKey(UnityWebRequest request)
    {
        if (request.downloadHandler == null)
        {
            return false;
        }

        //byte[] results = request.downloadHandler.data;
        //Debug.Log(convs(results));
        switch (GameInfo.URLType)
        {
            case URLType.Develop:
            case URLType.Info:
            case URLType.StudyCompas:
                return request.downloadHandler.text.IndexOf("success") != -1;
            case URLType.Quadra:
                return request.downloadHandler.text.IndexOf("key") != -1;
        }

        Debug.LogError("CheckKey���\�b�h�ŃG���[���������܂���");
        return default;
    }

    /// <summary>
    /// �T�[�o�[��̃Q�[���f�[�^�ƌ��݂̃Q�[���f�[�^�ɈႢ�����邩���ׂ�B
    /// TRUE: ����@FALSE: �Ȃ�
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    protected bool CheckDifferenceGameData(UnityWebRequest request)
    {
        // �T�[�o�[��ɐ_�o����̃f�[�^������΂��̏�����ʂ�
        if (request.downloadHandler.text.Contains(GameInfo.ApplicationName))
        {
            //Debug.Log("�f�[�^�𕜋A�����܂�");
            return false;
        }

        //Debug.Log("�T�[�o�[��̃Q�[���f�[�^�ƌ��݂̃Q�[���f�[�^�ɈႢ������܂����B\n�Q�[���f�[�^���폜���܂�");
        return true;
    }

    /// <summary>
    /// �f�[�^���O�o��
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public string convs(byte[] data)
    {
        return new String(Array.ConvertAll(data, x => (char)x));
    }

    /// <summary>
    /// �������ԃ`�F�b�N�J�n
    /// </summary>
    /// <param name="second">�b��</param>
    protected void StartCheckTimeLimit(float second)
    {
        EndCoroutine();
        m_Coroutine = CoCheckTimeLimit(second);
        StartCoroutine(m_Coroutine);
    }

    /// <summary>
    /// �������ԃ`�F�b�N�I��
    /// </summary>
    protected void EndCheckTimeLimit()
    {
        EndCoroutine();
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
    /// �������Ԃ��߂��Ă��Ȃ������ׂ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoCheckTimeLimit(float second)
    {
        yield return new WaitForSeconds(second);
        GameObject msgBox = (GameObject)Instantiate((GameObject)Resources.Load("MessageBox"));
        msgBox.GetComponent<MessageBox>().Initialize_Ok("Time out", $"There is no response from the opponent.\nReturn to the title.", () =>
        {
            FadeManager.Instance.LoadScene(0, 1f);
        });
        while (true) { yield return null; }
    }

    /// <summary>
    /// �Q�[���f�[�^���폜
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoDeleteGameData()
    {
        // �Q�[���f�[�^�폜
        var uwr = CreateDeleteUrl(KeyData.GameKey);
        yield return WaitForRequest(uwr);

        Debug.Log("�Q�[���f�[�^�폜");
    }
}
