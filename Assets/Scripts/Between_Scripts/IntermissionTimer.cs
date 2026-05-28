using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class IntermissionTimer : MonoBehaviour
{
    [SerializeField] float tiempoBase = 8f;
    [SerializeField] RectTransform barraActual;
    [SerializeField] GameObject panelGetReady;
    [SerializeField] TextMeshProUGUI textoGetReady;
    [SerializeField] float tiempoGetReady = 2f;
    [SerializeField] float tiempoFaster = 1.5f;

    private float tiempoRestante;
    private float anchoOriginal;
    private float tiempoFinal;
    private bool timerActivo = false;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            float multiplicador = GameManager.Instance.ObtenerMultiplicadorDificultad();
            tiempoFinal = tiempoBase / multiplicador;
        }
        else
        {
            tiempoFinal = tiempoBase;
        }

        tiempoRestante = tiempoFinal;
        anchoOriginal = barraActual.rect.width;
        barraActual.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, anchoOriginal);

        panelGetReady.SetActive(true);
        StartCoroutine(SecuenciaInicio());
    }

    IEnumerator SecuenciaInicio()
{
    textoGetReady.enabled = true;

    // 1. Mostrar resultado anterior si existe
    if (GameManager.Instance != null && GameManager.Instance.ultimoResultado.HasValue)
    {
        if (GameManager.Instance.ultimoResultado.Value)
        {
          textoGetReady.text = "Nice!";
        }
        else if (GameManager.Instance.vidas <= 0)
        {
            textoGetReady.text = "Too Bad...";
        }
        else
        {
           textoGetReady.text = "Too Slow!";
        }
        yield return new WaitForSeconds(2f);
    }

    // 2. Faster! opcional
    bool subioDificultad = GameManager.Instance != null &&
                           GameManager.Instance.NotificacionDificultad();
    if (subioDificultad)
    {
        textoGetReady.text = "Faster!";
        yield return new WaitForSeconds(tiempoFaster);
    }

    // 3. Get Ready!
    textoGetReady.text = "Get Ready!";
    yield return new WaitForSeconds(tiempoGetReady);

    // 4. Limpiar texto e iniciar timer
    //textoGetReady.text = "";
    
// Reemplaza timerActivo = true por:
if (GameManager.Instance != null && GameManager.Instance.vidas <= 0)
{
    SceneManager.LoadScene("Score");
}
else
{
    timerActivo = true;
}
}

    public void SecuenciaLost()
    {
        
    }

    void Update()
    {
        if (!timerActivo) return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;

            float progreso = tiempoRestante / tiempoFinal;
            barraActual.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                anchoOriginal * progreso
            );
        }
        else
        {
            timerActivo = false;
            barraActual.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);

            // Cargar el minijuego guardado
            if (GameManager.Instance != null)
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    GameManager.Instance.proximoMinijuego
                );
        }
    }

[ContextMenu("Simular Ganó")]
void SimularGano()
{
    if (GameManager.Instance != null)
        GameManager.Instance.MinigameWon();
}

[ContextMenu("Simular Perdió")]
void SimularPerdio()
{
    if (GameManager.Instance != null)
        GameManager.Instance.MinigameLost();
}


}