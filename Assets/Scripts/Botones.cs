using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    public void Iniciar()
    {
        if (DatabaseManager.Instance != null)
        {
            DatabaseManager.Instance.StartRound(roundId =>
            {
                // Al responder con éxito la base de datos, cargamos la primera escena
                SceneManager.LoadScene("Intermission");
            }, error =>
            {
                Debug.LogError("Error de BD al iniciar ronda: " + error);
                SceneManager.LoadScene("Intermission");
            });
        }
        else
        {
            SceneManager.LoadScene("Intermission");
        }
    }

    public void Salir()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void Menu()
    {
        SceneManager.LoadScene("PantallaInicial");
    }
}
