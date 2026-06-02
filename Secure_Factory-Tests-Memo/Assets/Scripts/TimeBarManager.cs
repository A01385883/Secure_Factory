using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeBarManager : MonoBehaviour
{
    public Slider timeBar;
    public float maxTime = 20f;

    private float currentTime;
    private MemoramaManager memoramaManager;

    void Start()
    {
        currentTime = maxTime;
        timeBar.maxValue = maxTime;
        timeBar.value = currentTime;

        memoramaManager = FindFirstObjectByType<MemoramaManager>();
    }

    void Update()
    {
        if (memoramaManager != null && memoramaManager.gameEnded)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime < 0)
            currentTime = 0;

        timeBar.value = currentTime;

        if (currentTime <= 0)
        {
             if (GameManager.Instance != null)
        GameManager.Instance.MinigameLost();
        }
    }
}