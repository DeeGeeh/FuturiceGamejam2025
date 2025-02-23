using DG.Tweening;
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

    [SerializeField] private SpriteRenderer _playerSprite;
    private Tween _wobbleTween;
    private Vector2 _lastMoveDirection = Vector2.zero;
    private float targetYRotation = 0.0f;

    private Vector2 moveDirection;
    private float currentSpeed;

    public bool canMove = true;
    public bool hasPrisoner = false;

    [SerializeField] private SpriteRenderer _gunRenderer;

    void Update()
    {
        UpdateVisual();

        if (!canMove)
            return;

        HandleInput();
        Move();

        _fog.SetVector3("PlayerPosition", transform.position);
    }


    private void UpdateVisual()
    {
        if (hasPrisoner)
        {
            _gunRenderer.color = Color.blue;
        }
        else
        {
            _gunRenderer.color = Color.white;
        }
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

        if (moveDirection.x != _lastMoveDirection.x && moveDirection.x != 0)
        {
            FlipSprite();
            _lastMoveDirection = moveDirection;
        }
        AnimateSprite();

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

    public void ReleasePrisoner()
    {
        hasPrisoner = false;
        _gunRenderer.color = Color.white;
    }


    private void FlipSprite()
    {
        if (moveDirection.x != 0)
        {
            targetYRotation = moveDirection.x > 0 ? 0f : 180f;
            //_playerSprite.transform.DORotate(new Vector3(0, targetYRotation, 0), 0.2f);
            _playerSprite.transform.rotation = Quaternion.Euler(0, targetYRotation, 0);
        }
    }

    private void AnimateSprite()
    {
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            if (_wobbleTween == null || !_wobbleTween.IsActive() || !_wobbleTween.IsPlaying())
            {
                Vector3 rot = _playerSprite.transform.rotation.eulerAngles;
                rot.z = 5.0f;
                _wobbleTween = _playerSprite.transform.DORotate(rot, 0.1f).SetLoops(-1, LoopType.Yoyo);
            }
        }
        else
        {
            if (_wobbleTween != null && _wobbleTween.IsActive())
            {
                _wobbleTween.Kill();
            }
            _playerSprite.transform.rotation = Quaternion.Euler(0, targetYRotation, 0);
        }
    }
}
