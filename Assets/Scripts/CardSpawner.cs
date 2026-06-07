using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public GameObject[] cardPrefabs;
    public RectTransform spawnPoint;
    public RectTransform cardsParent;
    public float spawnInterval = 2f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnCard), 1f, spawnInterval);
    }

    void SpawnCard()
    {
        int randomIndex = Random.Range(0, cardPrefabs.Length);

        GameObject newCard = Instantiate(cardPrefabs[randomIndex], cardsParent);

        RectTransform cardRect = newCard.GetComponent<RectTransform>();

        cardRect.anchoredPosition = spawnPoint.anchoredPosition;
        cardRect.localScale = Vector3.one;
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(SpawnCard));
    }
}