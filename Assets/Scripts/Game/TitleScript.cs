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
    /// ���[��ID�ƃ��[�U�[ID�������A���̌�}�b�`���C�N�V�[���Ɉړ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator IDInit()
    {
        

        yield return Cort_GetGameKey();

        yield return Cort_GetUserID();

        FadeManager.Instance.LoadScene(1, 0.25f);
    }

    /// <summary>
    /// GameKey�擾�i���[��ID�j
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cort_GetGameKey()
    {
        // WebGL�Ŏ��s���AGameKey�擾
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
    /// ���[�U�[ID�̎擾
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cort_GetUserID()
    {
        // WebGL�Ŏ��s���AGameKey�擾
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
