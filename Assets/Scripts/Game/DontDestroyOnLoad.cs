using UnityEngine;

/// <summary>
/// �V�[���J�ڌ���j������Ȃ��I�u�W�F�N�g
/// </summary>
/// <remarks>
/// ���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g��e�I�u�W�F�N�g�Ƃ��āA
/// �V�[���J�ڌ���g�������I�u�W�F�N�g���q�v�f�Ƃ��Ēǉ����Ă����B
/// </remarks>
public class DontDestroyOnLoad : MonoBehaviour
{
    /// <summary>
    /// �J�n���ɌĂ΂��
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}