using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    [SerializeField] private GameObject cameraListener;
    void Awake()
    {
        //AkSoundEngine.PostEvent("Play_AX_Menu",cameraListener);
    }

}
