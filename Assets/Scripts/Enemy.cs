using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float patrolSpeed = 1.5f;
    public float approachSpeed = 1.5f;
    public float patrolDistance = 3f;
    public float detectRange = 6f;
    public float loseRange = 9f;
    public float stopDistance = 3f;
    public float shootCooldown = 1.5f;

    private Rigidbody2D rb;
    private Transform player;
    private GameObject enemyArrow;
    private float startX;
    private float direction = 1f;
    private float shootTimer = 1f;
    private bool chasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startX = transform.position.x;
        enemyArrow = Resources.Load<GameObject>("EnemyArrow");

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }

        GetComponent<Health>().onDeath.AddListener(Die);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectRange) chasing = true;
        if (distance > loseRange) chasing = false;

        shootTimer -= Time.deltaTime;
        if (chasing && shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootCooldown;
        }
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (chasing)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance > stopDistance)
            {
                Vector2 toPlayer = (player.position - transform.position).normalized;
                rb.linearVelocity = toPlayer * approachSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            if (transform.position.x > startX + patrolDistance) direction = -1f;
            if (transform.position.x < startX - patrolDistance) direction = 1f;
            rb.linearVelocity = new Vector2(direction * patrolSpeed, 0f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!chasing)
        {
            direction = -direction;
        }
    }

    void Shoot()
    {
        if (enemyArrow == null) return;

        Vector2 aimDir = (player.position - transform.position).normalized;
        Vector3 spawnPos = transform.position + (Vector3)(aimDir * 0.8f);
        GameObject arrow = Instantiate(enemyArrow, spawnPos, Quaternion.identity);
        arrow.GetComponent<Arrow>().Shoot(aimDir);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}