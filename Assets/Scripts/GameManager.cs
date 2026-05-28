using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using JetBrains.Annotations;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float packetSpeed = 2f;
    public float packetInterval = 3f;
    private int routersPlaced = 0;
    
    public int vidas = 3;
    public int gameTimer = 8;
    [SerializeField] float MenuTime = 8f;
    private bool gameOver = false;
    private float winTimer = 0f;
    private bool countingWin = false;
    // Niveles Dificultad Mayor
    [SerializeField] int[] NivelesAumentosDificultad = { 4, 8, 14, 20};
    //Dificultad Mayor Generica
    [SerializeField] float[] TiempoAumentosDificultad = { 1.1f, 1.2f, 1.3f, 1.5f};
    private string[] minijuegosnombres = {"Correo" , "Splash"};
    [SerializeField] int score = 0;
    //Dont overthink this one
    public int i = 0;
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
[ContextMenu("Simular Subida Dificultad")]
void SimularDificultad()
{
    i = NivelesAumentosDificultad[0]; // Fuerza el primer umbral
}


public bool NotificacionDificultad()
{
    foreach (int umbral in NivelesAumentosDificultad)
    {
        if (i == umbral) return true;
    }
    return false;
}
    public float ObtenerMultiplicadorDificultad()
{
    for (int n = NivelesAumentosDificultad.Length - 1; n >= 0; n--)
    {
        if (i >= NivelesAumentosDificultad[n])
        {
            return TiempoAumentosDificultad[n];
        }
    }
    return 1f; // Sin modificador si aun no llego al primer umbral
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
            if (gameOver != true)
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
    if (i >= minijuegosnombres.Length)
    {
        i = 0;

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
    proximoMinijuego = minijuegosnombres[i];
    i++;

    SceneManager.LoadScene("Intermission");
}

public void MoreDifficult()
{
    // Aumenta dificultad segun nivel actual
    for (int n = 0; n < NivelesAumentosDificultad.Length; n++)
    {
        if (i == NivelesAumentosDificultad[n])
        {
            packetSpeed *= TiempoAumentosDificultad[n];
            packetInterval /= TiempoAumentosDificultad[n];
            break;
        }
    }
}
public bool? ultimoResultado { get; private set; } = null;

public void MinigameWon()
{
    ultimoResultado = true;
    score += 1;
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

    public bool IsGameOver() => gameOver;

    [ContextMenu("Simular 0 Vidas")]
void SimularCeroVidas()
{
    vidas = 0;
    MinigameLost();
}

[ContextMenu("Simular Dificultad 1")]
void SimularDificultad1() => i = NivelesAumentosDificultad[0];

[ContextMenu("Simular Dificultad 2")]
void SimularDificultad2() => i = NivelesAumentosDificultad[1];

[ContextMenu("Simular Dificultad 3")]
void SimularDificultad3() => i = NivelesAumentosDificultad[2];

[ContextMenu("Simular Dificultad 4")]
void SimularDificultad4() => i = NivelesAumentosDificultad[3];
    
}