using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour
{
    public Sprite greenSprite;
    public Sprite greenPressedSprite;
    public Sprite redSprite;
    public Sprite redPressedSprite;
    public bool IsActive => isActive;

    private Image image;
    private Button button;
    private bool isActive = false;

    void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnPressed);
        Deactivate();
    }

    public void Activate()
    {
        isActive = true;
        image.sprite = redSprite;
        var spriteState = button.spriteState;
        spriteState.pressedSprite = redPressedSprite;
        button.spriteState = spriteState;
    }

    public void Deactivate()
    {
        isActive = false;
        image.sprite = greenSprite;
        var spriteState = button.spriteState;
        spriteState.pressedSprite = greenPressedSprite;
        button.spriteState = spriteState;
    }

void OnPressed()
{
    if (isActive)
    {
        isActive = false;
        image.sprite = redPressedSprite;
        StartCoroutine(DeactivateAfterDelay(0.15f));
    }
    else
    {
        // presionaste un boton verde que no debias
        FindAnyObjectByType<BotonesGameManager>().RegisterMiss();
    }
}

System.Collections.IEnumerator DeactivateAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    Deactivate();
    FindAnyObjectByType<BotonesGameManager>().RegisterHit();
}
}
