using Unity.Mathematics;
using UnityEngine;

public class BananaParticle : MonoBehaviour
{
    public float angle;
    public float speed = 5;
    public Vector3 direction;
    public Vector3 screenWorldSize;

    public float timer;
    public float maxTimer = 2f;
    void Start()
    {
        screenWorldSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        float directionX = math.cos(angle * Mathf.Deg2Rad);
        float directionY = math.sin(angle * Mathf.Deg2Rad);

        direction = new Vector3(directionX, directionY, 0);
        direction.Normalize();
    }


    void Update()
    {
        transform.position += speed * Time.deltaTime * direction;

        if (transform.position.x > screenWorldSize.x)
        {
            direction.x *= -1;
        }
        if (transform.position.x < -screenWorldSize.x)
        {
            direction.x *= -1;
        }
        if (transform.position.y > screenWorldSize.y)
        {
            direction.y *= -1;
        }
        if (transform.position.y < -screenWorldSize.y)
        {
            direction.y *= -1;
        }

        timer += Time.deltaTime;
        if (timer > maxTimer)
        {
            Destroy(this.gameObject);
        }
    }
}
