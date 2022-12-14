using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private bool showCursor = false;
    
    private void Awake()
    {
        ToggleMouse(showCursor);
    }

    public void ToggleMouse(bool status)
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
