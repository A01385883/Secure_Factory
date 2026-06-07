using UnityEngine;
using UnityEngine.EventSystems;

public class DragCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int nivelCorrecto;

    private Vector3 posicionInicial;
    private Transform padreInicial;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Canvas canvasPrincipal;

    void Start()
    {
        posicionInicial = transform.position;
        padreInicial = transform.parent;
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        canvasPrincipal = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        posicionInicial = transform.position;
        padreInicial = transform.parent;

        canvasGroup.blocksRaycasts = false;

        transform.SetParent(canvasPrincipal.transform, true);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rectTransform,
            eventData.position,
            canvasPrincipal.worldCamera,
            out Vector3 posicionMundo
        );

        rectTransform.position = posicionMundo;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (transform.parent == canvasPrincipal.transform)
        {
            Regresar();
        }
    }

    public void Regresar()
    {
        transform.SetParent(padreInicial, true);
        transform.position = posicionInicial;
    }

    public void ColocarEnZona(Transform zona)
    {
        transform.SetParent(zona, true);
        transform.position = zona.position;
    }
}