using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public int nivelEsperado;
    public PurdueModelManager purdueModelManager;

    private bool ocupado = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (ocupado) return;

        DragCard tarjeta = eventData.pointerDrag.GetComponent<DragCard>();

        if (tarjeta != null)
        {
            if (tarjeta.nivelCorrecto == nivelEsperado)
            {
                tarjeta.ColocarEnZona(transform);
                ocupado = true;
                purdueModelManager.NivelCorrecto();
            }
            else
            {
                tarjeta.Regresar();
            }
        }
    }
}