using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Unit unit;
    public CinemachineCamera cam;
    public float maxTiltAngle = 10f; // максимальный наклон
    public float tiltSpeed = 5f; // скорость наклона


    private float currentTilt = 0f;


    private void Awake()
    {
        unit = GetComponent<Unit>();
        Events.UnitDeadEvent.AddListener(OnDead);
    }

    public void OnMove(InputValue value)
    {
        unit.moveInput = value.Get<Vector2>();
    }

    public void OnAttack(InputValue value)
    {
        unit.Attack();
    }

    public void OnAttackAlt(InputValue value)
    {
        unit.AttackAlt();
    }

    private void Update()
    {
        TiltCamera();
    }

    private void OnDead(Unit unit)
    {
        if (unit == this.unit)
            Events.GameOverEvent.Invoke();
    }

    private void TiltCamera()
    {
        // целевой наклон (влево/вправо)
        float targetTilt = -unit.moveInput.x * maxTiltAngle;

        // плавное изменение
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);

        // применяем к камере (наклон по Z)
        cam.transform.localRotation = Quaternion.Euler(0f, 0f, currentTilt);
    }

    public void PlayHitSound()
    {
    }
}