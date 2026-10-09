using UnityEngine;
using UnityEngine.Events;

// Put this on the Player AND on every enemy.
public class Health : MonoBehaviour
{
    public int maxHealth = 5;
    public float invincibleTime = 0.5f;   // damage cooldown (also protects against trap spam)

    public int Current { get; private set; }
    public UnityEvent<int, int> onHealthChanged; // current, max (HUD listens to this)
    public UnityEvent onDeath;

    float lastHitTime = -999f;
    bool dead;

    void Awake()
    {
        Current = maxHealth;
    }

    void Start()
    {
        onHealthChanged?.Invoke(Current, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (dead || Time.time < lastHitTime + invincibleTime) return;
        lastHitTime = Time.time;
        Current = Mathf.Max(0, Current - amount);
        onHealthChanged?.Invoke(Current, maxHealth);

        // quick red flash as placeholder "hurt" feedback
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) StartCoroutine(Flash(sr));

        if (Current <= 0)
        {
            dead = true;
            onDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (dead) return;
        Current = Mathf.Min(maxHealth, Current + amount);
        onHealthChanged?.Invoke(Current, maxHealth);
    }

    System.Collections.IEnumerator Flash(SpriteRenderer sr)
    {
        Color original = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = original;
    }
}
