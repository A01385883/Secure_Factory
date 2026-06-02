using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PuntuajeScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textoPuntuaje;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            textoPuntuaje.text = $"{GameManager.Instance.score}";
            Destroy(GameManager.Instance.gameObject);
        }
        else
        {
            textoPuntuaje.text = "0";
        }
    }

    public void JugarDeNuevo()
    {
        SceneManager.LoadScene("Splash"); // cambia por tu escena inicial
    }
}