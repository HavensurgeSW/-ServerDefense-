using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Towers/LevelData", fileName = "LevelData_")]
public class BaseTowerLevelData : ScriptableObject
{
    [SerializeField] int currentLevel = 0;
    [SerializeField] int price = 0;
    [SerializeField] int damage = 0;
    [SerializeField] float fireRate = 0;
    [SerializeField] float range = 1.0f;           
    [SerializeField] string[] targets = null;
    [SerializeField] int targetCount = 0;

    public int CURRENT_LEVEL { get => currentLevel;}
    public int PRICE { get => price; }
    public int DAMAGE { get => damage; }
    public float FIRE_RATE { get => fireRate; }
    public float RANGE { get => range; }
    public string[] TARGETS { get => targets; }
    public int TARGET_COUNT { get => targetCount; }
}
