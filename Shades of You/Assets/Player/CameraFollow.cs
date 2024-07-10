using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform;

    [Header("Flip Rotation Stats")]
    [SerializeField] private float _flipYRotationTime = 0.05f;

    private Coroutine _turnCoroutine;

    private PlayerMovement _player;

    private bool _isFacingRight;

    private void Awake()
    {
        _player = _playerTransform.gameObject.GetComponent<PlayerMovement>();

        _isFacingRight = _player.isFacingRight;
    }

    private void Update()
    {
        transform.position = _playerTransform.position;
    }

    public void CallTurn()
    {
        float endRotation = DetermineEndRotation();
        transform.DORotate(new Vector3(0, endRotation, 0), _flipYRotationTime, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .OnComplete(() => _isFacingRight = !_isFacingRight);
    }

    // private IEnumerator FlipYLerp()
    // {
    //     float startRotation = transform.localEulerAngles.y;
    //     float endRotationAmount = DetermineEndRotation();
    //     float yRotation = 0f;

    //     float elapsedTime = 0f;

    //     while (elapsedTime < _flipYRotationTime)
    //     {
    //         elapsedTime += Time.deltaTime;

    //         yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / _flipYRotationTime));

    //         transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    //         yield return null;
    //     }

    // }

    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;
        
        if (_isFacingRight)
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }
        
}
