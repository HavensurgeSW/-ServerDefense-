using System;

using UnityEngine;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour
{
    [Header("Main Configuration")]
    [SerializeField] private CursorHandler cursorStatus = null;
    [SerializeField] private GameObject holder = null;
    [SerializeField] private Button resumeButton = null;
    [SerializeField] private Button menuButton = null;

    [SerializeField] private bool isPaused = false;

    public bool IS_PAUSED { get => isPaused; }

    public void Init(Action onMenuButtonClicked)
    {
        resumeButton.onClick.AddListener(() => TogglePauseStatus(false));
        menuButton.onClick.AddListener(() =>
            {
                TogglePauseStatus(false);
                onMenuButtonClicked?.Invoke();
            });
    }

    public void TogglePauseStatus(bool status)
    {
        isPaused = status;
        
        cursorStatus.ToggleMouse(status);
        holder.SetActive(status);

        if (isPaused)
        {
            Pause();
        }
        else
        {
            UnPause();
        }
    }

    private void Pause()
    {
        Time.timeScale = 0.0f;
    }

    private void UnPause()
    {
        Time.timeScale = 1.0f;
    }
}
