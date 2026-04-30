using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    public void Iniciar()
    {
        SceneManager.LoadScene(1);
    }

    // Nuevo método para Créditos
    public void Creditos()
    {
        SceneManager.LoadScene(4); 
    }

  public void Minijuego()
    {
        SceneManager.LoadScene(2);
    }
    public void Salir()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


}