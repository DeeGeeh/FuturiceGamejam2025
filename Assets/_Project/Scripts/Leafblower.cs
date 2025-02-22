using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Leafblower : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rotationOffset = 0f; // Useful for adjusting the default angle
    [SerializeField] private bool flipSprite = true; // If your sprite needs to flip when aiming left
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the sprite renderer if using flip

    [SerializeField] private VisualEffect _fog;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        // If spriteRenderer wasn't assigned in inspector and flipSprite is true, try to get it
        if (flipSprite && spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        RotateTowardsMouse();

        _fog.SetVector3("BlowerPosition", transform.position);
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z -= 90.0f;
        _fog.SetVector3("BlowerRotation", rot);

        // Activate blower
        if (Input.GetMouseButton(0)){
            _fog.SetFloat("BlowerPower", (float)0.9);
            _fog.SetVector3("BlowerSize", new Vector3((float)1.0, (float)1.0, (float)1.0));
        } else {
            _fog.SetFloat("BlowerPower", (float)0);
            _fog.SetVector3("BlowerSize", new Vector3((float)0, (float)0, (float)0));
        }
    }

    private void RotateTowardsMouse()
    {
        // Get mouse position in world space
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Calculate the direction from object to mouse
        Vector2 direction = mousePosition - transform.position;

        // Calculate the angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation with offset
        transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);

        // Flip sprite if enabled (useful for weapons that should flip when aiming left)
        if (flipSprite && spriteRenderer != null)
        {
            spriteRenderer.flipY = (angle > 90 || angle < -90);
        }
    }
}
