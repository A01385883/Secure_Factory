using UnityEngine;
using System.Collections.Generic;

public class CorreoManager : MonoBehaviour
{
    [Header("Correos")]
    public Correo[] correos;

    [Header("Nombres Correo 'Bueno'")]
    public string nombreBueno = "Business";

    [Header("Nombres Correos Maliciosos")]
    public string[] nombresMalos = { "Hacker_1", "Hacker_2" };

    void Awake()
    {
        if (correos.Length < 3)
        {
            Debug.LogWarning("Asigna los 3 correos en el Inspector.");
            return;
        }

        // Crea la lista garantizando 1 bueno y 2 malos
        List<Correo.Email> roles = new List<Correo.Email>
        {
            new Correo.Email { nombre = nombreBueno,      esBueno = true,  color = Color.green },
            new Correo.Email { nombre = nombresMalos[0],  esBueno = false, color = Color.red   },
            new Correo.Email { nombre = nombresMalos[1],  esBueno = false, color = Color.red   },
        };

        // Baraja los roles
        Shuffle(roles);

        // Asigna un rol a cada correo
        for (int i = 0; i < correos.Length; i++)
            correos[i].datos = roles[i];
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