using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float packetSpeed = 2f;
    public float packetInterval = 3f;

    private bool gameOver = false;
    private int routersPlaced = 0;
    private float winTimer = 0f;
    private bool countingWin = false;

    //Añadido para el Menu de Pausa
    [SerializeField] GameObject pauseMenu;
    public static bool isPaused;

    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }
    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
         Time.timeScale = 1f;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (!isPaused)
            {
               PauseGame();
            }
            else
            {
                ResumeGame();
            }

        }
        if (countingWin)
        {
            winTimer += Time.deltaTime;
            if (winTimer >= 2f) EndGame(true);
        }
    }


    public void RouterPlaced()
    {
        routersPlaced++;
        if (routersPlaced >= 2)
        {
            countingWin = true;
        }
    }

    public void RouterRemoved()
    {
        routersPlaced = Mathf.Max(0, routersPlaced - 1);
    }

    public void MaliciousPacketReached()
    {
        if (gameOver) return;
        EndGame(false);
    }

    void EndGame(bool won)
    {
        gameOver = true;
        countingWin = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("puntuaje");
    }

    public bool IsGameOver() => gameOver;
}