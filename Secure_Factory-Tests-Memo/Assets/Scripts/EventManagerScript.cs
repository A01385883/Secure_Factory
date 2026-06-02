using UnityEngine;

public class EventManagerScript : MonoBehaviour
{
    public bool paused = false;
    public GameObject componentsPanel;
    public GameObject helpPanel;

    void Start()
    {
        CloseHelp();
    }

    public void HelpButtonPressed()
    {
        paused = true;
        Time.timeScale = 0f;
        componentsPanel.SetActive(false);
        helpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        paused = false;
        Time.timeScale = 1f;
        helpPanel.SetActive(false);
        componentsPanel.SetActive(true);
    }
}