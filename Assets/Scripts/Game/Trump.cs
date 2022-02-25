using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// トランプタイプ
/// </summary>
public enum TrumpType
{
    /// <summary>
    /// 何もなし
    /// </summary>
    None = -1,

    /// <summary>
    /// スペード
    /// </summary>
    S_1,
    S_2,
    S_3,
    S_4,
    S_5,
    S_6,
    S_7,
    S_8,
    S_9,
    S_10,
    S_11,
    S_12,
    S_13,

    /// <summary>
    /// クローバー
    /// </summary>
    C_1,
    C_2,
    C_3,
    C_4,
    C_5,
    C_6,
    C_7,
    C_8,
    C_9,
    C_10,
    C_11,
    C_12,
    C_13,

    /// <summary>
    /// ダイヤ
    /// </summary>
    D_1,
    D_2,
    D_3,
    D_4,
    D_5,
    D_6,
    D_7,
    D_8,
    D_9,
    D_10,
    D_11,
    D_12,
    D_13,

    /// <summary>
    /// ハート
    /// </summary>
    H_1,
    H_2,
    H_3,
    H_4,
    H_5,
    H_6,
    H_7,
    H_8,
    H_9,
    H_10,
    H_11,
    H_12,
    H_13,

    /// <summary>
    /// 裏面
    /// </summary>
    Back,

    Max,
}

/// <summary>
/// トランプの面
/// </summary>
public enum TrumpFace
{
    /// <summary>
    /// 何も無し
    /// </summary>
    None,

    /// <summary>
    /// 前面
    /// </summary>
    Front,

    /// <summary>
    /// 裏面
    /// </summary>
    Back,
}

public class Trump : MonoBehaviour
{
    [Header("マウスホバー時のマスク画像")]
    [SerializeField] private Image m_maskImage;

    [SerializeField] private TrumpSprite m_trumpSprite;

    [SerializeField] private Image m_image;

    [SerializeField] private EventTrigger m_eventTrigger;

    public EventTrigger TrumpEventTrigger {
        get { return m_eventTrigger; }
    }

    private TrumpFace m_face = TrumpFace.None;
    public TrumpFace Face {
        get { return m_face; }
    }

    private TrumpType m_type = TrumpType.S_1;
    public TrumpType Type {
        get { return m_type; }
        set { m_type = value; }
    }

    private int m_indexX = -1;
    public int IndexX { get { return m_indexX; } }


    private int m_indexY = -1;
    public int IndexY { get { return m_indexY; } }

    private int m_number = -1;
    public int Number {
        get { return m_number; }
        set { m_number = value; }
    }

    public void SetUp(int x, int y, int num, TrumpType trumpType)
    {
        m_eventTrigger.enabled = true;

        m_indexX = x;
        m_indexY = y;
        m_number = num;

        m_type = trumpType;

        FaceDown();
    }

    public void FaceDown()
    {
        if (m_face == TrumpFace.Back) { return; }
        SetState(TrumpFace.Back);
    }

    public void FaceUp()
    {
        if (m_face == TrumpFace.Front) { return; }
        SetState(TrumpFace.Front);
    }

    private void SetState(TrumpFace trumpFace)
    {
        m_face = trumpFace;

        switch (m_face)
        {
            case TrumpFace.Front:
                if(m_type == TrumpType.None) { return; }
                m_image.sprite = m_trumpSprite.TrumpSpriteList[(int)m_type];
                break;
            case TrumpFace.Back:
                m_image.sprite = m_trumpSprite.TrumpSpriteList[(int)TrumpType.Back];
                break;
        }
    }

    public IEnumerator Cort_DealMove(Vector2 targetPos, float moveTime, UnityAction callback)
    {
        float waitTime = 0f;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;
        Quaternion targetRot = Quaternion.Euler(0, 0, 180f);
        Quaternion targetRot2 = Quaternion.identity;

        while(waitTime < moveTime)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPos, waitTime / moveTime);
            

            if(waitTime < moveTime / 2f)
            {
                transform.localRotation = Quaternion.Lerp(startRot, targetRot, waitTime / moveTime);
            }
            else
            {
                transform.localRotation = Quaternion.Lerp(startRot, targetRot2, waitTime / moveTime);
            }


            waitTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPos;
        transform.localRotation = startRot;

        if(callback != null)
        {
            callback();
        }
    }

    public void Acquired(TrumpType trumpType)
    {
        m_eventTrigger.enabled = false;

        transform.localPosition = Vector3.zero;

        m_type = trumpType;

        FaceUp();
    }

    public IEnumerator Cort_FaceUp(float rotateTime)
    {
        if (m_face == TrumpFace.Front) { yield break; }

        m_maskImage.gameObject.SetActive(false);

        float waitTime = 0f;
        Quaternion startRot = transform.localRotation;
        Quaternion targetRot = Quaternion.Euler(0, 90f, 0);

        while(waitTime < rotateTime / 2)
        {
            transform.localRotation = Quaternion.Lerp(startRot, targetRot, waitTime / (rotateTime / 2));
            waitTime += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = targetRot;
        SetState(TrumpFace.Front);

        waitTime = 0f;
        startRot = transform.localRotation;
        targetRot = Quaternion.Euler(Vector3.zero);

        while (waitTime < rotateTime / 2)
        {
            transform.localRotation = Quaternion.Lerp(startRot, targetRot, waitTime / (rotateTime / 2));
            waitTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator Cort_FaceDown(float rotateTime, UnityAction callback)
    {
        if (m_face == TrumpFace.Back) { yield break; }

        float waitTime = 0f;
        Quaternion startRot = transform.localRotation;
        Quaternion targetRot = Quaternion.Euler(0, 90f, 0);

        while (waitTime < rotateTime / 2)
        {
            transform.localRotation = Quaternion.Lerp(startRot, targetRot, waitTime / (rotateTime / 2));
            waitTime += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = targetRot;
        SetState(TrumpFace.Back);

        waitTime = 0f;
        startRot = transform.localRotation;
        targetRot = Quaternion.Euler(Vector3.zero);

        while (waitTime < rotateTime / 2)
        {
            transform.localRotation = Quaternion.Lerp(startRot, targetRot, waitTime / (rotateTime / 2));
            waitTime += Time.deltaTime;
            yield return null;
        }

        if(callback != null)
        {
            callback();
        }
    }

    public IEnumerator Cort_Move(Vector2 targetPos, float moveTime, UnityAction callback)
    {
        float waitTime = 0f;
        Vector3 startPos = transform.localPosition;

        while (waitTime < moveTime)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPos, waitTime / moveTime);
            waitTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPos;

        if (callback != null)
        {
            callback();
        }
    }

    public void OnPointerEnter()
    {
        if(GameManager.Instance.CheckSelectTrump(this))
        {
            m_maskImage.gameObject.SetActive(true);
        }        
    }

    public void OnPointerDown()
    {
        GameManager.Instance.OnClick_SelectTrump(this);
    }

    public void OnPointerExit()
    {
        m_maskImage.gameObject.SetActive(false);
    }
}
