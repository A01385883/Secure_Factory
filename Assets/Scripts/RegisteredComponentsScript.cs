using UnityEngine;

public class RegisteredComponentsScript : MonoBehaviour
{
    public GameObject componentImage;
    private Color[] posibleColors;
    public Color[] colors;
    public Color[] otherColors;
    private Vector3[] positions;
    public Sprite[] imgs;

    void Awake()
    {
        // Definir arreglos en tiempo de ejecucion

        posibleColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta };
        colors = new Color[4];
        otherColors = new Color[2];

        imgs = new Sprite[]
        {
            Resources.Load<Sprite>("RegisteredComponentsMinigame/m1"),
            Resources.Load<Sprite>("RegisteredComponentsMinigame/m2"),
            Resources.Load<Sprite>("RegisteredComponentsMinigame/m3"),
            Resources.Load<Sprite>("RegisteredComponentsMinigame/m4"),
            Resources.Load<Sprite>("RegisteredComponentsMinigame/m5"),
            Resources.Load<Sprite>("RegisteredComponentsMinigame/m6"),
            Resources.Load<Sprite>("RegisteredComponentsMinigame/m7"),
            Resources.Load<Sprite>("RegisteredComponentsMinigame/m8"),
            Resources.Load<Sprite>("RegisteredComponentsMinigame/m9")
        };

        createComponentPalette();
    }

    void mezclaColores(Color[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int randI = Random.Range(0, i + 1);

            Color temp = arr[i];
            arr[i] = arr[randI];
            arr[randI] = temp;
        }
    }

    void mezclaImgs(Sprite[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int randI = Random.Range(0, i + 1);

            Sprite temp = arr[i];
            arr[i] = arr[randI];
            arr[randI] = temp;
        }
    }

    void createComponentPalette()
    {
        mezclaColores(posibleColors);
        int j = 0;
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = posibleColors[j];
            j++;
        }
        for (int i = 0; i < otherColors.Length; i++)
        {
            otherColors[i] = posibleColors[j];
            j++;
        }

        mezclaImgs(imgs);

        positions = new Vector3[]
        {
            new Vector3(-3, -3, 0),
            new Vector3(-1, -3, 0),
            new Vector3( 1, -3, 0),
            new Vector3( 3, -3, 0)
        };
    }
}
