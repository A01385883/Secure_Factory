using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using JetBrains.Annotations;
using System.Linq;


public class GameManager : MonoBehaviour
{
    public int difficulty = 1;
    public bool gameOver = false;
    public bool debugMode = false;
    public float timer = 60f;
    public bool paused = false;

    public static GameManager Instance { get; private set; }

    public float packetSpeed = 2f;
    public float packetInterval = 3f;
    private int routersPlaced = 0;
    
    public int vidas = 3;
    public int gameTimer = 8;
    [SerializeField] float MenuTime = 8f;

    private float winTimer = 0f;
    private bool countingWin = false;
    // Niveles Dificultad Mayor
    [SerializeField] int[] NivelesAumentosDificultad = { 4, 8, 14, 20};
    //Dificultad Mayor Generica
    [SerializeField] float[] TiempoAumentosDificultad = { 1.1f, 1.2f, 1.3f, 1.5f};
    private string[] minijuegosnombres = {"Minijuego_Correo" , "Splash"};
    // En GameManager
    public int score { get; private set; }
    //Dont overthink this one
    private int i = 0;
    public int minijuegoactual = 0;
    public int totalJugados = 0;  
    //Buh
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

    if (vidas != 3 | score != 0)
    {
        vidas = 3;
        score = 0;
    }
}


public void addScore()
    {
        //Lo dejo aca para que sea mas escalable
        score += 100;
    }
void Start()
{
    PrepararPrimerNivel();
    
}

void PrepararPrimerNivel()
{
    proximoMinijuego = minijuegosnombres[0];
    minijuegoactual = 1; // El siguiente será el índice 1
}

public bool NotificacionDificultad()
{
    foreach (int umbral in NivelesAumentosDificultad)
    {
        if (totalJugados == umbral) return true;
    }
    return false;
}

public float ObtenerMultiplicadorDificultad()
{
    for (int n = NivelesAumentosDificultad.Length - 1; n >= 0; n--)
    {
        if (totalJugados >= NivelesAumentosDificultad[n])
            return TiempoAumentosDificultad[n];
    }
    return 1f;
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

    void Update()
    {
        if (countingWin)
        {
            winTimer += Time.deltaTime;
            if (winTimer >= 2f) EndGame(true);
        }
        
        //Barra de Tiempo de Cada Nivel
        //TIME???? YOU MELOOOOONS
        if (gameTimer <= 0)
        {
            if (gameOver == true )
            {
                MinigameLost();
            }
            else
            {
                MinigameWon();
            }
        }
    }

//Gano

//Perdio Minijuego, aun tiene vidas (Incluso si es 1 que aun debemos elminiar)
  //Perdio Minijuego, Sin Vidas
public void ZeroLives()
{
    ultimoResultado = false;
    vidas = 0; // Mantiene 0 para que Intermission muestre "Too Bad..."
    SceneManager.LoadScene("Intermission");
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
            // Aplicar dificultad al shufflear
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

    // Guarda el proximo minijuego pero carga Intermission
    UnityEngine.Debug.Log($"minijuegoactual: {minijuegoactual}, largo: {minijuegosnombres.Length}");
    proximoMinijuego = minijuegosnombres[minijuegoactual];
    minijuegoactual++;
    totalJugados++;

    SceneManager.LoadScene("Intermission");
}



public bool? ultimoResultado { get; private set; } = null;

public void MinigameWon()
{
    UnityEngine.Debug.Log("MinigameWon llamado");
    ultimoResultado = true;
    addScore();
    PseudoRandomLevels();
}

public void MinigameLost()
{
    UnityEngine.Debug.Log($"MinigameLost llamado, vidas: {vidas}");
    ultimoResultado = false;
    vidas -= 1;
    if (vidas <= 0)
        ZeroLives();
    else
        PseudoRandomLevels();
}
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

    public int ObtenerNivelDificultad()
    {
    for (int n = NivelesAumentosDificultad.Length - 1; n >= 0; n--)
        if (totalJugados >= NivelesAumentosDificultad[n]) return n + 2;
    return 1;
    }

    
    public bool IsGameOver() => gameOver;

[ContextMenu("Simular 0 Vidas")]
void SimularCeroVidas()
{
    vidas = 0;
    MinigameLost();
}

[ContextMenu("Simular Dificultad 1")]
void SimularDificultad1() => totalJugados = NivelesAumentosDificultad[0];

[ContextMenu("Simular Dificultad 2")]
void SimularDificultad2() => totalJugados = NivelesAumentosDificultad[1];

[ContextMenu("Simular Dificultad 3")]
void SimularDificultad3() => totalJugados = NivelesAumentosDificultad[2];

[ContextMenu("Simular Dificultad 4")]
void SimularDificultad4() => totalJugados = NivelesAumentosDificultad[3];

[ContextMenu("Simular Subida Dificultad")]
void SimularDificultad()
{
    totalJugados = NivelesAumentosDificultad[0]; // Fuerza el primer umbral
}

}

