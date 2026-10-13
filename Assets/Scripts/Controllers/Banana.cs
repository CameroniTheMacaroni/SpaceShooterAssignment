using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Banana : MonoBehaviour
{
    public float angle;
    public float speed = 5;
    public Vector3 direction;
    public Vector3 screenWorldSize;

    public GameObject particle;
    public GameObject bananaParticle;
    public GameObject parent;

    void Start()
    {
        angle = UnityEngine.Random.Range(0, 360);//generate random angle

        float directionX = math.cos(angle * Mathf.Deg2Rad);//calculate the direction of that angle
        float directionY = math.sin(angle * Mathf.Deg2Rad);

        direction = new Vector3(directionX, directionY, 0);
        direction.Normalize();

        screenWorldSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));//find the bounds of the screen

        parent = GameObject.Find("ThisIsBananas");
    }

    
    void Update()
    {
        transform.position += speed * Time.deltaTime * direction;//update position

        //if the banana hits the edge of the screen, bounce
        if(transform.position.x > screenWorldSize.x)
        {
            direction.x *= -1;
            bounce();
        }
        if(transform.position.x < -screenWorldSize.x)
        {
            direction.x *= -1;
            bounce();
        }
        if (transform.position.y > screenWorldSize.y)
        {
            direction.y *= -1;
            bounce();
        }
        if (transform.position.y < -screenWorldSize.y)
        {
            direction.y *= -1;
            bounce();
        }
    }
    public void bounce()
    {
        StartCoroutine(waitForParticles());//wait for a bit to avoid spawning too many particles by accident
    }

    IEnumerator waitForParticles()
    {
        yield return new WaitForSeconds(0.2f);
        spawnParticles(-1);
        spawnParticles(1);
    }
    public void spawnParticles(float side)
    {
        bananaParticle = Instantiate(particle, transform.position, Quaternion.identity);//spawn a new particle
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//find the angle the banana is going in

        bananaParticle.GetComponent<BananaParticle>().angle = angle + (30 * side);//give the spawned particle a slightly different angle
    }
}
