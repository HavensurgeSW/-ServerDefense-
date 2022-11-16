using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HDRSwitcher : MonoBehaviour
{
    [SerializeField] Material mtl;
    [SerializeField] Color[] mtlColor;

    public void UpdateTowerColor() {
        mtl.color = mtlColor[0];
        Debug.Log("Switching colors!");
    }
   
}
