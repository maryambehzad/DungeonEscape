using UnityEngine;

// Put on the Key object. Its collider must have "Is Trigger" ticked.
public class Key : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player == null) return;

        player.keys++;
        if (GameManager.Instance != null) GameManager.Instance.ShowMessage("Key collected!");
        Destroy(gameObject);
    }
}
