using UnityEngine;

public class FallingEvent : MonoBehaviour
{
    private float speedScaleDif = 0.5f;
    private float fallSpeed = 5f;
    private float bottomDeadZone = -6f;
    private BucketManager bucketManager;
    private EventManagerScript eventManagerScript;
    private bool isAnomaly;

    public void Init(bool anomaly)
    {
        isAnomaly = anomaly;
        GetComponent<SpriteRenderer>().color = anomaly ? Color.red : Color.green;
    }

    void Start()
    {
        bucketManager = FindAnyObjectByType<BucketManager>();
        eventManagerScript = FindAnyObjectByType<EventManagerScript>();
    }

    void Update()
    {
        if (!bucketManager.gameOver && !eventManagerScript.paused)
        {
            float nivel = GameManager.Instance.ObtenerNivelDificultad();
            transform.position += Vector3.down * fallSpeed * Time.deltaTime * nivel * speedScaleDif;

            if (transform.position.y < bottomDeadZone)
            {
                if (isAnomaly && !bucketManager.debugMode)
                    bucketManager.RegisterMiss();
                Destroy(gameObject);
            }
        }
    }

    public void OnCaught()
    {
        if (!bucketManager.debugMode && !isAnomaly)
            bucketManager.RegisterMiss();
        Destroy(gameObject);
    }
}