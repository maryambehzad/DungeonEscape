using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 12f;
    public int damage = 1;
    public bool fromPlayer = true;

    public void Shoot(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger) return;

        if (other.CompareTag("Player"))
        {
            if (fromPlayer) return;
            other.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (!fromPlayer) return;
            other.GetComponent<Health>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}