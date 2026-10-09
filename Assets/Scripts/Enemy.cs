using UnityEngine;

// Enemy needs: Rigidbody2D (Gravity Scale 0, Freeze Rotation Z), a Collider2D, Health, layer "Enemy".
public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 3f;
    public float patrolDistance = 3f;     // how far left and right of the start point it walks

    [Header("Detection and attack")]
    public float detectRange = 5f;
    public float loseRange = 8f;          // gives up the chase beyond this distance
    public float attackRange = 0.9f;
    public int damage = 1;
    public float attackCooldown = 1f;

    Rigidbody2D rb;
    Transform player;
    Vector2 leftPoint, rightPoint, patrolTarget;
    bool chasing;
    float lastAttackTime = -999f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        var h = GetComponent<Health>();
        if (h != null) h.onDeath.AddListener(Die);
    }

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        Vector2 start = transform.position;
        leftPoint = start + Vector2.left * patrolDistance;
        rightPoint = start + Vector2.right * patrolDistance;
        patrolTarget = rightPoint;
    }

    void FixedUpdate()
    {
        if (player == null) { rb.linearVelocity = Vector2.zero; return; }

        float dist = Vector2.Distance(transform.position, player.position);

        if (!chasing && dist <= detectRange) chasing = true;
        else if (chasing && dist > loseRange) chasing = false;

        if (chasing)
        {
            if (dist <= attackRange)
            {
                rb.linearVelocity = Vector2.zero;
                TryAttack();
            }
            else
            {
                MoveToward(player.position, chaseSpeed);
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        MoveToward(patrolTarget, patrolSpeed);
        if (Vector2.Distance(transform.position, patrolTarget) < 0.2f)
            patrolTarget = (patrolTarget == rightPoint) ? leftPoint : rightPoint;
    }

    void MoveToward(Vector2 target, float speed)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * speed;
    }

    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        lastAttackTime = Time.time;
        var h = player.GetComponent<Health>();
        if (h != null) h.TakeDamage(damage);
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
