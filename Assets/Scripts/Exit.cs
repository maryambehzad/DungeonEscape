using UnityEngine;

// Put on the Exit object. Its collider must have "Is Trigger" ticked.
public class Exit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() == null) return;
        if (GameManager.Instance != null) GameManager.Instance.Win();
    }
}
