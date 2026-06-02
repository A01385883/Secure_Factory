using UnityEngine;
using TMPro; // Para usar TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Referencia al objeto de texto
    public int currentScore = 1000;  // Tu puntuación actual

    void Start()
    {
        UpdateScoreDisplay();
    }

    // Método para actualizar visualmente el texto
    void UpdateScoreDisplay()
    {
        scoreText.text = currentScore.ToString("N0"); // añade separadores de miles
    }
}