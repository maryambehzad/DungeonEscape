using UnityEngine;

// Player needs: Rigidbody2D (Gravity Scale 0, Freeze Rotation Z), a Collider2D, Health, tag "Player".
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Attack")]
    public int attackDamage = 1;
    public float attackRange = 1.2f;
    public float attackRadius = 0.6f;
    public float attackCooldown = 0.4f;
    public LayerMask enemyLayer;
    public GameObject attackVisual;   // optional: a child sprite that flashes when attacking

    [Header("Inventory")]
    public int keys = 0;

    Rigidbody2D rb;
    Camera cam;
    Vector2 moveInput;
    Vector2 aimDir = Vector2.down;
    float lastAttackTime = -999f;
    bool canControl = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        if (attackVisual != null) attackVisual.SetActive(false);
    }

    void Update()
    {
        if (!canControl) { moveInput = Vector2.zero; return; }

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // aim toward the mouse
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMouse = mouseWorld - (Vector2)transform.position;
        if (toMouse.sqrMagnitude > 0.01f) aimDir = toMouse.normalized;

        // rotate placeholder sprite to face the aim direction
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
            Attack();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;   // use rb.velocity on Unity 2022 and older
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        Vector2 center = (Vector2)transform.position + aimDir * attackRange;
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, attackRadius, enemyLayer);
        foreach (var hit in hits)
        {
            var h = hit.GetComponent<Health>();
            if (h != null) h.TakeDamage(attackDamage);
        }
        if (attackVisual != null) StartCoroutine(ShowAttack());
    }

    System.Collections.IEnumerator ShowAttack()
    {
        attackVisual.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        attackVisual.SetActive(false);
    }

    // Called by Health's onDeath event (hook it up in the Inspector) so the player stops moving.
    public void Freeze()
    {
        canControl = false;
        rb.linearVelocity = Vector2.zero;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (Vector3)(aimDir * attackRange), attackRadius);
    }
}
