using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStatus : MonoBehaviour
{
    [SerializeField] bool showCursor;
    void Awake()
    {
        Cursor.visible = showCursor;

        if(!showCursor)
            Cursor.lockState = CursorLockMode.Locked;
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
