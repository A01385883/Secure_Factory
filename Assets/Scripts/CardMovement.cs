using UnityEngine;
using UnityEngine.UI;

public class CardMovement : MonoBehaviour
{
    public float speed = 200f;
    public float firewallX = 100f;
    public float blockedDestroyTime = 1f;
    RectTransform rectTransform;
    Toggle toggle;
    bool isBlockedAtFirewall = false;
    float blockedTimer = 0f;
    public int riskAmount = 0;
    bool alreadyProcessed = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        toggle = GetComponent<Toggle>();
    }

    void Update()
    {
        if (rectTransform.anchoredPosition.x < firewallX)
        {
            rectTransform.anchoredPosition += Vector2.right * speed * Time.deltaTime;
            return;
        }

        if (!toggle.isOn)
        {
            isBlockedAtFirewall = true;
            blockedTimer += Time.deltaTime;

            if (blockedTimer >= blockedDestroyTime)
            {
                Destroy(gameObject);
            }

            return;
        }

        if (!alreadyProcessed)
        {
            alreadyProcessed = true;

            MiniGameTimer timer = FindAnyObjectByType<MiniGameTimer>();

            if (timer != null)
            {
                timer.AddRisk(riskAmount);
            }
        }

        rectTransform.anchoredPosition += Vector2.right * speed * Time.deltaTime;

        if (rectTransform.anchoredPosition.x > 1200)
        {
            Destroy(gameObject);
        }
    }
}