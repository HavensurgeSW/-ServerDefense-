using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text serverHealth = null;  
    [SerializeField] private TMP_Text packetPointsText = null;

    [SerializeField] private GameObject popUpPrefab = null;  
    [SerializeField] private Transform popUpsHolder = null;  

    [SerializeField] private Server server = null;

    public static Action<bool> OnWaveEnd;
    [SerializeField] private Image timerBar;

    bool isTimerEnabled = false;
    float timeLeft = 0;
    float maxTime = 0;

    private ObjectPool<PopUp> popUpPool = null;

    public void Init(float f)
    {
        maxTime = f;
        timeLeft = maxTime;
        UpdateServerHealthText(server.HEALTH);

        popUpPool = new ObjectPool<PopUp>(CreatePopUp, GetPopUp, ReleasePopUp);
    }

    public void GeneratePopUp(string id, Vector3 position)
    {
        PopUp popUp = popUpPool.Get();
        popUp.SetPosition(position);
        popUp.SetText(id);
    }

    public void RemovePopUp(PopUp item)
    {
        item.Release();
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

    private PopUp CreatePopUp()
    {
        PopUp item = Instantiate(popUpPrefab, popUpsHolder).GetComponent<PopUp>();
        item.Init(popUpPool.Release);
        return item;
    }

    private void GetPopUp(PopUp item)
    {
        item.gameObject.SetActive(true);
    }

    private void ReleasePopUp(PopUp item)
    {
        item.gameObject.SetActive(false);
    }
}