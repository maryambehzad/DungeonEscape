using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 4 || Input.anyKeyDown)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void OnGUI()
    {
        float w = Screen.width;
        float h = Screen.height;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.skin.label.fontSize = 80;
        GUI.Label(new Rect(0, h / 2 - 120, w, 100), "DUNGEON ESCAPE");
        GUI.skin.label.fontSize = 28;
        GUI.Label(new Rect(0, h / 2, w, 50), "Made by Maryam Behzad & Wesley Davis");
        GUI.Label(new Rect(0, h / 2 + 50, w, 50), "Art by Kenney.nl");
        GUI.Label(new Rect(0, h / 2 + 120, w, 50), "Press any key to continue");
    }
}