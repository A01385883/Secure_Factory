using UnityEngine;

public class EventSpawner : MonoBehaviour
{
    public GameObject eventPrefab;
    public Transform[] lanes;
    private float spawnRate = 2f;
    private float spawnTimer = 0f;
    private BucketManager bucketManager;
    private EventManagerScript eventManagerScript;

    void Start()
    {
        bucketManager = FindAnyObjectByType<BucketManager>();
        eventManagerScript = FindAnyObjectByType<EventManagerScript>();
        spawnRate = GetSpawnRate();
        SpawnEvent();
    }

    void Update()
    {
        if (!bucketManager.gameOver && !eventManagerScript.paused)
        {
            spawnTimer += Time.deltaTime;
            int nivel = GameManager.Instance.ObtenerNivelDificultad();
            if (spawnTimer >= spawnRate / nivel)
            {
                SpawnEvent();
                spawnTimer = 0f;
            }
        }
    }

void SpawnEvent()
{
    int nivel = GameManager.Instance.ObtenerNivelDificultad();
    int lane = Random.Range(0, lanes.Length);
    GameObject obj = Instantiate(eventPrefab, lanes[lane].position, Quaternion.identity);
    FallingEvent fe = obj.GetComponent<FallingEvent>();
    Debug.Log("FallingEvent encontrado: " + fe);
    if (fe != null)
    {
        float anomalyChance = 0.3f + (nivel * 0.03f);
        bool isAnomaly = Random.value < anomalyChance;
        fe.Init(isAnomaly);
    }
}
    float GetSpawnRate()
    {
        int nivel = GameManager.Instance.ObtenerNivelDificultad();
        // D1: 1.7s, D2: 1.4s, D3: 1.2s, D4: 1.0s, D5: 0.8s
        return Mathf.Max(0.8f, 2f - (nivel * 0.3f));
    }
}