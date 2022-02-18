using UnityEngine;
using UnityEngine.SceneManagement;

public class InitSceneLoad
{
    /// <summary>
    /// �Q�[���N�����A�������p�V�[�������[�h����
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadScene()
    {
        if(!SceneManager.GetSceneByName("Scene00_Init").IsValid())
        {
            SceneManager.LoadScene("Scene00_Init", LoadSceneMode.Additive);
        }
    }
}