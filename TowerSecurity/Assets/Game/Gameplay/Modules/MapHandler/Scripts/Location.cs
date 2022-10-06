using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    bool isSelected;
    public SpriteRenderer sr;
    [SerializeField] private string id;
    [SerializeField] private bool availability;

    public string ID => id;

    private void Start()
    {
        isSelected = false;
        availability = true;
    }

    public void ToggleSelected(bool b) {
        isSelected = b;
    }

    public void ToggleColor(Color clr) {
        sr.color = clr;
    }

    public bool GetAvailability() {
        return availability;
    }

    public void SetAvailable(bool b) {
        availability = b;   
    }

}
