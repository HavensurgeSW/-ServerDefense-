using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

using TMPro;

public class UIManager : MonoBehaviour
{
    public static Action<bool> OnWaveEnd = null;

    [Header("Server Configuration")]
    [SerializeField] private Server server = null;
    [SerializeField] private TMP_Text serverHealth = null;  
    [SerializeField] private TMP_Text packetPointsText = null;

    [Header("Popups Configuration")]
    [SerializeField] private GameObject popUpPrefab = null;  
    [SerializeField] private Vector2 offset = Vector2.zero;  
    [SerializeField] private Transform popUpsHolder = null;  

    [SerializeField] private Image timerBar = null;

    private bool isTimerEnabled = false;
    private float timeLeft = 0;
    private float maxTime = 0;

    private ObjectPool<PopUp> popUpPool = null;
    private List<PopUp> activePopUps = null;

    private Action OnTimerEnd = null;

    public void Init(float f)
    {
        maxTime = f;
        timeLeft = maxTime;
        UpdateServerHealthText(server.HEALTH);

        activePopUps = new List<PopUp>();
        popUpPool = new ObjectPool<PopUp>(CreatePopUp, GetPopUp, ReleasePopUp);
    }

    public void GeneratePopUp(string id, Vector3 position)
    {
        PopUp popUp = popUpPool.Get();
        activePopUps.Add(popUp);
        popUp.SetPosition(position + (Vector3)offset);
        popUp.SetLocationIdText(id);
        popUp.ToggleTowerDataText(false);
    }

    public void ClearAllPopUps()
    {
        for (int i = 0; i < activePopUps.Count; i++)
        {
            popUpPool.Release(activePopUps[i]);
        }

        activePopUps.Clear();
    }

    public void RemovePopUp(PopUp item)
    {
        item.Release();

        if (activePopUps.Contains(item))
        {
            activePopUps.Remove(item);
        }
    }

    public void AddOnTimerEndCallback(Action callback) 
    {
        OnTimerEnd += callback;
    }

    public void RemoveOnTimerEndCallback(Action callback)
    {
        OnTimerEnd -= callback;
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
        if (!isTimerEnabled)
        {
            return;
        }

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;

            if (timeLeft <= 0)
            {
                timeLeft = maxTime;
                isTimerEnabled = false;
                OnTimerEnd?.Invoke();
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