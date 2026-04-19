using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Vector2 moveInput;
    public float moveSpeed = 5f;
    public float moveForward = 1f;
    public float moveBack = 1f;
    public Animator characterAnimator;
    public Animator horseAnimator;
    public bool dead = false;
    public int hp = 3;
    public GameObject weapon;
    public bool canFallFromHorse = false;
    public Rigidbody characterRigidbody;
    public Collider characterCollider;
    public Vector3 characterFallForce;
    public SimpleDamageFlash damageFlash;
    bool damaged = false;

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        if (moveInput.x != 0)
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveInput.x * moveSpeed, Space.World);
        }

        if (moveInput.y != 0)
        {
            transform.Translate(
                Vector3.forward * Time.deltaTime * moveInput.y * (moveInput.y > 0 ? moveForward : moveBack),
                Space.World);
        }

        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, -5, 5);
        transform.position = p;
    }

    public void Attack()
    {
        if (dead) return;
        characterAnimator.SetTrigger("attack-right");
    }

    public void AttackAlt()
    {
        if (dead) return;
        characterAnimator.SetTrigger("attack-left");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            ChangeHp(-3);
        }

        if (other.gameObject.CompareTag("Weapon") && other.gameObject != weapon)
        {
            ChangeHp(-1);
        }
    }

    public void ChangeHp(int hp)
    {
        if (dead || damaged) return;

        this.hp += hp;
        if (hp < 0) Sound.I.Play("Hurt1");
        if (this.hp <= 0)
        {
            dead = true;
            if (canFallFromHorse) FallFromHorse();
            Events.UnitDeadEvent.Invoke(this);
        }
        else
        {
            StartCoroutine(DamagedRoutine());
        }
    }

    IEnumerator DamagedRoutine()
    {
        damaged = true;
        damageFlash.Play();
        yield return new WaitForSeconds(0.3f);
        damaged = false;
    }

    public void FallFromHorse()
    {
        characterAnimator.enabled = false;
        characterCollider.enabled = true;
        characterCollider.isTrigger = false;
        characterRigidbody.isKinematic = false;
        characterRigidbody.transform.SetParent(null);
        characterRigidbody.AddForce(characterFallForce);
    }

    public void PlayFootstepSound()
    {
        if (dead) return;
        Sound.I.Play("Step1");
    }
}