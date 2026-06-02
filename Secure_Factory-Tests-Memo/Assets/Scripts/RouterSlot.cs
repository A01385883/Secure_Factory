using UnityEngine;

public class RouterSlot : MonoBehaviour
{
    private RouterDrag placedRouter = null;
    public float detectionRadius = 0.6f;

    public bool IsOccupied() => placedRouter != null;

    public void PlaceRouter(RouterDrag router)
    {
        placedRouter = router;
        router.transform.position = transform.position;

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance es null. Asegurate de que el GameObject con GameManager esta en escena.");
            return;
        }

        GameManager.Instance.RouterPlaced();
    }

    void Update()
    {
        if (placedRouter == null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (var h in hits)
        {
            PacketMover mover = h.GetComponent<PacketMover>();
            if (mover != null && mover.IsMalicious())
                mover.BlockPacket();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}