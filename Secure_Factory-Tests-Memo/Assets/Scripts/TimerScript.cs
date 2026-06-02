using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float timer = 60f;
    public bool debugMode = false;
    public bool gameOver = false;
    private EventManagerScript eventManagerScript;
    public Slider timerSlider;
    private float timeLeft;

    void Start()
    {
        eventManagerScript = FindAnyObjectByType<EventManagerScript>();
        timeLeft = timer;
        timerSlider.minValue = 0;
        timerSlider.maxValue = timeLeft;
        timerSlider.value = timeLeft;
    }

    void Update()
    {
        if (!debugMode && !gameOver && !eventManagerScript.paused)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                timerSlider.value = timeLeft;
            }
            else
            {
    gameOver = true;
    GameManager.Instance.MinigameWon();
            }
        }
    }
}