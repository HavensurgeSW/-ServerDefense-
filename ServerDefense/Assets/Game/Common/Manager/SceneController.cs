using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SceneController : MonoBehaviour
{
    protected abstract void Awake();
    protected abstract void OnEnable();
    protected abstract void OnDisable();

    protected virtual void ChangeScene(SCENE scene, bool async)
    {
        if (async)
        {
            SceneManager.LoadSceneAsync((int)scene);
        }
        else
        {
            SceneManager.LoadScene((int)scene);
        }
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