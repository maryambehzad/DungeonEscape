using UnityEngine;
using UnityEngine.SceneManagement;

// Put on an empty GameObject called "GameManager".
// Draws the HUD and the YOU DIED / DUNGEON ESCAPED! screens (placeholder UI, no Canvas needed).
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    enum State { Playing, Won, Lost }
    State state = State.Playing;

    PlayerController player;
    Health playerHealth;

    string message = "";
    float messageUntil;

    GUIStyle hudStyle, bigStyle, buttonStyle, msgStyle;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.GetComponent<PlayerController>();
            playerHealth = p.GetComponent<Health>();
            if (playerHealth != null) playerHealth.onDeath.AddListener(Lose);
        }
    }

    public void Win()
    {
        if (state != State.Playing) return;
        state = State.Won;
        Time.timeScale = 0f;
    }

    public void Lose()
    {
        if (state != State.Playing) return;
        state = State.Lost;
        Time.timeScale = 0f;
    }

    public void ShowMessage(string text)
    {
        message = text;
        messageUntil = Time.unscaledTime + 2f;
    }

    void MakeStyles()
    {
        if (hudStyle != null) return;

        hudStyle = new GUIStyle(GUI.skin.label) { fontSize = 26, fontStyle = FontStyle.Bold };
        hudStyle.normal.textColor = Color.white;

        msgStyle = new GUIStyle(GUI.skin.label) { fontSize = 26, alignment = TextAnchor.MiddleCenter };
        msgStyle.normal.textColor = Color.yellow;

        bigStyle = new GUIStyle(GUI.skin.label) { fontSize = 64, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
        bigStyle.normal.textColor = Color.white;

        buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 28 };
    }

    void OnGUI()
    {
        MakeStyles();
        float w = Screen.width;
        float h = Screen.height;

        // HUD
        if (player != null && playerHealth != null)
        {
            GUI.Label(new Rect(20, 15, 500, 40), "Health: " + playerHealth.Current + " / " + playerHealth.maxHealth, hudStyle);
            GUI.Label(new Rect(20, 55, 500, 40), "Keys: " + player.keys, hudStyle);
            GUI.Label(new Rect(20, 95, 700, 40), "Objective: find the key, unlock the door, reach the exit", hudStyle);
        }

        // Temporary message
        if (Time.unscaledTime < messageUntil)
            GUI.Label(new Rect(0, h - 100, w, 50), message, msgStyle);

        // End screens
        if (state != State.Playing)
        {
            Color old = GUI.color;
            GUI.color = new Color(0, 0, 0, 0.7f);
            GUI.DrawTexture(new Rect(0, 0, w, h), Texture2D.whiteTexture);
            GUI.color = old;

            string title = (state == State.Won) ? "DUNGEON ESCAPED!" : "YOU DIED";
            GUI.Label(new Rect(0, h / 2 - 120, w, 100), title, bigStyle);

            if (GUI.Button(new Rect(w / 2 - 130, h / 2 + 10, 260, 60), "Restart", buttonStyle))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
