using UnityEngine;
using UnityEngine.InputSystem;

public class PacketScript : MonoBehaviour
{
    private Camera camaraPrincipal;
    private Color[] colorArr;
    private float moveSpeed = 3f;
    private float gravity = 2f;
    private float deadZoneX = 9.5f;
    private float deadZoneY = -5.5f;

    private int priority;
    private bool clicked = false;
    private bool holding = false;

    void Start()
    {
        camaraPrincipal = Camera.main;
        colorArr = new Color[4];
        ColorUtility.TryParseHtmlString("#eb4034", out colorArr[3]);
        ColorUtility.TryParseHtmlString("#fa7f05", out colorArr[2]);
        ColorUtility.TryParseHtmlString("#ffff00", out colorArr[1]);
        ColorUtility.TryParseHtmlString("#38f205", out colorArr[0]);
        priority = Random.Range(1, 5);
        GetComponent<SpriteRenderer>().color = colorArr[priority - 1];
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameOver())
        {
            if (holding) return;
            float speed = moveSpeed * Mathf.Pow(1.10f, GameManager.Instance.ObtenerDificultadInt());
            if (!clicked)
                transform.position += Vector3.right * speed * Time.deltaTime;
            else
                transform.position += Vector3.down * (moveSpeed + gravity) * Time.deltaTime;

            if (transform.position.x > deadZoneX || transform.position.y < deadZoneY)
            {
                if (!GameManager.Instance.debugMode)
                    GameManager.Instance.MinigameLost();
                Destroy(gameObject);
            }
        }
    }

    void OnMouseDrag()
    {
        if (!GameManager.Instance.IsGameOver())
        {
            clicked = true;
            holding = true;
            Vector3 posicionMouse = Mouse.current.position.ReadValue();
            posicionMouse.z = 1f;
            transform.position = camaraPrincipal.ScreenToWorldPoint(posicionMouse);
        }
    }

    void OnMouseUp()
    {
        if (!GameManager.Instance.IsGameOver()) holding = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BucketScript bucketScript = other.GetComponent<BucketScript>();
        if (bucketScript != null && priority != bucketScript.priority && !GameManager.Instance.debugMode)
            GameManager.Instance.MinigameLost();
        Destroy(gameObject);
    }
}