using UnityEngine;

public class ComponentSpawnerScript : MonoBehaviour
{
    public GameObject component;
    private float spawnRate = 2f;
    private float spawnTimer = 0f;
    private CriticEventManager eventManagerScript;
    private TimerScript timerScript;
    private Color[] correctColors;
    private Color[] incorrectColors;
    private Sprite[] imgs;

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
        correctColors = regScript.colors;
        incorrectColors = regScript.otherColors;
        imgs = regScript.imgs;

        spawnComponent();
    }

    void Update()
    {
        if (timerScript == null || eventManagerScript == null) return;

        if (!timerScript.gameOver && !eventManagerScript.paused)
        {
            float difficulty = GameManager.Instance.ObtenerNivelDificultad();
            if (spawnTimer < (spawnRate / difficulty))
                spawnTimer += Time.deltaTime;
            else
            {
                spawnComponent();
                spawnTimer = 0f;
            }
        }
    }

    void spawnComponent()
    {
        if (correctColors == null || incorrectColors == null || imgs == null) return;

        int difficulty = GameManager.Instance.ObtenerNivelDificultad();
        GameObject obj = Instantiate(component, transform.position, Quaternion.identity);
        SpriteRenderer fondoRenderer = obj.GetComponent<SpriteRenderer>();
        SpriteRenderer iconoRenderer = obj.transform.Find("Icono").GetComponent<SpriteRenderer>();

        if (fondoRenderer != null && iconoRenderer != null)
        {
            int i;
            if (Random.Range(1, 6) < (6 - Mathf.CeilToInt(difficulty / 2f)))
            {
                i = Random.Range(0, correctColors.Length);
                fondoRenderer.color = correctColors[i];
            }
            else
            {
                i = Random.Range(0, incorrectColors.Length);
                fondoRenderer.color = incorrectColors[i];
            }

            if (Random.Range(1, 21) < (21 - Mathf.CeilToInt(difficulty / 2f)))
                iconoRenderer.sprite = imgs[i];
            else
                iconoRenderer.sprite = imgs[Random.Range(4, imgs.Length)];
        }
    }
}