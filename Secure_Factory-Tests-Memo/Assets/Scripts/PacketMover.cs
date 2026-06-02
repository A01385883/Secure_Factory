using UnityEngine;

public class PacketMover : MonoBehaviour
{
    public float speed = 2f;

    private Transform[] waypoints;
    private int currentTarget = 0;
    private bool isMalicious = false;
    private bool blocked = false;
    private SpriteRenderer sr;

    public void Init(Transform[] points, float spd, SpriteRenderer renderer)
    {
        waypoints = points;
        speed = spd;
        sr = renderer;
        currentTarget = 0;
        isMalicious = false;
    }

    public bool IsMalicious() => isMalicious;

    void Update()
    {
        if (blocked) return;
        if (GameManager.Instance == null || GameManager.Instance.IsGameOver()) return;
        if (waypoints == null || currentTarget >= waypoints.Length) return;

        Transform target = waypoints[currentTarget];
        transform.position = Vector3.MoveTowards(
            transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
            ReachWaypoint();
    }

    void ReachWaypoint()
    {
        // Al llegar a la ciudad (waypoint 1) se vuelve malicioso
        if (currentTarget == 1)
        {
            isMalicious = true;
            if (sr != null)
                sr.color = Color.red;
        }

        currentTarget++;

        // Al llegar a Fabrica B (waypoint 2, ultimo)
        if (currentTarget >= waypoints.Length)
        {
            if (isMalicious)
                GameManager.Instance.MaliciousPacketReached();
            Destroy(gameObject);
        }
    }

    public void BlockPacket()
    {
        blocked = true;
        Destroy(gameObject, 0.15f);
    }
}