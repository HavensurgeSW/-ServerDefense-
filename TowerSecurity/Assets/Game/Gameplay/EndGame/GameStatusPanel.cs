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
            statusText.text = "Operation succesful";
            statusText.color = Color.green;
            subStatusText.text = "This network is now secure";
        }
        else
        {
            statusText.text = "Operation failure";
            statusText.color = Color.red;
            subStatusText.text = "The network is compromised";
        }
    }
}