using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUtilities : MonoBehaviour
{
    public void StartButton() {
        SceneManager.LoadScene(2);
    }
    public void ExitButton()
    {  
#if !UNITY_EDITOR
            Application.Quit();
#else
            UnityEditor.EditorApplication.isPlaying = false;
#endif    
    }
}
