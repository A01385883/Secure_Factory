using UnityEngine;
using UnityEngine.UI;

public class SceneTimer : MonoBehaviour
{
    [SerializeField] float tiempoBase = 8f;
    [SerializeField] RectTransform barraActual;

    private float tiempoRestante;
    private float anchoOriginal;
    private float tiempoFinal;

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
    }

    void Update()
    {
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
            barraActual.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);

            if (GameManager.Instance != null)
                GameManager.Instance.MinigameLost();
        }
    }
}