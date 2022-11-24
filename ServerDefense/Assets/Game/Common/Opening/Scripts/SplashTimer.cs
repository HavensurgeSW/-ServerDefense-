using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashTimer : MonoBehaviour
{
    public float waitTime = 9f;
    void Start()
    {
        StartCoroutine(WaitForSplash());
    }

    IEnumerator WaitForSplash() {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(1);
    }
}
