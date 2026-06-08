using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int vidas = 3;
    public int gameTimer = 8;
    public bool debugMode = false;
    private bool gameOver = false;

    [SerializeField] int[] NivelesAumentosDificultad = { 6, 11, 17, 25 };
    [SerializeField] float[] TiempoAumentosDificultad = { 1.3f, 1.6f, 2.0f, 2.5f };
    private string[] minijuegosnombres = {"CriticAlerts","Memorama","RegisteredComponents" , "Masher", "Botones", "PurdueModel"};

    public int score { get; private set; }
    public int minijuegoactual = 0;
    public int totalJugados = 0;
    private bool primerShuffle = true;

    // Variables de seguimiento para la Base de Datos
    private int currentMinigameId = -1;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ResetearValoresNuevaRonda();
    }

    void Start()
    {
        PrepararPrimerNivel();
    }

    // Suscribirse al evento de cambio de escena de Unity para detectar minijuegos automáticamente
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Método para limpiar datos al iniciar una ronda completamente nueva
    public void ResetearValoresNuevaRonda()
    {
        vidas = 3;
        score = 0;
        totalJugados = 0;
        minijuegoactual = 0;
        gameOver = false;
        currentMinigameId = -1;
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

    public int ObtenerDificultadInt()
    {
        return Mathf.Min(Mathf.CeilToInt(totalJugados / 5f), 5);
    }

    public bool IsGameOver() => gameOver;

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

    // Método auxiliar para obtener de forma segura la ronda activa del DatabaseManager
    private int GetActiveRoundId()
    {
        return DatabaseManager.Instance != null ? DatabaseManager.Instance.currentRoundId : -1;
    }

    public void MinigameWon()
    {
        ultimoResultado = true;
        addScore();

        int roundId = GetActiveRoundId();

        // Log de Diagnóstico para la consola
        Debug.Log($"[BD] MinigameWon ejecutado. Intentando registrar victoria para la ronda {roundId} y el minijuego con ID {currentMinigameId}");

        // BD: Marcar el minijuego actual de la ronda como GANADO
        if (DatabaseManager.Instance != null && roundId > 0 && currentMinigameId > 0)
        {
            DatabaseManager.Instance.WinGame(roundId, currentMinigameId);
        }

        PseudoRandomLevels();
    }

    public void MinigameLost()
    {
        ultimoResultado = false;
        vidas -= 1;

        int roundId = GetActiveRoundId();
        Debug.Log($"[BD] MinigameLost ejecutado. Vidas restantes: {vidas}. Ronda ID: {roundId}");

        if (vidas <= 0)
            ZeroLives();
        else
            PseudoRandomLevels();
    }

    public void ZeroLives()
    {
        ultimoResultado = false;
        vidas = 0;
        gameOver = true;

        int roundId = GetActiveRoundId();

        // BD: Finalizar ronda enviando el score final acumulado
        if (DatabaseManager.Instance != null && roundId > 0)
        {
            Debug.Log($"[BD] ZeroLives ejecutado. Finalizando ronda {roundId} en la base de datos con score final: {score}");
            
            DatabaseManager.Instance.FinishRound(roundId, score, () => {
                Debug.Log("[BD] Éxito: Ronda marcada como FINALIZADA en la Base de Datos.");
            }, error => {
                Debug.LogError("[BD] Error de base de datos al finalizar ronda: " + error);
            });
        }

        SceneManager.LoadScene("Intermission");
    }

    // --- DETECCIÓN AUTOMÁTICA DE MINIJUEGOS EN ESCENA ---

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (DatabaseManager.Instance == null) return;

        int roundId = DatabaseManager.Instance.currentRoundId;
        int dificultadActual = ObtenerDificultadInt();
        int gameId = GetGameIdFromSceneNameAndDifficulty(scene.name, dificultadActual);

        // Si la escena que se cargó corresponde a un minijuego válido y hay una ronda activa
        if (gameId > 0 && roundId > 0)
        {
            currentMinigameId = gameId;

            // BD: Registrar el inicio de este minijuego en ronda_juego
            DatabaseManager.Instance.StartGame(roundId, gameId, () =>
            {
                Debug.Log($"[BD] Minijuego {scene.name} (Dificultad: {dificultadActual}, ID: {gameId}) registrado en la ronda {roundId}.");
            }, error =>
            {
                Debug.LogError("[BD] Error al registrar minijuego en la base de datos: " + error);
            });
        }
    }

    // Asocia el nombre de la escena con el ID físico del juego en la base de datos (tabla 'juego')
    private int GetGameIdFromSceneNameAndDifficulty(string sceneName, int difficulty)
    {
        switch (sceneName)
        {
            case "CriticAlerts":
                if (difficulty == 1) return 1;  
                if (difficulty == 2) return 2; 
                if (difficulty == 3) return 3;
                if (difficulty == 4) return 4;
                return 5; // dificultad 5 o superior

            case "Memorama":
                if (difficulty == 1) return 6;
                if (difficulty == 2) return 7;
                if (difficulty == 3) return 8;
                if (difficulty == 4) return 9;
                return 10;

            case "RegisteredComponents":
                if (difficulty == 1) return 11;
                if (difficulty == 2) return 12;
                if (difficulty == 3) return 13;
                if (difficulty == 4) return 14;
                return 15;

            case "Masher":
                if (difficulty == 1) return 16;
                if (difficulty == 2) return 17;
                if (difficulty == 3) return 18;
                if (difficulty == 4) return 19;
                return 20;

            case "Botones":
                if (difficulty == 1) return 21;
                if (difficulty == 2) return 22;
                if (difficulty == 3) return 23;
                if (difficulty == 4) return 24;
                return 25;

            case "PurdueModel":
                if (difficulty == 1) return 26;
                if (difficulty == 2) return 27;
                if (difficulty == 3) return 28;
                if (difficulty == 4) return 29;
                return 30;

            default:
                return -1; // No es un minijuego (ej: Intermission, PantallaInicial, Puntuaje)
        }
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
