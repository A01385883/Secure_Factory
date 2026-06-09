using UnityEngine;

public class IntermissionSound : MonoBehaviour
{
    public AudioClip winSound;
    public AudioClip lossSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager no encontrado");
            return;
        }

        if (!GameManager.Instance.ultimoResultado.HasValue)
        {
            Debug.Log("Sin resultado anterior, no se reproduce sonido");
            return;
        }

        if (GameManager.Instance.ultimoResultado.Value)
        {
            // true = ganó (Nice!)
            if (winSound != null)
                audioSource.PlayOneShot(winSound);
        }
        else
        {
            // false = perdió (Too Bad / Too Slow)
            if (lossSound != null)
                audioSource.PlayOneShot(lossSound);
        }
    }
}