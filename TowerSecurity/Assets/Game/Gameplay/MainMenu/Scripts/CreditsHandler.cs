using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CreditsHandler : MonoBehaviour
{
    [SerializeField] private GameObject holder = null;
    [SerializeField] private Button exitButton = null;

    public void Init(Action onSwitchToMenu)
    {
        exitButton.onClick.AddListener(() => onSwitchToMenu?.Invoke());
    }

    public void ToggleStatus(bool status)
    {
        holder.SetActive(status);
    }
}
