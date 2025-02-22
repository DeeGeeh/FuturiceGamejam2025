using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.UI.Image;

public class Leafblower : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rotationOffset = 0f; // Useful for adjusting the default angle
    [SerializeField] private bool flipSprite = true; // If your sprite needs to flip when aiming left
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the sprite renderer if using flip

    [SerializeField] private VisualEffect _fog;

    [SerializeField] private float fuel = 100;
    [SerializeField] private float blowDelay = 1;
    private float blowDelayPoint = 0;

    [Header("Suction")]
    [SerializeField] private float _suctionLength = 20.0f;
    [SerializeField] private float _suctionWidth = 0.5f;
    [SerializeField] private LayerMask _layersToCheck;
    [SerializeField] private float _suctionSpeed = 2.0f;
    [SerializeField] private GameObject _suctionDebug;

    [SerializeField] private PlayerController _playerController;

    private Camera mainCamera;

    private bool _canMove = true;

    private void Start()
    {
        mainCamera = Camera.main;

        // If spriteRenderer wasn't assigned in inspector and flipSprite is true, try to get it
        if (flipSprite && spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        _suctionDebug.SetActive(false);
    }

    private void Update()
    {
        Suction();
        RotateTowardsMouse();

        _fog.SetVector3("BlowerPosition", transform.position);
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z -= 90.0f;
        _fog.SetVector3("BlowerRotation", rot);

        // Activate blower
        //if (Input.GetMouseButton(0)){
        //    blowDelayPoint += Time.deltaTime / blowDelay;
        //    if (blowDelayPoint > 1){
        //        blowDelayPoint = 1;
        //    }
        //} else {
        //    blowDelayPoint -= Time.deltaTime / blowDelay;
        //    if (blowDelayPoint < 0){
        //        blowDelayPoint = 0;
        //    }
        //}

        if (Input.GetMouseButton(0))
        {
            _fog.SetBool("BlowerOn", true);
        }
        else
        {
            _fog.SetBool("BlowerOn", false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(HitSuction());
            // HitSuction();
        }
        if (Input.GetMouseButtonUp(1))
        {
            Unsuck();
        }
    }

    private void RotateTowardsMouse()
    {
        if (!_canMove)
            return;

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

    private GameObject _sucker = null;

    private void Unsuck()
    {
        if (_sucker != null)
        {
            _sucker.GetComponent<SpriteRenderer>().color = Color.white;
            _sucker.GetComponent<Enemy>().enabled = true;
            _sucker.GetComponent<Enemy>().beingSucked = false;
        }
        _sucker = null;
        _playerController.canMove = true;
        _canMove = true;
    }

    private void Suction()
    {
        Debug.DrawRay(transform.position, transform.right * _suctionLength, Color.red);
        DebugSuctionArea();
        // _suctionDebug.SetActive(false);

        if (_sucker == null)
        {
            
            return;
        }

        // pull it in 
        // and also player can't move if sucking something in
        _sucker.transform.position = Vector2.MoveTowards(_sucker.transform.position, transform.position, _suctionSpeed * Time.deltaTime);
    }

    private void DebugSuctionArea()
    {
        //Vector2 origin = (Vector2)transform.position;
        //Vector2 direction = transform.right;
        //float width = _suctionWidth; // Use your desired width for the box
        //float height = _suctionLength; // Use your desired height for the box

        //// Draw the box (4 corners)
        //Vector2 halfExtents = new Vector2(width / 2, height / 2);

        //// Calculate the four corners of the box in world space
        //Vector2 topLeft = origin + new Vector2(-halfExtents.x, halfExtents.y);
        //Vector2 topRight = origin + new Vector2(halfExtents.x, halfExtents.y);
        //Vector2 bottomLeft = origin + new Vector2(-halfExtents.x, -halfExtents.y);
        //Vector2 bottomRight = origin + new Vector2(halfExtents.x, -halfExtents.y);

        //// Draw the debug lines to represent the box
        //Debug.DrawLine(topLeft, topRight, Color.red);
        //Debug.DrawLine(topRight, bottomRight, Color.red);
        //Debug.DrawLine(bottomRight, bottomLeft, Color.red);
        //Debug.DrawLine(bottomLeft, topLeft, Color.red);

        //// Optionally, visualize the direction by adding an offset based on the cast distance
        //Debug.DrawLine(topLeft + direction * _suctionLength, topRight + direction * _suctionLength, Color.red);
        //Debug.DrawLine(bottomLeft + direction * _suctionLength, bottomRight + direction * _suctionLength, Color.red);

    }

    private IEnumerator HitSuction()
    {
        if (_playerController.hasPrisoner)
            yield break;

        for (int i = 0; i < 5; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, _suctionLength, _layersToCheck);

            if (hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.name);
                _sucker = hit.transform.gameObject;
                hit.transform.GetComponent<SpriteRenderer>().color = Color.yellow;
                _sucker.GetComponent<Enemy>().enabled = false;
                _sucker.GetComponent<Enemy>().beingSucked = true;

                _playerController.canMove = false;
                _canMove = false;

                CameraShake.Shake(0.5f, 0.5f);
                yield break;
            }

            _suctionDebug.SetActive(true);
            yield return null;
        }
        _suctionDebug.SetActive(false);
    }
}
