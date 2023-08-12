using System;

using UnityEngine;
using UnityEngine.UI;

using ServerDefense.Tools.CursorHandling;

namespace ServerDefense.Gameplay.Gameplay.Modules.Pause
{
    public class PauseHandler : MonoBehaviour
    {
        [Header("Main Configuration")]
        [SerializeField] private CursorHandler cursorStatus = null;
        [SerializeField] private GameObject holder = null;
        [SerializeField] private Button resumeButton = null;
        [SerializeField] private Button menuButton = null;

        private bool isPaused = false;

        private Action<bool> OnPaused = null;

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

        public void AddOnPausedCallback(Action<bool> onPaused)
        {
            OnPaused += onPaused;
        }

        public void TogglePauseStatus(bool status)
        {
            isPaused = status;

            cursorStatus.ToggleMouseVisibility(status);
            holder.SetActive(status);

            if (isPaused)
            {
                Pause();
            }
            else
            {
                UnPause();
            }

            OnPaused?.Invoke(status);
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
}