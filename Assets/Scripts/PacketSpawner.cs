using UnityEngine;

public class PacketSpawner : MonoBehaviour
{
    public GameObject packetPrefab;

    private Transform factoryA;
    private Transform city;
    private Transform factoryB;
    private float timer = 0f;

    void Start()
    {
        GameObject a = GameObject.FindWithTag("FactoryA");
        GameObject c = GameObject.FindWithTag("City");
        GameObject b = GameObject.FindWithTag("FactoryB");

        if (a == null) Debug.LogError("Falta tag FactoryA");
        if (c == null) Debug.LogError("Falta tag City");
        if (b == null) Debug.LogError("Falta tag FactoryB");

        factoryA = a?.transform;
        city     = c?.transform;
        factoryB = b?.transform;

        // Spawn inmediato al iniciar
        timer = GameManager.Instance.packetInterval;
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.IsGameOver()) return;
        if (factoryA == null || city == null || factoryB == null) return;

        timer += Time.deltaTime;
        if (timer >= GameManager.Instance.packetInterval)
        {
            timer = 0f;
            SpawnPacket();
        }
    }

    void SpawnPacket()
    {
        if (packetPrefab == null)
        {
            Debug.LogError("packetPrefab no asignado en PacketSpawner");
            return;
        }

        GameObject go = Instantiate(packetPrefab, factoryA.position, Quaternion.identity);

        PacketMover mover = go.GetComponent<PacketMover>();
        if (mover == null)
            mover = go.AddComponent<PacketMover>();

        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();

        // Ruta: Fabrica A → Ciudad → Fabrica B
        Transform[] waypoints = new Transform[] { factoryA, city, factoryB };
        mover.Init(waypoints, GameManager.Instance.packetSpeed, sr);
    }
}