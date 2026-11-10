using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialHints : MonoBehaviour
{
    public static string hintText = "";

    private Transform player;
    private PlayerController controller;
    private Vector3 startPos;
    private int step = 0;
    private float timer = 0f;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            controller = p.GetComponent<PlayerController>();
            startPos = player.position;
        }
    }

    void Update()
    {
        if (player == null) return;

        if (controller.keys > 0 && step < 4)
        {
            step = 4;
            timer = 0f;
        }

        if (step == 0 && Vector3.Distance(player.position, startPos) > 2f)
        {
            step = 1;
        }
        else if (step == 1 && Mouse.current.leftButton.wasPressedThisFrame)
        {
            step = 2;
            timer = 0f;
        }
        else if (step == 2)
        {
            timer += Time.deltaTime;
            if (timer > 5f) step = 3;
        }
        else if (step == 4)
        {
            timer += Time.deltaTime;
            if (timer > 5f) step = 5;
        }

        hintText = "";
        if (step == 0) hintText = "Move with W A S D";
        if (step == 1) hintText = "Left click to shoot an arrow at the mouse";
        if (step == 2) hintText = "Red enemies shoot back. Dodge their arrows!";
        if (step == 3) hintText = "Find the key and walk into it";
        if (step == 4) hintText = "Walk into the door to open it";
    }
}