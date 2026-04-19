using System;
using System.Collections;
using UnityEngine;

public class EnemyOnHorse : MonoBehaviour
{
    private Unit unit;
    public Player player;
    public float distanceToAttack = 2;
    bool gameIsOver = false;
    public Character character;
    bool canAttack = true;
    public float timeBetweenAttacks = 2f;
    Coroutine pursueCoroutine;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        player = FindAnyObjectByType<Player>();
        Events.UnitDeadEvent.AddListener(OnUnitDead);
        Events.GameOverEvent.AddListener(OnGameOver);
    }

    private void Start()
    {
        pursueCoroutine = StartCoroutine(MoveToThePlayer());
    }

    private void OnGameOver()
    {
        gameIsOver = true;
    }

    private void OnUnitDead(Unit unit)
    {
        if (unit != this.unit) return;
        StopCoroutine(pursueCoroutine);
        StartCoroutine(MoveOutOfCamera());
    }

    IEnumerator MoveOutOfCamera()
    {
        yield return new WaitForSeconds(1);
        while (transform.position.z > -5)
        {
            unit.moveInput = Vector2.down;
            yield return new WaitForEndOfFrame();
        }

        Destroy(character.gameObject);
        Destroy(gameObject);
    }

    public IEnumerator MoveToThePlayer()
    {
        if (!player) yield break;
        while (!unit.dead && !gameIsOver)
        {
            if (player.transform.position.z > unit.transform.position.z)
            {
                unit.moveInput.y = 1;
            }
            else if (player.transform.position.z < unit.transform.position.z)
            {
                unit.moveInput.y = -1;
            } else
            {
                unit.moveInput.y = 0;
            }

            float dist = Vector3.Distance(player.transform.position, unit.transform.position);
            if (dist < distanceToAttack && canAttack)
            {
                if (player.transform.position.x > unit.transform.position.x)
                {
                    unit.Attack();
                }
                else
                {
                    unit.AttackAlt();
                }

                StartCoroutine(BetweenAttackTimer());
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator BetweenAttackTimer()
    {
        canAttack = false;
        yield return new WaitForSeconds(timeBetweenAttacks);
        canAttack = true;
    }
}