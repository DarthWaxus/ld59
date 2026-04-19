using System;
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

    private float[] possibleAngles = new float[] { 0f, 45f, 90f, -45f, -90f };
    
    public Transform startPoint;
    public Transform endPoint;

    public float moveDuration = 10f;

    public Slider progressSlider;

    public Worm worm;

    private void Awake()
    {
        Events.GameOverEvent.AddListener(OnGameOver);
    }

    private void OnGameOver()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        rotations = new List<List<Vector3>>();

        // генерим все комбинации для двух флагов
        foreach (float angle1 in possibleAngles)
        {
            foreach (float angle2 in possibleAngles)
            {
                rotations.Add(new List<Vector3>
                {
                    new Vector3(0f, 0f, angle1),
                    new Vector3(0f, 0f, angle2)
                });
            }
        }

        // запускаем цикл
        StartCoroutine(RotationLoop());
        StartCoroutine(MoveAlongZ());
    }

    private IEnumerator RotationLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayBetweenRotations);

            if (!rotating)
            {
                yield return StartCoroutine(RotateFlags());
            }
        }
    }

    public IEnumerator RotateFlags()
    {
        rotating = true;

        // выбираем случайную комбинацию
        var target = rotations[UnityEngine.Random.Range(0, rotations.Count)];

        Quaternion startRot1 = flag1.rotation;
        Quaternion startRot2 = flag2.rotation;

        Quaternion endRot1 = Quaternion.Euler(target[0]);
        Quaternion endRot2 = Quaternion.Euler(target[1]);

        float time = 0f;

        while (time < rotateDuration)
        {
            float t = time / rotateDuration;

            flag1.rotation = Quaternion.Lerp(startRot1, endRot1, t);
            flag2.rotation = Quaternion.Lerp(startRot2, endRot2, t);

            time += Time.deltaTime;
            yield return null;
        }

        // гарантируем точное значение
        flag1.rotation = endRot1;
        flag2.rotation = endRot2;

        rotating = false;
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