using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// �V�[���J�ڎ��̃t�F�[�h�C���E�A�E�g�𐧌䂷�邽�߂̃N���X .
/// </summary>
public class FadeManager : SingletonMonoBehaviour<FadeManager>
{

    

    /// <summary>�t�F�[�h���̓����x</summary>
    private float fadeAlpha = 0;

    /// <summary>�t�F�[�h�����ǂ���</summary>
    private bool isFading = false;

    /// <summary>�t�F�[�h�F</summary>
    public Color fadeColor = Color.black;

    private bool fadeFlg;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    public void OnGUI()
    {

        // Fade
        if (isFading)
        {
            //�F�Ɠ����x���X�V���Ĕ��e�N�X�`����`��
            fadeColor.a = fadeAlpha;
            GUI.color = fadeColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }

    }

    /// <summary>
    /// ��ʑJ��
    /// </summary>
    /// <param name='scene'>�V�[���C���f�b�N�X</param>
    /// <param name='interval'>�Ó]�ɂ����鎞��(�b)</param>
    public void LoadScene(int scene, float interval)
    {
        if (fadeFlg) return;
        StartCoroutine(TransScene(scene, interval));
        fadeFlg = true;
    }

    /// <summary>
    /// �V�[���J�ڗp�R���[�`��
    /// </summary>
    /// <param name='scene'>�V�[���C���f�b�N�X</param>
    /// <param name='interval'>�Ó]�ɂ����鎞��(�b)</param>
    private IEnumerator TransScene(int scene, float interval)
    {
        //���񂾂�Â�
        isFading = true;
        float time = 0;
        while (time <= interval)
        {
            fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.unscaledDeltaTime;
            yield return 0;
        }

        Time.timeScale = 1f;

        //�V�[���ؑ�
        SceneManager.LoadScene(scene);

        //��C�ɖ��邭
        time = 0;
        while (time <= interval)
        {
            fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            time += Time.unscaledDeltaTime;
            yield return 0;
        }

        isFading = false;
        fadeFlg = false;
    }
}