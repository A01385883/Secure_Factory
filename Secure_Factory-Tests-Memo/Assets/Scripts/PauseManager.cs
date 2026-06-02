using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [SerializeField] GameObject pauseMenu;
    public static bool isPaused;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

void Start()
{
    // En lugar de desactivar todo el objeto, solo oculta el panel visualmente
    // Cambio hecho debido a que tambien desaparecia el boton para pausar
    CanvasGroup cg = pauseMenu.GetComponent<CanvasGroup>();
    if (cg == null) cg = pauseMenu.AddComponent<CanvasGroup>();
    cg.alpha = 0f;
    cg.interactable = false;
    cg.blocksRaycasts = false;
    isPaused = false;
}

    void Update()
    {
        // Se activa con Escape O con el botón 1 del teclado
        bool togglePressed = Keyboard.current.escapeKey.wasPressedThisFrame
                          || Keyboard.current.digit1Key.wasPressedThisFrame;

        if (togglePressed)
        {
            if (!isPaused) PauseGame();
            else ResumeGame();
        }
    }

    public void PauseGame()
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
}