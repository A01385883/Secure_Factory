using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

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

    [Header("Escenas")]
    public string winScene  = "Credits";
    public string loseScene = "PantallaInicial";

    [Header("Referencias hijos")]
    public Image imagenPerfil;

    [Header("Sprites (asignar solo en UNO de los correos)")]
    public Sprite[] hackersSprites;
    public Sprite empleadoSprite;

    static List<Sprite> hackersDisponibles;
    static int hackerIndex = 0;
    static bool spritesListaReady = false;

    void Awake()
    {
        if (hackersSprites != null && hackersSprites.Length > 0)
        {
            hackersDisponibles = new List<Sprite>(hackersSprites);
            Shuffle(hackersDisponibles);
            hackerIndex = 0;
            spritesListaReady = true;
        }
    }

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = datos.color;

        if (datos.fotoPerfil == null && spritesListaReady)
        {
            if (datos.esBueno)
                datos.fotoPerfil = empleadoSprite;
            else
            {
                datos.fotoPerfil = hackersDisponibles[hackerIndex % hackersDisponibles.Count];
                hackerIndex++;
            }
        }

        if (imagenPerfil != null && datos.fotoPerfil != null)
            imagenPerfil.sprite = datos.fotoPerfil;
    }

    public void OnDescargarClicked()
    {
        if (datos == null) return;

        if (datos.esBueno)
        {
            Debug.Log($"'{datos.nombre}' es Legítimo — Ganaste!");
            Win();
        }
        else
        {
            Debug.Log($"'{datos.nombre}' es Hacker — Perdiste!");
            Lose();
        }
    }

    public void OnIgnorarClicked()
    {
        if (datos == null) return;

        if (!datos.esBueno)
        {
            Debug.Log($"'{datos.nombre}' ignorado correctamente — Ganaste!");
            Win();
        }
        else
        {
            Debug.Log($"'{datos.nombre}' era legítimo y lo ignoraste — Perdiste!");
            Lose();
        }
    }

    void Win()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(winScene);
    }

    void Lose()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(loseScene);
    }

    void Shuffle<T>(List<T> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lista[i], lista[j]) = (lista[j], lista[i]);
        }
    }
}