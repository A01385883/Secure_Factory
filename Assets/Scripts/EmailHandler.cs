using UnityEngine;
using UnityEngine.SceneManagement;

public class EmailHandler : MonoBehaviour
{
    [Header("Nombres de escenas")]
    public string winScene  = "PantallaInicial";
    public string loseScene = "Credits";

    [HideInInspector]
    public Correo.Email datos;

    public void OnDescargarClicked()
    {
        if (datos == null)
        {
            Debug.LogWarning("Este EmailHandler no tiene datos asignados.");
            return;
        }

        if (datos.esBueno) Win();
        else Lose();
    }

    public void OnIgnorarClicked()
    {
        if (datos == null)
        {
            Debug.LogWarning("Este EmailHandler no tiene datos asignados.");
            return;
        }

        if (!datos.esBueno) Win();
        else Lose();
    }

    void Win()
    {
        Debug.Log($"Ganaste — {datos.nombre}");
        Time.timeScale = 1f;
        SceneManager.LoadScene(winScene);
    }

    void Lose()
    {
        Debug.Log($"Perdiste — {datos.nombre}");
        Time.timeScale = 1f;
        SceneManager.LoadScene(4);
    }
}