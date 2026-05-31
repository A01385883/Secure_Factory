using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    public int cardId;                 // mismo id para las dos cartas del par
    public Sprite backSprite;          // tarjeta volteada
    public Sprite frontSprite;         // tarjeta del objeto correspondiente
    private SpriteRenderer sr;
    private MemoramaManager manager;
    public bool isFlipped = false;
    public bool isMatched = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        manager = FindFirstObjectByType<MemoramaManager>();
        ShowBack();
    }

    void OnMouseDown()
    {
        if (isMatched) return;
        if (isFlipped) return;

        FlipCard();
        manager.SelectCard(this);
    }

    public void FlipCard()
    {
        isFlipped = true;
        sr.sprite = frontSprite;
    }

    public void ShowBack()
    {
        isFlipped = false;
        sr.sprite = backSprite;
    }

    public void SetMatched()
    {
        isMatched = true;
    }
}