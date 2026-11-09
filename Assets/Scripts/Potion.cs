using UnityEngine;

public class Potion : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().Heal(2);
            GameManager.Instance.ShowMessage("Health restored!");
            Destroy(gameObject);
        }
    }
}