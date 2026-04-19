using UnityEngine;

public class MovingWithWorld : MonoBehaviour
{
    public bool moving = false;
    public ParticleSystem dust;
    void Start()
    {
    }
    void Update()
    {
        transform.Translate(Vector3.back * Game.I.speed * Time.deltaTime);
        if (transform.position.z < -5)
        {
            Destroy(gameObject);
        }
    }
}