using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool paused = false;

    void Update()
    {
        bool pressed = Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame;
        if (!pressed) return;

        if (paused)
        {
            Resume();
        }
        else if (Time.timeScale > 0f)
        {
            Pause();
        }
    }

    void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
    }

    void Resume()
    {
        paused = false;
        Time.timeScale = 1f;
    }

    void OnGUI()
    {
        if (!paused) return;

        float w = Screen.width;
        float h = Screen.height;

        GUI.color = new Color(0, 0, 0, 0.7f);
        GUI.DrawTexture(new Rect(0, 0, w, h), Texture2D.whiteTexture);
        GUI.color = Color.white;

        GUI.skin.label.fontSize = 64;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.contentColor = Color.white;
        GUI.Label(new Rect(0, h / 2 - 200, w, 100), "PAUSED");

        GUI.skin.button.fontSize = 28;

        if (GUI.Button(new Rect(w / 2 - 130, h / 2 - 80, 260, 60), "Resume"))
        {
            Resume();
        }

        if (GUI.Button(new Rect(w / 2 - 130, h / 2 + 0, 260, 60), "Main Menu"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }

        GUI.skin.label.fontSize = 22;
        GUI.Label(new Rect(0, h / 2 + 90, w, 50), "Press ESC or P to resume");
    }
}