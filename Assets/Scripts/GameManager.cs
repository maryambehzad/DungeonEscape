using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string nextLevelName = "";
    public bool isFirstLevel = false;

    static int score;
    static float totalTime;
    int scoreAtStart;
    float timeAtStart;
    int lastKeys;
    float lastHealth;
    bool won;
    bool lost;
    string message = "";
    float messageUntil;
    PlayerController player;
    Health playerHealth;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        TutorialHints.hintText = "";
    }

    void Start()
    {
        if (isFirstLevel)
        {
            score = 0;
            totalTime = 0;
        }
        scoreAtStart = score;
        timeAtStart = totalTime;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        player = p.GetComponent<PlayerController>();
        playerHealth = p.GetComponent<Health>();
        playerHealth.onDeath.AddListener(Lose);
        lastHealth = playerHealth.Current;

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy e in enemies)
        {
            e.GetComponent<Health>().onDeath.AddListener(EnemyKilled);
        }
    }

    void Update()
    {
        if (won || lost) return;
        totalTime += Time.deltaTime;

        if (player.keys > lastKeys)
        {
            score += 50;
            SoundManager.Play("key");
        }
        if (player.keys < lastKeys)
        {
            SoundManager.Play("door");
        }
        lastKeys = player.keys;

        if (playerHealth.Current < lastHealth)
        {
            SoundManager.Play("hit", 0.8f);
        }
        lastHealth = playerHealth.Current;
    }

    void EnemyKilled()
    {
        score += 10;
        SoundManager.Play("hit");
    }

    public void Win()
    {
        if (won || lost) return;
        won = true;
        SoundManager.Play("win");
        Time.timeScale = 0f;
    }

    public void Lose()
    {
        if (won || lost) return;
        lost = true;
        SoundManager.Play("lose");
        Time.timeScale = 0f;
    }

    public void ShowMessage(string text)
    {
        message = text;
        messageUntil = Time.unscaledTime + 2f;
    }

    string TimeText()
    {
        int minutes = (int)(totalTime / 60);
        int seconds = (int)(totalTime % 60);
        return minutes + ":" + seconds.ToString("00");
    }

    void OnGUI()
    {
        float w = Screen.width;
        float h = Screen.height;
        GUI.skin.label.fontSize = 26;
        GUI.skin.button.fontSize = 28;

        GUI.skin.label.alignment = TextAnchor.UpperLeft;

        GUI.contentColor = new Color(1f, 0.4f, 0.4f);
        GUI.Label(new Rect(20, 15, 500, 40), "Health: " + playerHealth.Current + " / " + playerHealth.maxHealth);

        GUI.contentColor = new Color(1f, 0.8f, 0.2f);
        GUI.Label(new Rect(20, 55, 500, 40), "Keys: " + player.keys);

        if (TutorialHints.hintText != "")
        {
            GUI.contentColor = Color.cyan;
            GUI.Label(new Rect(20, 95, w - 40, 40), TutorialHints.hintText);
        }

        GUI.skin.label.alignment = TextAnchor.UpperRight;

        GUI.contentColor = new Color(0.6f, 1f, 0.6f);
        GUI.Label(new Rect(w - 320, 15, 300, 40), "Score: " + score);

        GUI.contentColor = Color.white;
        GUI.Label(new Rect(w - 320, 55, 300, 40), "Time: " + TimeText());

        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        if (Time.unscaledTime < messageUntil)
        {
            GUI.contentColor = Color.yellow;
            GUI.Label(new Rect(0, h - 100, w, 50), message);
        }

        GUI.contentColor = Color.white;

        if (won || lost)
        {
            GUI.color = new Color(0, 0, 0, 0.7f);
            GUI.DrawTexture(new Rect(0, 0, w, h), Texture2D.whiteTexture);
            GUI.color = Color.white;

            string title = "YOU DIED";
            string buttonText = "Restart";
            if (won && nextLevelName != "")
            {
                title = "LEVEL COMPLETE!";
                buttonText = "Next Level";
            }
            else if (won)
            {
                title = "DUNGEON ESCAPED!";
                buttonText = "Main Menu";
            }

            GUI.skin.label.fontSize = 64;
            GUI.Label(new Rect(0, h / 2 - 160, w, 100), title);
            GUI.skin.label.fontSize = 26;

            if (won)
            {
                GUI.Label(new Rect(0, h / 2 - 60, w, 50), "Score: " + score + "     Time: " + TimeText());
            }

            if (GUI.Button(new Rect(w / 2 - 130, h / 2 + 10, 260, 60), buttonText))
            {
                Time.timeScale = 1f;
                if (lost)
                {
                    score = scoreAtStart;
                    totalTime = timeAtStart;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else if (nextLevelName != "")
                {
                    SceneManager.LoadScene(nextLevelName);
                }
                else
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
    }
}