using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 direction;

    public float speed = 5;
    public float acceleration = 1;
    public float timer;
    public float despawnTime = 3;

    public float collisionDistance = 0.2f;

    public List<GameObject> asteroidTransforms;
    public GameObject asteroidParent;

    void Start()
    {
        asteroidParent = GameObject.Find("AsteroidParent");

        for (int i = 0; i < asteroidParent.transform.childCount; i++)
        {
            asteroidTransforms.Add(asteroidParent.transform.GetChild(i).gameObject);
        }
    }

    
    void Update()
    {
        transform.position += (Vector3) direction * speed * Time.deltaTime;
        speed += acceleration * Time.deltaTime;

        testForCollision();

        timer += Time.deltaTime;
        if (timer > despawnTime)
        {
            Destroy(this.gameObject);
        }
    }

    public void testForCollision()
    {
        for (int i = 0; i < asteroidTransforms.Count; i++)
        {
            if (asteroidTransforms[i] != null)
            {
                if (Vector2.Distance(transform.position, asteroidTransforms[i].transform.position) < collisionDistance)
                {
                    Destroy(asteroidTransforms[i]);
                    asteroidTransforms.RemoveAt(i);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
