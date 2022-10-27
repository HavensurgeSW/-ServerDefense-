using UnityEngine;

using TMPro;

public class CmdEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text entryText = null;

    public void ToggleStatus(bool status)
    {
        gameObject.SetActive(status);
    }

    public void SetText(string text)
    {
        entryText.text = text;
    }

    public void SetTextColor(Color color)
    {
        entryText.color = color;
    }

    public void SetSiblingIndex(int index)
    {
        transform.SetSiblingIndex(index);
    }
}
