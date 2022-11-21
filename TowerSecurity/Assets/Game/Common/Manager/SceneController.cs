using UnityEngine;
using UnityEngine.SceneManagement;

using static CommonUtils;

public abstract class SceneController : MonoBehaviour
{
    protected abstract void Awake();
    protected abstract void OnEnable();
    protected abstract void OnDisable();

    protected void ChangeScene(SCENE scene)
    {
        SceneManager.LoadScene((int)scene);
    }

    protected void QuitApplication()
    {
#if !UNITY_EDITOR
        Application.Quit();
#else
        UnityEditor.EditorApplication.isPlaying = false;
#endif    
    }
}
