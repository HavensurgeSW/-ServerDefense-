using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text serverHealth = null;  
    [SerializeField] private TMP_Text packetPointsText = null;

    [SerializeField]Server server;

    public static Action<bool> OnWaveEnd;
    [SerializeField] private Image timerBar;
    bool isTimerEnabled = false;
    float timeLeft = 0;
    float maxTime = 0;

    public void Init(float f)
    {
        maxTime = f;
        timeLeft = maxTime;
        UpdateServerHealthText(server.HEALTH);
    }

    private void OnEnable()
    {
        Server.OnDamaged += UpdateServerHealthText;
        OnWaveEnd += ToggleTimer;
    }

    private void OnDisable()
    {
        Server.OnDamaged -= UpdateServerHealthText;
        OnWaveEnd -= ToggleTimer;
    }

    public void UpdatePacketPointsText(int points)
    {
        packetPointsText.text = "Current KB: " + points.ToString();
    }

    private void UpdateServerHealthText(int health)
    {
        serverHealth.text = "Server Health: " + health.ToString();
    }

    private void Update()
    {
        if (isTimerEnabled) {
            if (timeLeft > 0) 
            {
                timeLeft -= Time.deltaTime;
                timerBar.fillAmount = timeLeft / maxTime;

                if (timeLeft <= 0)
                {
                    timeLeft = maxTime;
                    isTimerEnabled = false;
                }
            }
        }
    }

    private void ToggleTimer(bool b)
    {
        isTimerEnabled = b;
        timeLeft = maxTime;
    }


}
