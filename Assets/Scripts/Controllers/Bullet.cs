using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 direction;

    public Vector2 velocity;
    public float speed;
    public float acceleration;
    void Start()
    {
        
    }

    
    void Update()
    {
        transform.position += (Vector3) direction * speed * Time.deltaTime;
    }
}
