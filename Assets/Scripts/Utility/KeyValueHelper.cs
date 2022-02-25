using UnityEngine;
using System.Collections;

// �yinfo���z
// �쐬/�X�V
// https://www.studycompass.info/cloudt/gamedata/update
// �擾
// https://www.studycompass.info/cloudt/gamedata/get
// �폜
// https://www.studycompass.info/cloudt/gamedata/delete

// �y�J�����z
// �쐬/�X�V
// http://localhost:80/concent_testBuild/cloudt/gamedata/update
// �擾
// http://localhost:80/concent_testBuild/cloudt/gamedata/get
// �폜
// http://localhost:80/concent_testBuild/cloudt/gamedata/delete


/// <summary>
/// WWW�ʐM�pURL�����N���X
/// </summary>
public class KeyValueHelper
{
    /// <summary>
    /// BaseURL �N�A�h����
    /// </summary>
    private const string BaseUrl_Quadra = "http://www16306uf.sakura.ne.jp/Quiz/QuizIndex.php?proc=";

    /// <summary>
    /// BaseURL �J�����p
    /// </summary>
    private const string BaseUrl_Develop = "http://localhost:80/cloudt/gamedata/";

    /// <summary>
    /// BaseURL Info���p
    /// </summary>
    private const string BaseUrl_Info = "https://www.studycompass.info/cloudt/gamedata/";

    /// <summary>
    /// BaseURL �{�Ԋ��p
    /// </summary>
    private const string BaseUrl_studyCompass = "https://www.studycompass.net/cloudt/gamedata/";

    /// <summary>
    /// ���ʑ��M�pURL Info���p
    /// </summary>
    static string infoResultUrl = "https://www.studycompass.info/cloudt/game/result/save";

    /// <summary>
    /// ���ʑ��M�pURL develop���p
    /// </summary>
    static string developResultUrl = "http://localhost:80/cloudt/game/result/save";

    /// <summary>
    /// ���ʑ��M�pURL �{�Ԋ��p
    /// </summary>
    static string productionResultUrl = "https://www.studycompass.net/cloudt/game/result/save";


    #region �N�A�h����

    /// <summary>
    /// Key���S��v�̃��R�[�h�擾�pURL�쐬
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string CreateGetUrl_Quadra(string key)
    {
        string url;
        url = BaseUrl_Quadra + "getValueWithKey&apriId=devQuiz&key=" + key;
        return url;
    }

    /// <summary>
    /// Key�ƕ�����v�̃��R�[�h�ݒ�/�X�VURL�쐬
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string CreateUpdateUrl_Quadra(string key, string value)
    {
        string url;
        url = BaseUrl_Quadra + "setValueWithKey&apriId=devQuiz&key=" + key + "&value=" + value;
        return url;
    }

    /// <summary>
    /// Key�ƕ�����v�̃��R�[�h�폜
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string CreateDeleteUrl_Quadra(string key)
    {
        string url;
        url = BaseUrl_Quadra + "deleteValueWithKey&apriId=devQuiz&key=" + key;
        return url;
    }

    #endregion

    #region �{�ԗp��

    /// <summary>
    /// Key���S��v�̃��R�[�h�擾�pURL�쐬
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string CreateGetUrl_Info(string key)
    {
        string url;
        url = BaseUrl_Info + "get?key=" + key;
        return url;
    }

    /// <summary>
    /// ���R�[�h��S�č폜
    /// </summary>
    /// <returns></returns>
    public static string CreateDeleteUrl_Info()
    {
        string url;
        url = BaseUrl_Info + "delete";
        return url;
    }

    /// <summary>
    /// Key�Ɗ��S��v�̃��R�[�h��ۑ�
    /// </summary>
    /// <returns></returns>
    public static string CreateUpdateUrl_Info()
    {
        string url;
        url = BaseUrl_Info + "update";
        return url;
    }

    #endregion

    #region �J����

    /// <summary>
    /// Key���S��v�̃��R�[�h�擾�pURL�쐬
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string CreateGetUrl_Develop(string key)
    {
        string url;
        url = BaseUrl_Develop + "get?key=" + key;
        return url;
    }

    /// <summary>
    /// ���R�[�h��S�č폜
    /// </summary>
    /// <returns></returns>
    public static string CreateDeleteUrl_Develop()
    {
        string url;
        url = BaseUrl_Develop + "delete";
        return url;
    }

    /// <summary>
    /// Key�Ɗ��S��v�̃��R�[�h��ۑ�
    /// </summary>
    /// <returns></returns>
    public static string CreateUpdateUrl_Develop()
    {
        string url;
        url = BaseUrl_Develop + "update";
        return url;
    }

    #endregion

    #region studyCompass

    /// <summary>
    /// Key���S��v�̃��R�[�h�擾�pURL�쐬
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string CreateGetUrl_StudyCompass(string key)
    {
        string url;
        url = BaseUrl_studyCompass + "get?key=" + key;
        return url;
    }

    /// <summary>
    /// ���R�[�h��S�č폜
    /// </summary>
    /// <returns></returns>
    public static string CreateDeleteUrl_StudyCompass()
    {
        string url;
        url = BaseUrl_studyCompass + "delete";
        return url;
    }

    /// <summary>
    /// Key�Ɗ��S��v�̃��R�[�h��ۑ�
    /// </summary>
    /// <returns></returns>
    public static string CreateUpdateUrl_StudyCompass()
    {
        string url;
        url = BaseUrl_studyCompass + "update";
        return url;
    }

    #endregion


    /// <summary>
    /// ���s���𑗐M
    /// </summary>
    /// <returns></returns>
    public static string ResultSetUrl()
    {
        // ���ʑ��M�pAPI�����ɂ���ĈقȂ邽��URLType���Ƃɐݒ�(#74�Ή�)
        switch (GameInfo.URLType)
        {
            case URLType.Info:
                return infoResultUrl;
            case URLType.Develop:
                return developResultUrl;
            case URLType.StudyCompas:
                return productionResultUrl;
        }
        return null;
    }
}
