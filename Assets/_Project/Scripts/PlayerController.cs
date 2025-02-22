using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private bool normalizeMovement = true;
    [SerializeField] private Vector2 _playerBounds;

    [SerializeField] private VisualEffect _fog;
    [SerializeField] private VisualEffectAsset _fogAsset;

    private Vector2 moveDirection;
    private float currentSpeed;

    public bool canMove = true;


    void Update()
    {
        if (!canMove)
            return;

        HandleInput();
        Move();

        _fog.SetVector3("PlayerPosition", transform.position);
    }

    private void HandleInput()
    {
        // Get movement input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Calculate movement direction
        moveDirection = new Vector2(moveX, moveY);

        // Normalize movement if enabled (prevents faster diagonal movement)
        if (normalizeMovement && moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        // Handle sprint
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
    }

    private void Move()
    {
        // Calculate new position
        Vector3 newPosition = transform.position + new Vector3(moveDirection.x, moveDirection.y, 0) * (currentSpeed * Time.deltaTime);

        // Clamp the position within the bounds
        newPosition.x = Mathf.Clamp(newPosition.x, -_playerBounds.x, _playerBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, -_playerBounds.y, _playerBounds.y);

        // Apply the clamped position
        transform.position = newPosition;
    }
}
