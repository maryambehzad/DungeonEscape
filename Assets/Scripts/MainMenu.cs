using UnityEngine;
using UnityEngine.SceneManagement;

// Put on an empty GameObject in the MainMenu scene.
// Placeholder menu drawn with OnGUI (no Canvas needed).
public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "Level1";

    bool showInstructions;
    GUIStyle titleStyle, buttonStyle, textStyle;

    void Awake()
    {
        Time.timeScale = 1f;
    }

    void MakeStyles()
    {
        if (titleStyle != null) return;

        titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 72, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
        titleStyle.normal.textColor = Color.white;

        buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 30 };

        textStyle = new GUIStyle(GUI.skin.label) { fontSize = 26, alignment = TextAnchor.UpperCenter };
        textStyle.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        MakeStyles();
        float w = Screen.width;
        float h = Screen.height;

        GUI.Label(new Rect(0, h * 0.12f, w, 100), "DUNGEON ESCAPE", titleStyle);

        if (!showInstructions)
        {
            float bx = w / 2 - 150;
            float by = h * 0.40f;

            if (GUI.Button(new Rect(bx, by, 300, 70), "Play", buttonStyle))
                SceneManager.LoadScene(gameSceneName);

            if (GUI.Button(new Rect(bx, by + 90, 300, 70), "Instructions", buttonStyle))
                showInstructions = true;

            // Quit does nothing in a web build, so hide it there.
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                if (GUI.Button(new Rect(bx, by + 180, 300, 70), "Quit", buttonStyle))
                    Application.Quit();
            }
        }
        else
        {
            string text =
                "Find the key, unlock the door, and reach the exit.\n\n" +
                "W / A / S / D  -  Move\n" +
                "Mouse  -  Aim\n" +
                "Left Mouse Button  -  Attack\n\n" +
                "Enemies hurt you. If your health reaches 0, you die.";

            GUI.Label(new Rect(0, h * 0.32f, w, 300), text, textStyle);

            if (GUI.Button(new Rect(w / 2 - 150, h * 0.72f, 300, 70), "Back", buttonStyle))
                showInstructions = false;
        }
    }
}
