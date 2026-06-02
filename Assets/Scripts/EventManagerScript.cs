using UnityEngine;

public class EventManagerScript : MonoBehaviour
{
    public bool paused = false;
    private GameObject helpPanel;
    private GameObject helpButtonPanel;

    void Start()
    {
        helpPanel = GameObject.FindWithTag("Help Panel");
        helpButtonPanel = GameObject.FindWithTag("Help Button Panel");
        HelpButtonPressed();
    }

    public void HelpButtonPressed()
    {
        paused = true;
        Time.timeScale = 0f;
        helpButtonPanel.SetActive(false);
        helpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        paused = false;
        Time.timeScale = 1f;
        helpPanel.SetActive(false);
        helpButtonPanel.SetActive(true);
    }
}