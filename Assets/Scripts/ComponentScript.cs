using UnityEngine;
using UnityEngine.InputSystem;

public class ComponentScript : MonoBehaviour
{
    public bool debugMode = false;
    private float speedScaleDif = 0.7f;
    private float moveSpeed = 5f;
    private float deadZone = 10f;
    private CriticEventManager eventManagerScript;
    private TimerScript timerScript;
    private Color[] colors;
    private Sprite[] imgs;
    private bool correct;

    void Start()
    {
        eventManagerScript = FindAnyObjectByType<CriticEventManager>();
        timerScript = FindAnyObjectByType<TimerScript>();

        var registered = GameObject.FindWithTag("Registered Components");
        if (registered == null)
        {
            Debug.LogError("No se encontró el objeto con tag 'Registered Components'");
            return;
        }

        var regScript = registered.GetComponent<RegisteredComponentsScript>();
        colors = regScript.colors;
        imgs = regScript.imgs;

        correct = isCorrect();
    }

    void Update()
    {
        if (timerScript == null || eventManagerScript == null) return;

        if (!timerScript.gameOver && !eventManagerScript.paused)
        {
            float difficulty = GameManager.Instance.ObtenerNivelDificultad();
            transform.position += Vector3.right * moveSpeed * Time.deltaTime * difficulty * speedScaleDif;

            if (transform.position.x > deadZone)
            {
                Destroy(gameObject);
                if (!debugMode)
                {
                    if (!correct)
                    {
                        timerScript.gameOver = true;
                        GameManager.Instance.MinigameLost();
                    }
                }
            }
        }
    }

    private void OnMouseOver()
    {
        if (timerScript == null || eventManagerScript == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame && !timerScript.gameOver && !eventManagerScript.paused)
        {
            Destroy(gameObject);
            if (!debugMode)
            {
                if (correct)
                {
                    timerScript.gameOver = true;
                    GameManager.Instance.MinigameLost();
                }
            }
        }
    }

    bool isCorrect()
    {
        if (colors == null || imgs == null) return false;

        SpriteRenderer fondoRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer iconoRenderer = transform.Find("Icono").GetComponent<SpriteRenderer>();

        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i] == fondoRenderer.color)
                if (imgs[i] == iconoRenderer.sprite) return true;
        }
        return false;
    }
}