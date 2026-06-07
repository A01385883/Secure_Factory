using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonesGameManager : MonoBehaviour
{
    public List<GameButton> buttons;
    private bool gameRunning = false;
    private int missCount = 0;
    private bool won = false;
    public float gameDuration = 12f;
    public int maxMisses = 3;
    private float spawnInterval;
    private float moleVisibleDuration;
    private int maxSimultaneousActive;

    void Start() => StartGame();

    void ApplyDifficulty()
    {
        int difficulty = GameManager.Instance.ObtenerNivelDificultad();
        switch (difficulty)
        {
            case 1: spawnInterval = 1.2f; moleVisibleDuration = 1.5f; maxSimultaneousActive = 1; break;
            case 2: spawnInterval = 1.0f; moleVisibleDuration = 1.2f; maxSimultaneousActive = 1; break;
            case 3: spawnInterval = 0.8f; moleVisibleDuration = 1.0f; maxSimultaneousActive = 2; break;
            case 4: spawnInterval = 0.6f; moleVisibleDuration = 0.8f; maxSimultaneousActive = 2; break;
            case 5: spawnInterval = 0.4f; moleVisibleDuration = 0.6f; maxSimultaneousActive = 3; break;
        }
    }

    void StartGame()
    {
        ApplyDifficulty();
        missCount = 0;
        won = false;
        gameRunning = true;
        StartCoroutine(SpawnLoop());
        StartCoroutine(GameTimer());
    }

    IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(gameDuration);
        EndGame(won: true);
    }

    IEnumerator SpawnLoop()
    {
        while (gameRunning)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (!gameRunning) break;
            List<GameButton> inactive = buttons.FindAll(b => !b.IsActive);
            int activeCount = buttons.Count - inactive.Count;
            if (inactive.Count > 0 && activeCount < maxSimultaneousActive)
            {
                GameButton chosen = inactive[Random.Range(0, inactive.Count)];
                chosen.Activate();
                StartCoroutine(AutoDeactivate(chosen));
            }
        }
    }

IEnumerator AutoDeactivate(GameButton button)
{
    yield return new WaitForSeconds(moleVisibleDuration);
    if (button.IsActive)
    {
        button.Deactivate();
        RegisterMiss();
    }
}

public void RegisterHit() { }

public void RegisterMiss()
{
    missCount++;
    if (missCount >= maxMisses)
        EndGame(won: false);
}
    void EndGame(bool won)
    {
        gameRunning = false;
        this.won = won;
        foreach (var b in buttons) b.Deactivate();

        if (won)
            GameManager.Instance.MinigameWon();
        else
            GameManager.Instance.MinigameLost();
    }

    
}