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

    [SerializeField] private FuelBar fuelBar;
    [SerializeField] private float blowDelay = 1;
    private float blowDelayPoint = 0;

    [Header("Suction")]
    [SerializeField] private float _suctionLength = 20.0f;
    [SerializeField] private float _suctionWidth = 0.5f;
    [SerializeField] private LayerMask _layersToCheck;
    [SerializeField] private float _suctionSpeed = 2.0f;
    [SerializeField] private GameObject _suctionDebug;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioClip _clipSuction;
    [SerializeField] private LineRenderer _lineRenderer;

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

        // _suctionDebug.SetActive(false);
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

/*
        if (Input.GetMouseButton(0))
        {
            _fog.SetBool("BlowerOn", true);
        }
        else
        {
            _fog.SetBool("BlowerOn", false);
        }
*/
// Activate blower
        if (Input.GetMouseButton(0) && fuelBar.currentFuel > 0){
            _fog.SetBool("BlowerOn", true);
            blowDelayPoint += Time.deltaTime / blowDelay;
            if (blowDelayPoint > 1){
                blowDelayPoint = 1;
            }
            fuelBar.currentFuel -= Time.deltaTime * 2;
            if (fuelBar.currentFuel < 0){
                fuelBar.currentFuel = 0;
                _fog.SetBool("BlowerOn", false);
            }
        } else {
            blowDelayPoint -= Time.deltaTime / blowDelay;
            if (blowDelayPoint < 0){
                blowDelayPoint = 0;
            }
        }
        _fog.SetVector3("BlowerSize", new Vector3(blowDelayPoint, blowDelayPoint, blowDelayPoint));


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

        _sfxSource.Stop();
        _lineRenderer.enabled = false;
    }

    private void Suction()
    {
        Debug.DrawRay(transform.position, transform.right * _suctionLength, Color.red);
        // _suctionDebug.SetActive(false);

        if (_sucker == null)
        {
            
            return;
        }

        // pull it in 
        // and also player can't move if sucking something in
        _sucker.transform.position = Vector2.MoveTowards(_sucker.transform.position, transform.position, _suctionSpeed * Time.deltaTime);

        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, _sucker.transform.localPosition);
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

                _sfxSource.clip = _clipSuction;
                _sfxSource.Play();
                _sfxSource.loop = true;
                CameraShake.Shake(0.5f, 0.5f);

                _lineRenderer.enabled = true;
                yield break;
            }

            // _suctionDebug.SetActive(true);
            yield return null;
        }
        // _suctionDebug.SetActive(false);
    }
}
