using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurdueModelManager : MonoBehaviour
{
    public float tiempoTotal = 30f;
    private float tiempoActual;
    public Slider sliderTiempo;
    public TMP_Text textoEstado;
    public int nivelesCorrectos = 0;
    public int nivelesParaCompletar = 7;
    private bool juegoActivo = true;

void Start()
{
    float multiplicador = GameManager.Instance.ObtenerMultiplicadorDificultad();
    tiempoActual = tiempoTotal / multiplicador; //Multiplicador es dificultad
    tiempoTotal = tiempoActual; // actualiza el total para que el slider sea correcto

    if (sliderTiempo != null)
    {
        sliderTiempo.maxValue = tiempoTotal;
        sliderTiempo.value = tiempoTotal;
    }
    if (textoEstado != null)
        textoEstado.text = "Not completed";
}

    void Update()
    {
        if (!juegoActivo) return;
        tiempoActual -= Time.deltaTime;
        if (sliderTiempo != null)
            sliderTiempo.value = tiempoActual;
        if (tiempoActual <= 0)
            NoCompletado();
    }

    public void NivelCorrecto()
    {
        nivelesCorrectos++;
        if (nivelesCorrectos >= nivelesParaCompletar)
            Completado();
    }

    void Completado()
    {
        juegoActivo = false;
        if (textoEstado != null)
            textoEstado.text = "Completed";
        Invoke(nameof(CambiarEscena), 1f);
    }

    void NoCompletado()
    {
        juegoActivo = false;
        if (textoEstado != null)
            textoEstado.text = "Not completed";
        CambiarEscena();
    }

    void CambiarEscena()
    {
        if (juegoActivo == false && textoEstado.text == "Completed")
            GameManager.Instance.MinigameWon();
        else
            GameManager.Instance.MinigameLost();
    }
}