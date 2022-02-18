using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// ���b�Z�[�W�̃^�C�v
/// </summary>
public enum MessageType
{
    YesNo,
    Ok,
    MessageOnly,
};

/// <summary>
/// ���b�Z�[�W�{�b�N�X�N���X
/// </summary>
public class MessageBox : MonoBehaviour
{
    /// <summary>
    /// Yes�{�^��
    /// </summary>
    [SerializeField]
    private Button _yesButton = default;

    /// <summary>
    /// No�{�^��
    /// </summary>
    [SerializeField]
    private Button _noButton = default;

    /// <summary>
    /// OK�{�^��
    /// </summary>
    [SerializeField]
    private Button _okButton = default;

    /// <summary>
    /// ���e�L�X�g
    /// </summary>
    [SerializeField]
    private Text _subjectText = default;

    /// <summary>
    /// ���b�Z�[�W�e�L�X�g
    /// </summary>
    [SerializeField]
    private Text _messageText = default;

    /// <summary>
    /// �uYesButton�v�������A���s�����C�x���g
    /// </summary>
    private UnityAction _yesEvent = default;

    /// <summary>
    /// �uNoButton�v�������A���s�����C�x���g
    /// </summary>
    private UnityAction _noEvent = default;

    /// <summary>
    /// [OkButton]�������A���s�����C�x���g
    /// </summary>
    private UnityAction _okEvent = default;

    /// <summary>
    /// �{�^����������Ă��邩�H
    /// </summary>
    /// <remarks>
    /// TRUE:  ������Ă���
    /// FALSE: ������Ă��Ȃ�
    /// </remarks>
    private bool _isPushedButton = false;

    /// <summary>
    /// �I�u�W�F�N�g�\����
    /// </summary>
    private void OnEnable()
    {
        _yesButton.onClick.AddListener(() => OnClick_YesButton());
        _noButton.onClick.AddListener(() => OnClick_NoButton());
        _okButton.onClick.AddListener(() => OnClick_OkButton());
    }

    /// <summary>
    /// ���b�Z�[�W�������@Yes/No�{�^���\��
    /// </summary>
    /// <param name="subject">���</param>
    /// <param name="message">���b�Z�[�W</param>
    /// <param name="yesEvent">Yes�{�^���������A���s����郁�\�b�h</param>
    /// <param name="noEvent">No�{�^���������A���s����郁�\�b�h</param>
    public void Initialize_YesNo(string subject, string message, UnityAction yesEvent, UnityAction noEvent)
    {
        _subjectText.text = subject;
        _messageText.text = message;
        _yesEvent = yesEvent;
        _noEvent = noEvent;
        SetMessageType(MessageType.YesNo);
    }

    /// <summary>
    /// ���b�Z�[�W�������@Yes/No�{�^���\�� (��薳��)
    /// </summary>
    /// <param name="subject">���</param>
    /// <param name="message">���b�Z�[�W</param>
    /// <param name="yesEvent">Yes�{�^���������A���s����郁�\�b�h</param>
    /// <param name="noEvent">No�{�^���������A���s����郁�\�b�h</param>
    public void Initialize_YesNo(string message, UnityAction yesEvent, UnityAction noEvent)
    {
        _subjectText.text = string.Empty;
        _messageText.text = message;
        _yesEvent = yesEvent;
        _noEvent = noEvent;
        SetMessageType(MessageType.YesNo);
    }

    /// <summary>
    /// ���b�Z�[�W������ Ok�{�^���\��
    /// </summary>
    /// <param name="subject">���</param>
    /// <param name="message">���b�Z�[�W</param>
    /// <param name="okEvent">Ok�{�^���������A���s����郁�\�b�h</param>
    public void Initialize_Ok(string subject, string message, UnityAction okEvent)
    {
        _subjectText.text = subject;
        _messageText.text = message;
        _okEvent = okEvent;
        SetMessageType(MessageType.Ok);
    }

    /// <summary>
    /// ���b�Z�[�W������ Ok�{�^���\���i��薳���j
    /// </summary>
    /// <param name="message">���b�Z�[�W</param>
    /// <param name="okEvent">Ok�{�^���������A���s����郁�\�b�h</param>
    public void Initialize_Ok(string message, UnityAction okEvent)
    {
        _subjectText.text = string.Empty;
        _messageText.text = message;
        _okEvent = okEvent;
        SetMessageType(MessageType.Ok);
    }

    /// <summary>
    /// ���b�Z�[�W������ ���b�Z�[�W�̂ݕ\��
    /// </summary>
    /// <param name="subject">���</param>
    /// <param name="message">���b�Z�[�W</param>
    ///     /// <param name="destroyTime">���b�Z�[�W���폜���鎞��</param>
    public void Initialize_MessageOnly(string subject, string message, float destroyTime)
    {
        _subjectText.text = subject;
        _messageText.text = message;
        SetMessageType(MessageType.MessageOnly);
        StartCoroutine(CloseWindow(destroyTime));
    }

    /// <summary>
    /// ���b�Z�[�W������ ���b�Z�[�W�̂ݕ\���i��薳���j
    /// </summary>
    /// <param name="message">���b�Z�[�W</param>
    /// <param name="destroyTime">���b�Z�[�W���폜���鎞��</param>
    public void Initialize_MessageOnly(string message, float destroyTime)
    {
        _subjectText.text = string.Empty;
        _messageText.text = message;
        SetMessageType(MessageType.MessageOnly);
        StartCoroutine(CloseWindow(destroyTime));
    }

    /// <summary>
    /// ���b�Z�[�W������ ���b�Z�[�W�̂ݕ\���i��薳���A�j�������j
    /// </summary>
    /// <param name="message">���b�Z�[�W</param>
    public void Initialize_MessageOnly(string message)
    {
        _subjectText.text = string.Empty;
        _messageText.text = message;
        SetMessageType(MessageType.MessageOnly);
    }

    /// <summary>
    /// ���b�Z�[�W������ ���b�Z�[�W�̂ݕ\���i�j�������j
    /// </summary>
    /// <param name="message">���b�Z�[�W</param>
    public void Initialize_MessageOnly(string subject, string message)
    {
        _subjectText.text = subject;
        _messageText.text = message;
        SetMessageType(MessageType.MessageOnly);
    }

    /// <summary>
    /// ���b�Z�[�W�̃^�C�v��ݒ�
    /// </summary>
    /// <remarks>
    /// �{�^���̕\��/��\�����s��
    /// </remarks>
    /// <param name="type"></param>
    private void SetMessageType(MessageType type)
    {
        switch (type)
        {
            case MessageType.MessageOnly:
                _yesButton.gameObject.SetActive(false);
                _noButton.gameObject.SetActive(false);
                _okButton.gameObject.SetActive(false);
                break;
            case MessageType.Ok:
                _yesButton.gameObject.SetActive(false);
                _noButton.gameObject.SetActive(false);
                _okButton.gameObject.SetActive(true);
                break;
            case MessageType.YesNo:
                _yesButton.gameObject.SetActive(true);
                _noButton.gameObject.SetActive(true);
                _okButton.gameObject.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// �uYesButton�v�������A���s���郁�\�b�h
    /// </summary>
    public void OnClick_YesButton()
    {
        if (_isPushedButton) { return; }
        _isPushedButton = true;

        if (_yesEvent != null)
        {
            _yesEvent.Invoke();
        }

        //AudioManager.I.PlaySe(AudioKey.ButtonSE);
        StartCoroutine(CloseWindow(0.1f));
    }

    /// <summary>
    /// �uNoButton�v�������A���s���郁�\�b�h
    /// </summary>
    public void OnClick_NoButton()
    {
        if (_isPushedButton) { return; }
        _isPushedButton = true;

        if (_noEvent != null)
        {
            _noEvent.Invoke();
        }

        //AudioManager.I.PlaySe(AudioKey.ButtonSE);
        StartCoroutine(CloseWindow(0.1f));
    }

    /// <summary>
    /// �uOkButton�v�������A���s���郁�\�b�h
    /// </summary>
    public void OnClick_OkButton()
    {
        if (_isPushedButton) { return; }
        _isPushedButton = true;

        if (_okEvent != null)
        {
            _okEvent.Invoke();
        }

        //AudioManager.I.PlaySe(AudioKey.ButtonSE);
        StartCoroutine(CloseWindow(0.1f));
    }

    /// <summary>
    /// ���b�Z�[�W�����
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator CloseWindow(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        Destroy(this.gameObject);
    }
}