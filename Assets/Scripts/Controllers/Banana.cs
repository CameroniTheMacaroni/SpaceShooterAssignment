using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Banana : MonoBehaviour
{
    public float angle;
    public float speed = 5;
    public Vector3 direction;
    public Vector3 screenWorldSize;

    public GameObject newBanana;
    public GameObject bananaParticle;
    public GameObject parent;

    void Start()
    {
        angle = UnityEngine.Random.Range(0, 360);

        float directionX = math.cos(angle * Mathf.Deg2Rad);
        float directionY = math.sin(angle * Mathf.Deg2Rad);

        direction = new Vector3(directionX, directionY, 0);
        direction.Normalize();

        screenWorldSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        parent = GameObject.Find("ThisIsBananas");
    }

    
    void Update()
    {
        transform.position += speed * Time.deltaTime * direction;

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
        StartCoroutine(waitForBananas());
    }

    IEnumerator waitForBananas()
    {
        yield return new WaitForSeconds(0.2f);
        spawnBananas(-1);
        spawnBananas(1);
    }
    public void spawnBananas(float side)
    {
        bananaParticle = Instantiate(newBanana, transform.position, Quaternion.identity);
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bananaParticle.GetComponent<BananaParticle>().angle = angle + (30 * side);
    }
}
