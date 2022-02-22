using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    private Image m_Image;

    public IEnumerator Cort_Start(float fadeInTime, float fadeOutTime)
    {
        gameObject.SetActive(true);

        m_Image = GetComponent<Image>();

        // 元の画像の色にしつつ透明に
        m_Image.color = new Color(1, 1, 1, 0);

        // 画面の上側にセット
        transform.localPosition = new Vector3(0, 300f, 0);

        float waitTime = 0f;
        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = Vector3.zero;
        Color startColor = m_Image.color;
        Color targetColor = Color.white;

        while(waitTime < fadeInTime)
        {
            // 徐々に下に移動
            transform.localPosition = Vector3.Lerp(startPos, targetPos, waitTime / fadeInTime);

            // 徐々に不透明にする
            m_Image.color = Color.Lerp(startColor, targetColor, waitTime / fadeInTime);

            waitTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        startColor = m_Image.color;
        targetColor = Color.clear;
        waitTime = 0f;

        while(waitTime < fadeOutTime)
        {
            // 徐々に透明にする
            m_Image.color = Color.Lerp(startColor, targetColor, waitTime / fadeInTime);

            waitTime += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
    
}
