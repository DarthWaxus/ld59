using System;
using System.Collections;
using UnityEngine;

public class Worm : MonoBehaviour
{
    public float moveSpeed = 10;
    private Animator anim;
    bool opened = false;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Eat()
    {
        StartCoroutine(EatRoutine());
    }
    IEnumerator EatRoutine()
    {
        while (transform.position.z > 7)
        {
            transform.Translate(
                Vector3.back * Time.deltaTime * moveSpeed,
                Space.World);

            if (transform.position.z < 40 && !opened)
            {
                opened = true;
                anim.SetBool("opened", true);
                Sound.I.Play("Eat");
            }
            
            yield return new WaitForEndOfFrame();
        }

        Events.GameOverEvent.Invoke();
    }
}