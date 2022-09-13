using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public float MAXHEALTH { get; set; }
    [SerializeField]private Image healthBar;

    public void SetMaxHP(int health) {
        MAXHEALTH = health;
    }
    public void SetHealthbarFill(int health){
        Debug.Log(health / MAXHEALTH);
        healthBar.fillAmount = health / MAXHEALTH;
    }

}
