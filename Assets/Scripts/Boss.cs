using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public Transform flag1;
    public Transform flag2;

    public float rotateDuration = 0.5f;
    public float delayBetweenRotations = 2f;

    private List<List<Vector3>> rotations;
    private bool rotating = false;

    private List<float> angles = new List<float> { 90f, 45f, 0f, -45f, -90f };
    
    public Transform startPoint;
    public Transform endPoint;

    public float moveDuration = 10f;

    public Slider progressSlider;

    public Worm worm;
    
    public float obstacleDelay = 2f;
    public float enemyDelay = 3f;

    public float minDelayMultiplier = 0.5f; // ускорение к концу

    public GameObject obstaclePrefab;
    public GameObject enemyPrefab;
    
    public GameObject targetPrefab;
    public float obstacleFlyDuration = 0.7f;
    public float arcHeight = 5f;
    
    private Player player;

// занятые линии врагами
    private HashSet<float> occupiedLines = new HashSet<float>();

    private void Awake()
    {
        Events.GameOverEvent.AddListener(OnGameOver);
        Events.UnitDeadEvent.AddListener(OnUnitDead);
    }
    
    private void OnUnitDead(Unit unit)
    {
        float x = unit.transform.position.x;

        if (occupiedLines.Contains(x))
        {
            occupiedLines.Remove(x);
        }
    }
    
    private float GetDelayMultiplier()
    {
        float t = progressSlider != null ? progressSlider.value : 0f;
        return Mathf.Lerp(1f, minDelayMultiplier, t);
    }

    private void OnGameOver()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    public void Init()
    {
        StartCoroutine(Flag1Loop()); // obstacles
        StartCoroutine(Flag2Loop()); // enemies

        StartCoroutine(MoveAlongZ());
    }
    
    private IEnumerator Flag1Loop()
    {
        while (true)
        {
            float delay = obstacleDelay * GetDelayMultiplier();
            yield return new WaitForSeconds(delay);

            float angle = angles[Random.Range(0, angles.Count)];

            yield return StartCoroutine(RotateSingle(flag1, angle));

            yield return StartCoroutine(SpawnObstacleWithTelegraph());
        }
    }
    
    private IEnumerator SpawnObstacleWithTelegraph()
    {
        if (player == null) yield break;

        Vector3 targetPos = player.transform.position;
        targetPos.z += Random.Range(2,4);
        targetPos.x += Random.Range(-1,2);

        GameObject target = Instantiate(targetPrefab, targetPos, Quaternion.identity);

        yield return new WaitForSeconds(3f);

        Vector3 startPos = transform.position;
        GameObject obstacle = Instantiate(obstaclePrefab, startPos, Quaternion.identity);

        float time = 0f;

        while (time < obstacleFlyDuration)
        {
            float t = time / obstacleFlyDuration;

            Vector3 pos = Vector3.Lerp(startPos, targetPos, t);

            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;
            pos.y += height;

            obstacle.transform.position = pos;

            time += Time.deltaTime;
            yield return null;
        }

        obstacle.transform.position = targetPos;
        MovingWithWorld mww = obstacle.GetComponent<MovingWithWorld>();
        mww.moving = true;
        if(mww.dust) mww.dust.Play();
        TrailRenderer tr = obstacle.GetComponent<TrailRenderer>();
        if (tr) tr.enabled = false;

        Destroy(target);
    }
    
    private IEnumerator Flag2Loop()
    {
        while (true)
        {
            float delay = enemyDelay * GetDelayMultiplier();
            yield return new WaitForSeconds(delay);

            // фильтруем свободные линии
            List<float> available = new List<float>();

            foreach (float angle in angles)
            {
                float x = AngleToX(angle);
                if (!occupiedLines.Contains(x))
                {
                    available.Add(angle);
                }
            }

            if (available.Count == 0)
                continue;

            float rot = available[Random.Range(0, available.Count)];

            yield return StartCoroutine(RotateSingle(flag2, rot));

            float xPos = AngleToX(rot);

            SpawnEnemy(xPos);
            occupiedLines.Add(xPos);
        }
    }
    
    private float AngleToX(float angle)
    {
        if (angle == 90f) return -4f;
        if (angle == 45f) return -2f;
        if (angle == 0f) return 0f;
        if (angle == -45f) return 2f;
        if (angle == -90f) return 4f;

        return 0f;
    }
    
    private IEnumerator RotateSingle(Transform flag, float angle)
    {
        Quaternion startRot = flag.rotation;
        Quaternion endRot = Quaternion.Euler(0f, 0f, angle);

        float time = 0f;

        while (time < rotateDuration)
        {
            float t = time / rotateDuration;
            flag.rotation = Quaternion.Lerp(startRot, endRot, t);

            time += Time.deltaTime;
            yield return null;
        }

        flag.rotation = endRot;
    }
    
    private void SpawnObstacle(float x)
    {
        Vector3 pos = new Vector3(x, 0f, 50f);
        Instantiate(obstaclePrefab, pos, Quaternion.identity);
    }

    private void SpawnEnemy(float x)
    {
        Vector3 pos = new Vector3(x, 0f, -2f);
        Instantiate(enemyPrefab, pos, Quaternion.identity);
    }
   
    private IEnumerator MoveAlongZ()
    {
        float time = 0f;

        Vector3 startPos = startPoint.position;
        Vector3 endPos = endPoint.position;

        while (time < moveDuration)
        {
            float t = time / moveDuration;

            transform.position = Vector3.Lerp(startPos, endPos, t);

            // обновляем слайдер (0..1)
            if (progressSlider != null)
            {
                progressSlider.value = t;
            }

            time += Time.deltaTime;
            yield return null;
        }

        // фиксируем финальное положение
        transform.position = endPos;

        if (progressSlider != null)
        {
            progressSlider.value = 1f;
        }
        
        worm.Eat();
    }
}