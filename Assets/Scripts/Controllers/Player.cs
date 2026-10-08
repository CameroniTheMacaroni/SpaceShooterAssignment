using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;

    public float maxSpeed = 10;
    public float acceleration = 5;
    public float deceleration = 5;

    public Vector2 velocity;
    public Vector2 cancelMomentum;

    void Start()
    {

    }

    void Update()
    {
        playerMovement();
    }

    public void playerMovement()
    {
        if (Keyboard.current.wKey.isPressed)
        {
            velocity += acceleration * Time.deltaTime * Vector2.up;//add acceleration variable to speed with respect to time
        }
        if (Keyboard.current.dKey.isPressed)
        {
            velocity += acceleration * Time.deltaTime * Vector2.right;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            velocity += acceleration * Time.deltaTime * Vector2.down;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            velocity += acceleration * Time.deltaTime * Vector2.left;
        }

        //if theres no movement inputs
        if (Keyboard.current.wKey.isPressed == false && Keyboard.current.aKey.isPressed == false && Keyboard.current.sKey.isPressed == false && Keyboard.current.dKey.isPressed == false)
        {
            cancelMomentum = velocity.normalized;//normalize the velocity vector...

            if (velocity != Vector2.zero)//... and if the ship is still moving... 
            {
                velocity -= cancelMomentum * Time.deltaTime * deceleration;//... substact the normalized velocity vector from the velocity vector
            }
        }

        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);//don't let the ship go too fast
        transform.position += (Vector3)velocity * Time.deltaTime;//update ship position

    }
}
