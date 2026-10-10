using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bullet;
    public List<Transform> asteroidTransforms;

    public GameObject spawnedBullet;

    public float maxSpeed = 10;
    public float acceleration = 5;
    public float regularDeceleration = 5;

    public Vector2 velocity;
    public Vector2 cancelMomentum;

    public Vector2 mousePos;
    public Vector2 direction2Mouse;

    public bool spinOut;
    public float spinOutTimer;
    public float spinAmount = 0.7f;
    public float spinOutMaxTimer = 2;
    public float spinOutDeceleration = 7;
    public float bananaCollisionDistance = 0.3f;
    public GameObject banana;
    public GameObject shipSpriteRenderer;

    void Start()
    {

    }

    void Update()
    {
        if (!spinOut)
        {
            playerMovement();
        }

        shoot();

        if (Vector2.Distance(transform.position, banana.transform.position) < bananaCollisionDistance || spinOut)//if the player comes in contact with the banana... 
        {
            slip();
        }
    }

    public void shoot()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            direction2Mouse = mousePos - (Vector2) transform.position;
            direction2Mouse.Normalize();

            spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            spawnedBullet.GetComponent<Bullet>().direction = direction2Mouse;
            
        }
        Debug.DrawLine(direction2Mouse, Vector2.zero, Color.magenta);

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

        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);//don't let the ship go too fast

        //if theres no movement inputs
        if (Keyboard.current.wKey.isPressed == false && Keyboard.current.aKey.isPressed == false && Keyboard.current.sKey.isPressed == false && Keyboard.current.dKey.isPressed == false)
        {
            decelerate(regularDeceleration);
        }

        transform.position += (Vector3)velocity * Time.deltaTime;//update ship position

    }

    public void decelerate(float deceleration)
    {
        cancelMomentum = velocity.normalized;//normalize the velocity vector...

        if (velocity != Vector2.zero)//... and if the ship is still moving... 
        {
            velocity -= cancelMomentum * Time.deltaTime * deceleration;//... substact the normalized velocity vector from the velocity vector
        }

    }

    public void slip()
    {
        spinOut = true;
        spinOutTimer += Time.deltaTime;

        decelerate(spinOutDeceleration);
        transform.position += (Vector3)velocity * Time.deltaTime;//update ship position

        if (spinOutTimer > spinOutMaxTimer)
        {
            spinOut = false;
            spinOutTimer = 0;
            shipSpriteRenderer.transform.eulerAngles = Vector3.zero;
        }

        shipSpriteRenderer.transform.Rotate(0, 0, (spinOutMaxTimer - spinOutTimer)*spinAmount);
    }
}
