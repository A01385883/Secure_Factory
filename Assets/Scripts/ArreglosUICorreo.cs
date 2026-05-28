using UnityEngine;

[ExecuteAlways] // Se actualiza en tiempo de edición también
public class CameraFitUI : MonoBehaviour
{
    [Header("Offsets por lado (en píxeles)")]
    public float offsetTop    =  0f;
    public float offsetBottom =  0f;
    public float offsetLeft   =  0f;
    public float offsetRight  =  0f;

    private RectTransform rectTransform;
    private Canvas rootCanvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rootCanvas    = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        ApplyFit();
    }

    void ApplyFit()
    {
        if (rectTransform == null || rootCanvas == null) return;

        // Ancla el rect a toda la pantalla
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.pivot     = new Vector2(0.5f, 0.5f);

        // Aplica los offsets como márgenes
        rectTransform.offsetMin = new Vector2( offsetLeft,  offsetBottom); // esquina inferior-izq
        rectTransform.offsetMax = new Vector2(-offsetRight, -offsetTop);   // esquina superior-der
    }
}