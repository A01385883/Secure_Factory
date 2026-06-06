using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ButtonMashScript : MonoBehaviour
{
    [SerializeField] Sprite[] stageSprites;
    [SerializeField] UnityEngine.UI.Image backgroundImage;
    public EventManagerAutoClose eventManagerScript;
    public ButtonMashTimerScript timerScript;

    private int clicksPerStage = 9;
    private int totalStages = 6;
    private int currentClicks = 0;
    private int currentStage = 0;
    private bool finished = false;

    void Start()
    {
        int nivel = GameManager.Instance.ObtenerNivelDificultad();
        int nivelesImpares = (nivel + 1) / 2;
        clicksPerStage = 9 + ((nivelesImpares - 1) * 4);
        CambiarStage(0);
    }

    void Update()
    {
        if (finished || timerScript.gameOver || eventManagerScript.paused) return;
        if (Mouse.current.leftButton.wasPressedThisFrame)
            RegisterClick();
    }

    void RegisterClick()
    {
        currentClicks++;
        if (currentClicks >= clicksPerStage)
        {
            currentClicks = 0;
            currentStage++;
            CambiarStage(currentStage);

            if (currentStage >= totalStages - 1)
            {
                finished = true;
                timerScript.gameOver = true;
                StartCoroutine(WinDelay());
            }
        }
    }

    IEnumerator WinDelay()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.MinigameWon();
    }

    void CambiarStage(int index)
    {
        if (index < stageSprites.Length)
            backgroundImage.sprite = stageSprites[index];
    }
}