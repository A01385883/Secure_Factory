using UnityEngine;
using System.Collections;

public class EventManagerAutoClose : MonoBehaviour
{
    public bool paused = false;
    public GameObject helpPanel;
    public GameObject buttonMash; // arrastra el boton a clickear
    public float tiempoAutoClose = 3f;

    void Start()
    {
        helpPanel.SetActive(true);
        if (buttonMash != null) buttonMash.SetActive(false);
        paused = true;
        StartCoroutine(AutoClose());
    }

    IEnumerator AutoClose()
    {
        yield return new WaitForSecondsRealtime(tiempoAutoClose);
        helpPanel.SetActive(false);
        if (buttonMash != null) buttonMash.SetActive(true);
        paused = false;
    }
}