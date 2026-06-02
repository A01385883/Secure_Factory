using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Correo : MonoBehaviour
{
    [System.Serializable]
    public class Email
    {
        public string nombre;
        public bool esBueno;
        public Color color;
        public Sprite fotoPerfil;
    }

    [Header("Datos de este correo")]
    public Email datos;

    [Header("Referencias hijos")]
    public Image imagenPerfil;

    [Header("Timer")]
    [SerializeField] float tiempoBase = 8f;
    [SerializeField] RectTransform barraActual;
    [SerializeField] float tiempoEspera = 2f;

    [Header("Panel Resultado")]
    [SerializeField] GameObject panelResultado;
    [SerializeField] TextMeshProUGUI textoResultado;

    [Header("Indicaciones")]
    [SerializeField] string mensajeIndicaciones = "Download the correct email!";
    [SerializeField] float tiempoIndicaciones = 1.5f;

    private float tiempoRestante;
    private float anchoOriginal;
    private bool timerActivo = false;
    private bool resultadoMostrado = false;

    void Start()
    {
     if (GameManager.Instance != null)
    {
        float multiplicador = GameManager.Instance.ObtenerMultiplicadorDificultad();
        tiempoBase = tiempoBase / multiplicador;
    }

    if (imagenPerfil != null && datos.fotoPerfil != null)
        imagenPerfil.sprite = datos.fotoPerfil;


        // Iniciar timer
        tiempoRestante = tiempoBase;
        timerActivo = false;
        if (barraActual != null)
        {
            anchoOriginal = barraActual.rect.width;
            barraActual.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, anchoOriginal);
        }

        // Mostrar indicaciones
        if (panelResultado != null)
        {
            panelResultado.SetActive(true);
            if (textoResultado != null)
                textoResultado.text = mensajeIndicaciones;
        }

        StartCoroutine(SecuenciaInicio());
    }

    IEnumerator SecuenciaInicio()
    {
        yield return new WaitForSeconds(tiempoIndicaciones);

        if (panelResultado != null)
            panelResultado.SetActive(false);

        Debug.Log($"[{gameObject.name}] Panel oculto, timer iniciado");
        timerActivo = true;
    }

    void Update()
    {
        if (!timerActivo || barraActual == null || resultadoMostrado) return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            float progreso = tiempoRestante / tiempoBase;
            barraActual.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                anchoOriginal * progreso
            );
        }
        else
        {
            timerActivo = false;
            barraActual.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            MostrarResultado(false);
        }
    }

    public void OnDescargarClicked()
    {
        Debug.Log($"CLICK DETECTADO en {gameObject.name}");
        if (datos == null || resultadoMostrado) return;
        timerActivo = false;

        if (datos.esBueno)
        {
            Debug.Log($"'{datos.nombre}' es Legítimo — Ganaste!");
            MostrarResultado(true);
        }
        else
        {
            Debug.Log($"'{datos.nombre}' es Hacker — Perdiste!");
            MostrarResultado(false);
        }
    }

    public void OnIgnorarClicked()
    {
        Debug.Log($"CLICK DETECTADO en {gameObject.name}");
        if (datos == null || resultadoMostrado) return;
        timerActivo = false;

        if (!datos.esBueno)
        {
            Debug.Log($"'{datos.nombre}' ignorado correctamente — Ganaste!");
            MostrarResultado(true);
        }
        else
        {
            Debug.Log($"'{datos.nombre}' era legítimo y lo ignoraste — Perdiste!");
            MostrarResultado(false);
        }
    }

    void MostrarResultado(bool gano)
    {
        resultadoMostrado = true;

        if (panelResultado != null)
        {
            panelResultado.SetActive(true);
            if (textoResultado != null)
                textoResultado.text = gano ? "Success!" : "Failure!";
        }

        if (GameManager.Instance != null)
        {
            if (gano) GameManager.Instance.MinigameWon();
            else      GameManager.Instance.MinigameLost();
        }

        StartCoroutine(EsperarYCargar());
    }

    IEnumerator EsperarYCargar()
    {
        yield return new WaitForSeconds(tiempoEspera);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Intermission");
    }
}