using UnityEngine;
using UnityEngine.UI;

public class RegisteredComponentsPanelScript : MonoBehaviour
{
    public GameObject iconoPrefab;
    private RegisteredComponentsScript registeredComponentsScript;
    public Color[] colors;
    public Sprite[] imgs;


    void Start()
    {
        registeredComponentsScript = GameObject.FindWithTag("Registered Components").GetComponent<RegisteredComponentsScript>();

        colors = registeredComponentsScript.colors;
        imgs = registeredComponentsScript.imgs;

        SpawnObjectsUI();
    }

    void SpawnObjectsUI()
    {
        Vector3[] posicionesUI = new Vector3[]
        {
            new Vector3(-300, 0, 0), 
            new Vector3(-100, 0, 0), 
            new Vector3( 100, 0, 0),
            new Vector3( 300, 0, 0) 
        };

        for (int i = 0; i < colors.Length; i++)
        {
            GameObject obj = Instantiate(iconoPrefab, this.transform);

            RectTransform rectTrans = obj.GetComponent<RectTransform>();

            Image fondoRenderer = obj.GetComponent<Image>();
            Image iconoRenderer = obj.transform.Find("Image").GetComponent<Image>();

            if (rectTrans != null && fondoRenderer != null && iconoRenderer != null)
            {
                rectTrans.anchoredPosition = posicionesUI[i];
                fondoRenderer.color = colors[i];
                iconoRenderer.sprite = imgs[i];
                iconoRenderer.SetNativeSize();
            }
        }
    }
}
