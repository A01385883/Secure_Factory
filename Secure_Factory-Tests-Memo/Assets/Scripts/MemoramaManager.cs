using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoramaManager : MonoBehaviour
{
    public CardBehaviour[] cards;
    public int score = 0;
    public TMP_Text scoreText;

    public GameObject winCanvas;
    public string nextSceneName = "SiguienteMinijuego";
    public float winDelay = 2f;

    public bool gameEnded = false;

    private List<Vector3> posicionesOriginales = new List<Vector3>();

    private CardBehaviour firstCard;
    private CardBehaviour secondCard;
    private bool canPlay = true;
    private int matchedPairs = 0;
    public int totalPairs = 6;

    void Start()
    {
        GuardarPosiciones();
        MezclarTarjetas();
        UpdateScore();

        if (winCanvas != null)
            winCanvas.SetActive(false);
    }

    void GuardarPosiciones()
    {
        posicionesOriginales.Clear();

        foreach (CardBehaviour card in cards)
        {
            if (card != null)
                posicionesOriginales.Add(card.transform.position);
        }
    }

    void MezclarTarjetas()
    {
        List<Vector3> posicionesMezcladas = new List<Vector3>(posicionesOriginales);

        for (int i = 0; i < posicionesMezcladas.Count; i++)
        {
            int randomIndex = Random.Range(i, posicionesMezcladas.Count);

            Vector3 temp = posicionesMezcladas[i];
            posicionesMezcladas[i] = posicionesMezcladas[randomIndex];
            posicionesMezcladas[randomIndex] = temp;
        }

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)
                cards[i].transform.position = posicionesMezcladas[i];
        }
    }

    public void SelectCard(CardBehaviour selectedCard)
    {
        if (!canPlay || gameEnded) return;

        if (firstCard == null)
        {
            firstCard = selectedCard;
            return;
        }

        if (secondCard == null && selectedCard != firstCard)
        {
            secondCard = selectedCard;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        canPlay = false;

        yield return new WaitForSeconds(1f);

        Debug.Log("Carta 1: " + firstCard.name + " | ID: " + firstCard.cardId + " | Sprite: " + firstCard.frontSprite.name);
        Debug.Log("Carta 2: " + secondCard.name + " | ID: " + secondCard.cardId + " | Sprite: " + secondCard.frontSprite.name);

        if (firstCard.cardId == secondCard.cardId)
        {
            firstCard.SetMatched();
            secondCard.SetMatched();
            score += 10;
            matchedPairs++;

            if (matchedPairs >= totalPairs)
            {
                Debug.Log("Ganaste el memorama");
                gameEnded = true;
                canPlay = false;

                if (winCanvas != null)
                    winCanvas.SetActive(true);

                StartCoroutine(GoToNextSceneAfterWin());
            }
        }
        else
        {
            firstCard.ShowBack();
            secondCard.ShowBack();
            score -= 1;
        }

        UpdateScore();

        firstCard = null;
        secondCard = null;

        if (!gameEnded)
            canPlay = true;
    }

    void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

IEnumerator GoToNextSceneAfterWin()
{
    yield return new WaitForSeconds(winDelay);
    
    if (GameManager.Instance != null)
        GameManager.Instance.MinigameWon(); // O MinigameLost() si perdió
}
}