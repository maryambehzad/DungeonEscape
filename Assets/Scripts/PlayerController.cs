using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float shootCooldown = 0.4f;
    public GameObject arrowPrefab;
    public int keys = 0;

    Rigidbody2D rb;
    Camera cam;
    Vector2 moveInput;
    Vector2 aimDir = Vector2.right;
    float lastShootTime = -999f;
    bool canControl = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        if (!canControl)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMouse = mouseWorld - (Vector2)transform.position;
        if (toMouse.sqrMagnitude > 0.01f)
        {
            aimDir = toMouse.normalized;
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= lastShootTime + shootCooldown)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void Shoot()
    {
        lastShootTime = Time.time;
        Vector3 spawnPos = transform.position + (Vector3)(aimDir * 0.6f);
        GameObject arrow = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);
        arrow.GetComponent<Arrow>().Shoot(aimDir);
    }

    public void Freeze()
    {
        canControl = false;
        rb.linearVelocity = Vector2.zero;
    }
}