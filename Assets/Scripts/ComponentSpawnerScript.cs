using UnityEngine;
using System.Linq;

public class ComponentSpawnerScript : MonoBehaviour
{
    public GameObject component;
    private float spawnRate = 2;
    private float spawnTimer = 0;
    private float difficulty = 1;

    private GameManager gameManagerScript;

    private Color[] correctColors;
    private Color[] incorrectColors;

    private Sprite[] imgs;


    void Start()
    {
        gameManagerScript = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();
        difficulty = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>().difficulty;

        correctColors = GameObject.FindWithTag("Registered Components").GetComponent<RegisteredComponentsScript>().colors;
        incorrectColors = GameObject.FindWithTag("Registered Components").GetComponent<RegisteredComponentsScript>().otherColors;

        imgs = GameObject.FindWithTag("Registered Components").GetComponent<RegisteredComponentsScript>().imgs;

        spawnComponent();
    }

    void Update()
    {
        if (!gameManagerScript.gameOver && !gameManagerScript.paused){
            if (spawnTimer < (spawnRate / difficulty))
            {
                spawnTimer += Time.deltaTime;
            }
            else
            {
                spawnComponent();
                spawnTimer = 0;
            }
        }
    }

    void spawnComponent()
    {
        GameObject obj = Instantiate(component, transform.position, Quaternion.identity);

        SpriteRenderer fondoRenderer = obj.GetComponent<SpriteRenderer>();
        SpriteRenderer iconoRenderer = obj.transform.Find("Icono").GetComponent<SpriteRenderer>();

        if (fondoRenderer != null && iconoRenderer != null) {
            // Porcentaje: Probabilidad de spawnear componente correcto

            int i;
            // Dificultad 1 y 2: 80%. Dificultad 3 y 4: 60%. Dificultad 5: 40%.
            if (Random.Range(1, 6) < (6 - Mathf.CeilToInt(difficulty / 2))) {
                i = Random.Range(0, correctColors.Length);
                fondoRenderer.color = correctColors[i];
            } else {
                i = Random.Range(0, incorrectColors.Length);
                fondoRenderer.color = incorrectColors[i];
            }

            // Dificultad 1 y 5: 95%. Dificultad 3 y 4: 90%. Dificultad 5: 85%.
            if (Random.Range(1, 21) < (21 - Mathf.CeilToInt(difficulty / 2))) {
                iconoRenderer.sprite = imgs[i];
            } else {
                iconoRenderer.sprite = imgs[Random.Range(4, imgs.Length)];
            }
        }
    }
}
