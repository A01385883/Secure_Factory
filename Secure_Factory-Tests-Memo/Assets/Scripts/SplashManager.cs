using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{
    public string nextScene = "MainMenu";
    public float duration = 3f;
    public Slider loadingBar;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (loadingBar != null)
        {
            loadingBar.value = timer / duration;
        }

        if (timer >= duration)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}