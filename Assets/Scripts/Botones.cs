using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    public void Iniciar()
    {
        SceneManager.LoadScene("Intermission");
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
