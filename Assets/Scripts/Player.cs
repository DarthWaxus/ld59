using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Unit unit;
    public CinemachineCamera cam;
    public float maxTiltAngle = 10f;
    public float tiltSpeed = 5f;
    public bool inputEnabled = false;


    private float currentTilt = 0f;


    private void Awake()
    {
        unit = GetComponent<Unit>();
        Events.UnitDeadEvent.AddListener(OnDead);
    }

    public void OnMove(InputValue value)
    {
        if(!inputEnabled) return;
        unit.moveInput = value.Get<Vector2>();
    }

    public void OnAttack(InputValue value)
    {
        if(!inputEnabled) return;
        unit.Attack();
    }

    public void OnAttackAlt(InputValue value)
    {
        if(!inputEnabled) return;
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
        float targetTilt = -unit.moveInput.x * maxTiltAngle;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
        cam.transform.localRotation = Quaternion.Euler(0f, 0f, currentTilt);
    }
}