using UnityEngine;

// Put on the Door object. Its collider must NOT be a trigger (so it blocks the player).
public class Door : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (player == null) return;

        if (player.keys > 0)
        {
            player.keys--;
            if (GameManager.Instance != null) GameManager.Instance.ShowMessage("Door unlocked!");
            Destroy(gameObject);
        }
        else
        {
            if (GameManager.Instance != null) GameManager.Instance.ShowMessage("The door is locked. Find a key.");
        }
    }
}
