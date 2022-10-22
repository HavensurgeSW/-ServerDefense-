using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class PopUp : MonoBehaviour
{
    [SerializeField] private Image background = null;
    [SerializeField] private TMP_Text text = null;

    private Action<PopUp> OnRelease = null;

    public void Init(Action<PopUp> onRelease)
    {
        OnRelease = onRelease;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public void Release()
    {
        OnRelease?.Invoke(this);
    }
}
