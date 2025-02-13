using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Create-With-VR-Starter-Scene");
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        EditorApplication.isPlaying = false; // Stops Play Mode in Unity Editor
    #else
        Application.Quit(); // Exits the built game
    #endif
    }
}