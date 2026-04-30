using UnityEngine;
using UnityEngine.InputSystem;

public class RouterDrag : MonoBehaviour
{
    private Vector3 originPosition;
    private bool dragging = false;
    private Camera cam;

    void Start()
    {
        originPosition = transform.position;
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        dragging = true;
    }

    void OnMouseUp()
    {
        dragging = false;

        RouterSlot[] slots = FindObjectsByType<RouterSlot>(FindObjectsInactive.Exclude);
        RouterSlot best = null;
        float bestDist = 0.8f;

        foreach (var slot in slots)
        {
            float d = Vector3.Distance(transform.position, slot.transform.position);
            if (d < bestDist && !slot.IsOccupied())
            {
                bestDist = d;
                best = slot;
            }
        }

        if (best != null)
            best.PlaceRouter(this);
        else
            ReturnToOrigin();
    }

    void Update()
    {
        if (!dragging) return;

        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, 0f));
        transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
    }

    public void ReturnToOrigin()
    {
        transform.position = originPosition;
    }
}