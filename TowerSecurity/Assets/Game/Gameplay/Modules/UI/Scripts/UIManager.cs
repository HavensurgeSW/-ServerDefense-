using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] Server server;
    [SerializeField] TMP_Text svrhp;
  

    private void Update()
    {
        svrhp.text = "Server Health: " + server.HEALTH.ToString();
    }


}
