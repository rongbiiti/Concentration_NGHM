using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : UWRHelper
{
    public void GotoMatchMaking()
    {
        StartCoroutine(nameof(IDInit));
    }

    /// <summary>
    /// ルームIDとユーザーID初期化、その後マッチメイクシーンに移動
    /// </summary>
    /// <returns></returns>
    private IEnumerator IDInit()
    {
        

        yield return Cort_GetGameKey();

        yield return Cort_GetUserID();

        FadeManager.Instance.LoadScene(1, 0.25f);
    }

    /// <summary>
    /// GameKey取得（ルームID）
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cort_GetGameKey()
    {
        // WebGLで実行時、GameKey取得
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if(GetClieParameters.m_GameKey != null)
            {
                KeyData.GameKey = GetClieParameters.m_GameKey;
            }
        }

        

        yield break;
    }

    /// <summary>
    /// ユーザーIDの取得
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cort_GetUserID()
    {
        // WebGLで実行時、GameKey取得
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (GetClieParameters.m_UserId != null)
            {
                GameInfo.MyUserID = GetClieParameters.m_UserId;
            }
        }

        yield break;
    }
}
