using UnityEngine;

using TMPro;

public class GameStatusPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text statusText = null;
    [SerializeField] private TMP_Text subStatusText = null;

    public void Init(bool status)
    {
        if (status)
        {
            statusText.text = "You won";
            statusText.color = Color.green;
            subStatusText.text = "Good stuff king";
        }
        else
        {
            statusText.text = "You lost";
            statusText.color = Color.red;
            subStatusText.text = "Git gud";
        }
    }
}