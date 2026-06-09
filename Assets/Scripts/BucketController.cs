using UnityEngine;
using UnityEngine.InputSystem;

public class BucketController : MonoBehaviour
{
    public Transform[] lanes;
    private int currentLane = 1;
    private BucketManager bucketManager;
    private EventManagerScript eventManagerScript;

    void Start()
    {
        bucketManager = FindAnyObjectByType<BucketManager>();
        eventManagerScript = FindAnyObjectByType<EventManagerScript>();
        SnapToLane();
    }

    void Update()
    {
        if (!bucketManager.gameOver && !eventManagerScript.paused)
        {
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame) MoveLeft();
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame) MoveRight();
        }
    }

    public void MoveLeft()
    {
        if (currentLane > 0) { currentLane--; SnapToLane(); }
    }

    public void MoveRight()
    {
        if (currentLane < lanes.Length - 1) { currentLane++; SnapToLane(); }
    }

    void SnapToLane()
    {
        transform.position = new Vector3(
            lanes[currentLane].position.x,
            transform.position.y,
            transform.position.z
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        FallingEvent fe = other.GetComponent<FallingEvent>();
        if (fe != null) fe.OnCaught();
    }
}