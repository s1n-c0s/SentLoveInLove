using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetting : MonoBehaviour
{
    private int currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ResetApp();
        }
    }

    public void ResetApp()
    {
        // Reload the current active scene
        SceneManager.LoadScene(currentSceneIndex);
    }
}

