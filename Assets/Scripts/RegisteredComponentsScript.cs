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
        colors = new Color[] { Color.black, Color.black, Color.black, Color.black };
        otherColors = new Color[] { Color.black, Color.black };

        imgs = new Sprite[]
        {
            Resources.Load<Sprite>("m1"),
            Resources.Load<Sprite>("m2"),
            Resources.Load<Sprite>("m3"),
            Resources.Load<Sprite>("m4"),
            Resources.Load<Sprite>("m5"),
            Resources.Load<Sprite>("m6"),
            Resources.Load<Sprite>("m7"),
            Resources.Load<Sprite>("m8"),
            Resources.Load<Sprite>("m9")
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
