using UnityEngine;

public class PacketSpawner : MonoBehaviour
{
    public GameObject component;
    private float spawnRate = 1;
    private float spawnTimer = 0;

    void Start()
    {
        Instantiate(component, transform.position, Quaternion.identity);
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameOver())
        {
            if (spawnTimer < (spawnRate / GameManager.Instance.ObtenerMultiplicadorDificultad()))
                spawnTimer += Time.deltaTime;
            else
            {
                Instantiate(component, transform.position, Quaternion.identity);
                spawnTimer = 0;
            }
        }
    }
}