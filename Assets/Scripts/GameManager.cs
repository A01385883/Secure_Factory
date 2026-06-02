using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float packetSpeed = 2f;
    public float packetInterval = 3f;
    private int routersPlaced = 0;

    public int vidas = 3;
    public int gameTimer = 8;
    public bool debugMode = false;
    private bool gameOver = false;
    private float winTimer = 0f;
    private bool countingWin = false;

    [SerializeField] int[] NivelesAumentosDificultad = { 4, 9, 15, 23 };
    [SerializeField] float[] TiempoAumentosDificultad = { 1.3f, 1.6f, 2.0f, 2.5f };
    private string[] minijuegosnombres = {"CriticAlerts","Memorama","RegisteredComponents" ,"Minijuego_Correo"};

    public int score { get; private set; }
    private int i = 0;
    public int minijuegoactual = 0;
    public int totalJugados = 0;
    private bool primerShuffle = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (vidas != 3 || score != 0)
        {
            vidas = 3;
            score = 0;
        }
    }

    void Start()
    {
        PrepararPrimerNivel();
    }

    void PrepararPrimerNivel()
    {
        proximoMinijuego = minijuegosnombres[0];
        minijuegoactual = 1;
    }

    public void addScore()
    {
        score += 100;
    }

    public bool NotificacionDificultad()
    {
        foreach (int umbral in NivelesAumentosDificultad)
            if (totalJugados == umbral) return true;
        return false;
    }

    public float ObtenerMultiplicadorDificultad()
    {
        for (int n = NivelesAumentosDificultad.Length - 1; n >= 0; n--)
            if (totalJugados >= NivelesAumentosDificultad[n])
                return TiempoAumentosDificultad[n];
        return 1f;
    }

    public int ObtenerNivelDificultad()
    {
        for (int n = NivelesAumentosDificultad.Length - 1; n >= 0; n--)
            if (totalJugados >= NivelesAumentosDificultad[n]) return n + 2;
        return 1;
    }

    public void MoreDifficult()
    {
        for (int n = 0; n < NivelesAumentosDificultad.Length; n++)
        {
            if (totalJugados == NivelesAumentosDificultad[n])
            {
                packetSpeed *= TiempoAumentosDificultad[n];
                packetInterval /= TiempoAumentosDificultad[n];
                break;
            }
        }
    }

    public bool IsGameOver() => gameOver;

    public void RouterPlaced()
    {
        routersPlaced++;
        if (routersPlaced >= 2) countingWin = true;
    }

    public void RouterRemoved()
    {
        routersPlaced = Mathf.Max(0, routersPlaced - 1);
    }

    public void MaliciousPacketReached()
    {
        if (gameOver) return;
        EndGame(false);
    }

    void EndGame(bool won)
    {
        gameOver = true;
        countingWin = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("puntuaje");
    }

    public string proximoMinijuego { get; private set; }

    public void PseudoRandomLevels()
    {
        if (minijuegoactual >= minijuegosnombres.Length)
        {
            minijuegoactual = 0;
            if (primerShuffle)
            {
                primerShuffle = false;
            }
            else
            {
                MoreDifficult();
                string ultimoJugado = minijuegosnombres[minijuegosnombres.Length - 1];
                for (int j = 0; j < minijuegosnombres.Length; j++)
                {
                    var temp = minijuegosnombres[j];
                    int randomIndex = Random.Range(j, minijuegosnombres.Length);
                    minijuegosnombres[j] = minijuegosnombres[randomIndex];
                    minijuegosnombres[randomIndex] = temp;
                }
                if (minijuegosnombres[0] == ultimoJugado)
                {
                    int swapIndex = Random.Range(1, minijuegosnombres.Length);
                    var temp = minijuegosnombres[0];
                    minijuegosnombres[0] = minijuegosnombres[swapIndex];
                    minijuegosnombres[swapIndex] = temp;
                }
            }
        }
        proximoMinijuego = minijuegosnombres[minijuegoactual];
        minijuegoactual++;
        totalJugados++;
        SceneManager.LoadScene("Intermission");
    }

    public bool? ultimoResultado { get; private set; } = null;

    public void MinigameWon()
    {
        ultimoResultado = true;
        addScore();
        PseudoRandomLevels();
    }

    public void MinigameLost()
    {
        ultimoResultado = false;
        vidas -= 1;
        if (vidas <= 0)
            ZeroLives();
        else
            PseudoRandomLevels();
    }

    public void ZeroLives()
    {
        ultimoResultado = false;
        vidas = 0;
        SceneManager.LoadScene("Intermission");
    }

    [ContextMenu("Simular 0 Vidas")]
    void SimularCeroVidas() { vidas = 0; MinigameLost(); }

    [ContextMenu("Simular Dificultad 1")]
    void SimularDificultad1() => totalJugados = NivelesAumentosDificultad[0];

    [ContextMenu("Simular Dificultad 2")]
    void SimularDificultad2() => totalJugados = NivelesAumentosDificultad[1];

    [ContextMenu("Simular Dificultad 3")]
    void SimularDificultad3() => totalJugados = NivelesAumentosDificultad[2];

    [ContextMenu("Simular Dificultad 4")]
    void SimularDificultad4() => totalJugados = NivelesAumentosDificultad[3];
}