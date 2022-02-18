using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// シーン遷移時のフェードイン・アウトを制御するためのクラス .
/// </summary>
public class FadeManager : SingletonMonoBehaviour<FadeManager>
{

    

    /// <summary>フェード中の透明度</summary>
    private float fadeAlpha = 0;

    /// <summary>フェード中かどうか</summary>
    private bool isFading = false;

    /// <summary>フェード色</summary>
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
            //色と透明度を更新して白テクスチャを描画
            fadeColor.a = fadeAlpha;
            GUI.color = fadeColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }

    }

    /// <summary>
    /// 画面遷移
    /// </summary>
    /// <param name='scene'>シーンインデックス</param>
    /// <param name='interval'>暗転にかかる時間(秒)</param>
    public void LoadScene(int scene, float interval)
    {
        if (fadeFlg) return;
        StartCoroutine(TransScene(scene, interval));
        fadeFlg = true;
    }

    /// <summary>
    /// シーン遷移用コルーチン
    /// </summary>
    /// <param name='scene'>シーンインデックス</param>
    /// <param name='interval'>暗転にかかる時間(秒)</param>
    private IEnumerator TransScene(int scene, float interval)
    {
        //だんだん暗く
        isFading = true;
        float time = 0;
        while (time <= interval)
        {
            fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.unscaledDeltaTime;
            yield return 0;
        }

        Time.timeScale = 1f;

        //シーン切替
        SceneManager.LoadScene(scene);

        //一気に明るく
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