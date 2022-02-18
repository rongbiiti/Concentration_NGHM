using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �g�����v�^�C�v
/// </summary>
public enum TrumpType
{
    /// <summary>
    /// �����Ȃ�
    /// </summary>
    None = -1,

    /// <summary>
    /// �X�y�[�h
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
    /// �N���[�o�[
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
    /// �_�C��
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
    /// �n�[�g
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
    /// ����
    /// </summary>
    Back,

    Max,
}

/// <summary>
/// �g�����v�̖�
/// </summary>
public enum TrumpFace
{
    /// <summary>
    /// ��������
    /// </summary>
    None,

    /// <summary>
    /// �O��
    /// </summary>
    Front,

    /// <summary>
    /// ����
    /// </summary>
    Back,
}

public class Trump : MonoBehaviour
{
    [Header("�}�E�X�z�o�[���̃}�X�N�摜")]
    [SerializeField] private Image m_maskImage;

    [SerializeField] private TrumpSprite m_trumpSprite;
}
