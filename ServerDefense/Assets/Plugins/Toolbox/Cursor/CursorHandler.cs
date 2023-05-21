using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private bool showCursor = false;
    [SerializeField] private bool configureOnAwake = true;
    
    private void Awake()
    {
        if (configureOnAwake)
        {
            ToggleMouseVisibility(showCursor);
        }
    }

    public void ToggleMouseVisibility(bool status)
    {
        Cursor.visible = status;

        if (status)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
