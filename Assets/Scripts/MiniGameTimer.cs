using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGameTimer : MonoBehaviour
{
    public Slider timeSlider;

    public float maxTime = 12f;

    float currentTime;

    bool gameEnded = false;
    
    public Slider riskBar;
    public int currentRisk = 0;
    public int maxRisk = 100;

    public GameObject endGame;
    public TMP_Text endGameText;

    public CardSpawner cardSpawner;

    void Start()
    {
        currentTime = maxTime;

        timeSlider.maxValue = maxTime;
        timeSlider.value = maxTime;

        riskBar.maxValue = maxRisk;
        riskBar.value = currentRisk;

        endGame.SetActive(false);
    }

    void Update()
    {
        if(gameEnded)
            return;

        currentTime -= Time.deltaTime;

        timeSlider.value = currentTime;

        if(currentTime <= 0)
        {
            currentTime = 0;
            gameEnded = true;

            WinGame();
        }
    }

    public void AddRisk(int amount)
    {
        currentRisk += amount;
        currentRisk = Mathf.Clamp(currentRisk, 0, maxRisk);

        riskBar.value = currentRisk;

        if (currentRisk >= maxRisk)
        {
            LoseGame();
        }
    }

    void WinGame()
    {
        gameEnded = true;

        cardSpawner.StopSpawning();

        CardMovement[] cards = FindObjectsByType<CardMovement>(FindObjectsSortMode.None);

        foreach (CardMovement card in cards)
        {
            Destroy(card.gameObject);
        }

        endGame.SetActive(true);
        endGameText.text = "OT NETWORK SECURED!!!";
        endGameText.color = Color.green;
    }

    void LoseGame()
    {
        gameEnded = true;

        cardSpawner.StopSpawning();

        CardMovement[] cards = FindObjectsByType<CardMovement>(FindObjectsSortMode.None);

        foreach (CardMovement card in cards)
        {
            Destroy(card.gameObject);
        }

        endGame.SetActive(true);
        endGameText.text = "OT NETWORK UNSECURE! :(";
        endGameText.color = Color.red;
    }
}