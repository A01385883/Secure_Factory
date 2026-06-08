using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    [SerializeField] private string baseApiUrl = "http://localhost:3000";

    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern string GetToken();
    #else
    private string GetToken() { return "EDITOR_DEV_MOCK_TOKEN"; }
    #endif

    private string _cachedToken;
    
    // Almacenamos el ID de la ronda aquí para que sea independiente de las escenas
    [HideInInspector] public int currentRoundId = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            #if UNITY_WEBGL && !UNITY_EDITOR
            _cachedToken = GetToken();
            try
            {
                Uri uri = new Uri(Application.absoluteURL);
                baseApiUrl = uri.GetLeftPart(UriPartial.Authority);
            }
            catch(Exception e)
            {
                Debug.LogError("No se pudo parsear la URL: " + e.Message);
            }
            #endif
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Clases públicas para asegurar compatibilidad total con JsonUtility en WebGL
    [Serializable]
    public class StartGameBody
    {
        public int game_id;
    }

    [Serializable]
    public class FinishRoundBody
    {
        public int score;
    }

    [Serializable]
    public class StartRoundResponse
    {
        public int round_id;
    }

    /// <summary>
    /// 1. Registrar inicio de ronda
    /// </summary>
    public void StartRound(Action<int> onSuccess, Action<string> onError = null)
    {
        // Enviamos "{}" como cuerpo para que express.json() no falle en el backend
        StartCoroutine(PostRequest("/api/rondas", "{}", (responseJson) =>
        {
            try
            {
                var res = JsonUtility.FromJson<StartRoundResponse>(responseJson);
                currentRoundId = res.round_id; // Se guarda aquí de forma persistente
                onSuccess?.Invoke(res.round_id);
            }
            catch (Exception ex)
            {
                onError?.Invoke("Error al deserializar round_id: " + ex.Message);
            }
        }, onError));
    }

    /// <summary>
    /// 2. Registrar el inicio de un minijuego concreto en la ronda
    /// </summary>
    public void StartGame(int roundId, int gameId, Action onSuccess = null, Action<string> onError = null)
    {
        var body = new StartGameBody { game_id = gameId };
        string json = JsonUtility.ToJson(body);
        StartCoroutine(PostRequest($"/api/rondas/{roundId}/juegos", json, (res) => onSuccess?.Invoke(), onError));
    }

    /// <summary>
    /// 3. Marcar el minijuego actual de la ronda como ganado
    /// </summary>
    public void WinGame(int roundId, int gameId, Action onSuccess = null, Action<string> onError = null)
    {
        StartCoroutine(PatchRequest($"/api/rondas/{roundId}/juegos/{gameId}/ganar", "{}", (res) => onSuccess?.Invoke(), onError));
    }

    /// <summary>
    /// 4. Finalizar la ronda enviando la puntuación acumulada
    /// </summary>
    public void FinishRound(int roundId, int finalScore, Action onSuccess = null, Action<string> onError = null)
    {
        var body = new FinishRoundBody { score = finalScore };
        string json = JsonUtility.ToJson(body);
        StartCoroutine(PatchRequest($"/api/rondas/{roundId}/finalizar", json, (res) => {
            currentRoundId = -1; // Limpiamos la ronda activa al terminar
            onSuccess?.Invoke();
        }, onError));
    }

    private IEnumerator PostRequest(string endpoint, string jsonBody, Action<string> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = new UnityWebRequest(baseApiUrl + endpoint, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            // Verificamos dinámicamente si el token ya está disponible
            string token = !string.IsNullOrEmpty(_cachedToken) ? _cachedToken : GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                onError?.Invoke($"Error POST {endpoint}: {request.error} | Response: {request.downloadHandler.text}");
            }
        }
    }

    private IEnumerator PatchRequest(string endpoint, string jsonBody, Action<string> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = new UnityWebRequest(baseApiUrl + endpoint, "PATCH"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            string token = !string.IsNullOrEmpty(_cachedToken) ? _cachedToken : GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                onError?.Invoke($"Error PATCH {endpoint}: {request.error} | Response: {request.downloadHandler.text}");
            }
        }
    }
}