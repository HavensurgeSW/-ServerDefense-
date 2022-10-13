using UnityEngine;

public class Location : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private string id;
    [SerializeField] private bool availability;

    private BaseTower tower = null;
    private bool isSelected = false;

    public string ID { get => id; }
    public BaseTower TOWER { get => tower; }

    private void Start()
    {
        isSelected = false;
        availability = true;
    }

    public void SetTower(BaseTower tower)
    {
        this.tower = tower;
    }

    public void ToggleSelected(bool b)
    {
        isSelected = b;
    }

    public void ToggleColor(Color clr)
    {
        sr.color = clr;
    }

    public bool GetAvailability()
    {
        return availability;
    }

    public void SetAvailable(bool b)
    {
        availability = b;
    }
}