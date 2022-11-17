using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HDRSwitcher : MonoBehaviour
{
    [SerializeField] GameObject go;
    [SerializeField] Material mtl;
    [SerializeField] Color mtlColor;

    private void Start()
    {
        mtl = go.GetComponent<Material>();
    }

    public void UpdateTowerColor(int currentLevel) {
        mtl.color = mtlColor;
    }

  

}
