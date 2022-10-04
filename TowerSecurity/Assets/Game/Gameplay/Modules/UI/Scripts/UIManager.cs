using UnityEngine;

using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text serverHealth = null;  
    [SerializeField] private TMP_Text packetPointsText = null;  

    public void Init()
    {

    }

    private void OnEnable()
    {
        Server.OnDamaged += UpdateServerHealthText;
    }

    private void OnDisable()
    {
        Server.OnDamaged -= UpdateServerHealthText;        
    }

    public void UpdatePacketPointsText(int points)
    {
        packetPointsText.text = "Packet Points: " + points.ToString();
    }

    private void UpdateServerHealthText(int health)
    {
        serverHealth.text = "Server Health: " + health.ToString();
    }
}
