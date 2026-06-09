using UnityEngine;
using UnityEngine.InputSystem;

public class BucketManager : MonoBehaviour
{
    public bool gameOver = false;
    public bool debugMode = false;
    public bool paused = false;
    public int maxMisses = 5;
    private int missCount = 0;

    public void RegisterMiss()
    {
        if (gameOver || debugMode) return;
        missCount++;
        if (missCount >= maxMisses)
        {
            gameOver = true;
            GameManager.Instance.MinigameLost();
        }
    }

    public void RegisterWin()
    {
        if (gameOver) return;
        gameOver = true;
        GameManager.Instance.MinigameWon();
    }

    void Update()
    {
        if (debugMode && Keyboard.current.rKey.wasPressedThisFrame)
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}