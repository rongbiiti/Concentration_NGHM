using UnityEngine;
using UnityEngine.SceneManagement;

public class InitSceneLoad
{
    /// <summary>
    /// ゲーム起動時、初期化用シーンをロードする
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