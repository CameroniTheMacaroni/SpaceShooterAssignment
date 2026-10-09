using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 direction;

    public float speed = 5;
    public float acceleration = 1;
    public float timer;
    public float despawnTime = 3;

    public float collisionDistanceAsteroid = 0.2f;
    public float collisionDistanceSun = 1;

    public List<GameObject> asteroidTransforms;
    public GameObject asteroidParent;
    public GameObject sun;

    public bool isOrbiting;
    public float orbitPercentage;
    public float orbitSpeed = 150;


    void Start()
    {
        asteroidParent = GameObject.Find("AsteroidParent");
        sun = GameObject.Find("Sun");

        for (int i = 0; i < asteroidParent.transform.childCount; i++)//populate the list with all the asteroid game objects
        {
            asteroidTransforms.Add(asteroidParent.transform.GetChild(i).gameObject);
        }
    }

    
    void Update()
    {
        if (!isOrbiting)// if the bullet isn't in orbit of the sun
        {
            transform.position += (Vector3)direction * speed * Time.deltaTime;//move the bullet forward in a straight line
            speed += acceleration * Time.deltaTime;//make it accelerate

            testForCollision();//detect collisions

            //despawn the bullet if it's been too long 
            timer += Time.deltaTime;
            if (timer > despawnTime)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            orbitPercentage += Time.deltaTime * orbitSpeed;

            if (orbitPercentage >= 360)
            {
                orbitPercentage = 0;
            }

            transform.position = sun.GetComponent<Orbit>().calculateOrbit(orbitPercentage);
        }

    }

    public void testForCollision()
    {
        for (int i = 0; i < asteroidTransforms.Count; i++)//for each of the asteroids in the list... 
        {
            if (asteroidTransforms[i] != null)
            {
                if (Vector2.Distance(transform.position, asteroidTransforms[i].transform.position) < collisionDistanceAsteroid)//... track the distance between the bullet and the asteroids, and if they've collided ... 
                {
                    //... destroy the asteroid and the bullet.
                    Destroy(asteroidTransforms[i]);
                    asteroidTransforms.RemoveAt(i);
                    Destroy(this.gameObject);
                }
            }
        }

        if (Vector2.Distance(transform.position, sun.transform.position) < 1 && !isOrbiting)// if the bullet is in range of the sun
        {
            isOrbiting = true;
            Debug.Log("works");
            sun.GetComponent<Orbit>().addToList(this.gameObject);
        }
    }
}
