using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;

    void Start()
    {
        // Add onClick listeners to the buttons
        button1.onClick.AddListener(() => LoadLevel("AlphaScene"));
        button2.onClick.AddListener(() => LoadLevel("DrewAlphaCopy"));
        button3.onClick.AddListener(() => LoadLevel("ConnorAlphaCopy"));
    }

    void LoadLevel(string levelName)
    {
        if (!string.IsNullOrEmpty(levelName))
        {
            SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.LogError("Level name is not set.");
        }
    }
}
