 using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class IntermissionTimer : MonoBehaviour
{
    [SerializeField] float tiempoBase = 5f;
    [SerializeField] RectTransform barraActual;
    [SerializeField] GameObject panelGetReady;
    [SerializeField] TextMeshProUGUI textoGetReady;
    [SerializeField] float tiempoGetReady = 2f;
    [SerializeField] float tiempoFaster = 1.5f;

    private float tiempoRestante;
    private float anchoOriginal;
    private float tiempoFinal;
    private bool timerActivo = false;

[SerializeField] GameObject[] vidasVisuales; // Arrastra los 3 sprites aquí en orden
[SerializeField] float tiempoDesvanecimiento = 1.5f; // Debe ser menor que tiempoResultado

[SerializeField] TextMeshProUGUI textoScore;

void ActualizarScore()
{
    if (textoScore != null && GameManager.Instance != null)
        textoScore.text = $"Score: {GameManager.Instance.score}";
}

void AplicarEstadoVidas()
{
    if (GameManager.Instance == null) return;
    
    int vidasActuales = GameManager.Instance.vidas;
    
    for (int v = 0; v < vidasVisuales.Length; v++)
    {
        Image imagen = vidasVisuales[v].GetComponent<Image>();
        if (imagen == null) continue;
        
        // Si el índice es mayor o igual a las vidas actuales, apágalo
        if (v >= vidasActuales)
            imagen.color = new Color(imagen.color.r, imagen.color.g, imagen.color.b, 0f);
        else
            imagen.color = new Color(imagen.color.r, imagen.color.g, imagen.color.b, 1f);
    }
}

IEnumerator DesvanecerVida()
{
    // Con Too Bad vidas es 0, con Too Slow es la que se acaba de perder
    int indiceVida = GameManager.Instance.vidas;
    if (indiceVida >= vidasVisuales.Length) yield break;

    Image imagen = vidasVisuales[indiceVida].GetComponent<Image>();
    if (imagen == null) yield break;

    float tiempoTranscurrido = 0f;
    Color colorOriginal = imagen.color;

    while (tiempoTranscurrido < tiempoDesvanecimiento)
    {
        tiempoTranscurrido += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, tiempoTranscurrido / tiempoDesvanecimiento);
        imagen.color = new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, alpha);
        yield return null;
    }

    imagen.color = new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, 0f);
}
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
        ActualizarScore();
        AplicarEstadoVidas();
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
            StartCoroutine(DesvanecerVida());
        }
        else
        {
           textoGetReady.text = "Too Slow!";
           StartCoroutine(DesvanecerVida());
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
    SceneManager.LoadScene("Puntuaje");
}
else
{
    timerActivo = true;
}
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

    if (GameManager.Instance != null)
    {
        Debug.Log($"GameManager existe");
        Debug.Log($"proximoMinijuego: '{GameManager.Instance.proximoMinijuego}'");
        Debug.Log($"vidas: {GameManager.Instance.vidas}");
        Debug.Log($"totalJugados: {GameManager.Instance.totalJugados}");
        SceneManager.LoadScene(GameManager.Instance.proximoMinijuego);
    }
    else
    {
        Debug.LogError("GameManager.Instance es null al intentar cargar escena");
    }
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