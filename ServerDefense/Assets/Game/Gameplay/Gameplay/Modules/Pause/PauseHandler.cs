using System;

using UnityEngine;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour
{
    [Header("Main Configuration")]
    [SerializeField] private GameObject holder = null;
    [SerializeField] private Button resumeButton = null;
    [SerializeField] private Button menuButton = null;

    [SerializeField] private bool isPaused = false;
    public bool IS_PAUSED {get => isPaused;}

    public void TogglePauseStatus(bool status)
    {
        holder.SetActive(status);
    }
}
