using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 _initialPos, _initialRot;

    private void Awake()
    {
        Instance = this;

        _initialPos = transform.position;
        _initialRot = transform.rotation.eulerAngles;
    }

    private void OnShake(float duration, float strength)
    {
        transform.DOPunchPosition(new Vector3(strength, strength, 0), duration)
            .OnComplete(TweenBack);

        // Shake rotation only around the Z axis
        transform.DOPunchRotation(new Vector3(0, 0, strength), duration);
    }

    public static void Shake(float duration, float strength) => Instance.OnShake(duration, strength);

    private void TweenBack()
    {
        transform.DOMove(_initialPos, 0.1f);
        transform.DORotate(_initialRot, 0.1f);
    }
}
