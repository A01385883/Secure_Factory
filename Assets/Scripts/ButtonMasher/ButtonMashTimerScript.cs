using UnityEngine;
using UnityEngine.UI;

public class ButtonMashTimerScript : MonoBehaviour
{
    [SerializeField] float tiempoBase = 13f;
    public bool debugMode = false;
    public bool gameOver = false;
    public EventManagerAutoClose eventManagerScript;
    public RectTransform timerRect;
    private float timeLeft;
    private float tiempoTotal;
    private float anchoOriginal;

    void Start()
    {
        int nivel = GameManager.Instance.ObtenerNivelDificultad();
        int nivelesPares = nivel / 2;
        float reduccion = 1f - (nivelesPares * 0.07f);
        timeLeft = tiempoBase * reduccion;
        tiempoTotal = timeLeft;
        anchoOriginal = timerRect.rect.width;
        timerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, anchoOriginal);
    }

    void Update()
    {
        if (!debugMode && !gameOver && !eventManagerScript.paused)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                float progreso = timeLeft / tiempoTotal;
                timerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, anchoOriginal * progreso);
            }
            else
            {
                gameOver = true;
                GameManager.Instance.MinigameLost();
            }
        }
    }
}