using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    public GameObject exitObject;

    private int totalKeys;
    private bool allCollected = false;

    void Start()
    {
        Key[] keys = FindObjectsByType<Key>(FindObjectsSortMode.None);
        totalKeys = keys.Length;

        if (exitObject != null)
        {
            exitObject.SetActive(false);
        }

        if (totalKeys == 1)
        {
            GameManager.Instance.ShowMessage("Collect 1 key");
        }
        else
        {
            GameManager.Instance.ShowMessage("Collect " + totalKeys + " keys");
        }
    }

    void Update()
    {
        if (allCollected) return;

        Key[] keys = FindObjectsByType<Key>(FindObjectsSortMode.None);
        if (keys.Length == 0)
        {
            allCollected = true;
            if (exitObject != null)
            {
                exitObject.SetActive(true);
            }
            GameManager.Instance.ShowMessage("Find the exit door!");
        }
    }
}