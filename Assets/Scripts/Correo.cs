using UnityEngine;
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
  [System.Serializable]
  public class SpritePool
  { //Fotos de Perfil de los correos
    public Sprite[] hackersSprites; //Multiples
    public Sprite empleadoSprite; //Uno
  }
  public SpritePool spritePool;
  void AsignarSprites(List<Email> shuffled)
  {
    List<Sprite> hackersDisponibles = new List<Sprite>(spritePool.hackersSprites);
    Shuffle(hackersDisponibles);
    int hackerIndex = 0;
    
    foreach (Email email in shuffled)
    {
      if (email.esBueno)
      {
        email.fotoPerfil = spritePool.empleadoSprite;
      }
      else
      {
        email.fotoPerfil = hackersDisponibles[hackerIndex % hackersDisponibles.Count];
        hackerIndex++;
      }
    }
  }
    //Logica en Click
    public class EmailHandler : MonoBehaviour
  {
    [HideInInspector]
    public Correo.Email datos;
        void OnMouseDown()
        {
            if (datos == null) return;
            if (datos.esBueno)
               Debug.Log($"'{datos.nombre}' es un Correo Legitimo");
            if (!datos.esBueno)
               Debug.Log($"'{datos.nombre}' es un Hacker... Oops");
            else
              Debug.Log($"What?");   
        }
    }
    // Pool unificada — solo List<Email>, ya no necesitas List<string>
    public GameObject[] Correos_Minijuego;
    public List<Email> pool = new List<Email>
    {
        new Email { nombre = "Business",  esBueno = true,  color = Color.green },
        new Email { nombre = "Hacker_1",  esBueno = false, color = Color.red   },
        new Email { nombre = "Hacker_2",  esBueno = false, color = Color.red   },
    };
    List<Email> GetRandomSelection()
    {
        List<Email> copia = new List<Email>(pool);
        Shuffle(copia);
        return copia;
    }
    void Shuffle<T>(List<T> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lista[i], lista[j]) = (lista[j], lista[i]);
        }
    }
    void AsignarElementos(List<Email> shuffled)
    {
      AsignarSprites(shuffled);
        for (int i = 0; i < Correos_Minijuego.Length; i++)
        {
            Email datos = shuffled[i];
            SpriteRenderer sr = Correos_Minijuego[i].GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = datos.color;
            Transform fotoTransform = Correos_Minijuego[i].transform.Find("Image_Correo");
            if (fotoTransform != null)
      {
        UnityEngine.UI.Image img = fotoTransform.GetComponent<UnityEngine.UI.Image>();
        if (img != null)
        img.sprite = datos.fotoPerfil;
      }    
            EmailHandler handler = Correos_Minijuego[i].GetComponent<EmailHandler>();
            if (handler != null)
                handler.datos = datos;
        }
    }
    void Start()
    {
        List<Email> seleccion = GetRandomSelection();
        for (int i = 0; i < seleccion.Count; i++)
            Debug.Log($"Slot {i + 1}: {seleccion[i].nombre}");
        AsignarElementos(seleccion); // reutiliza la misma lista ya barajada
    }
    void Update() { }
}