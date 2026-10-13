using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bullet;
    public List<Transform> asteroidTransforms;

    public GameObject bananasParent;
    public List<GameObject> bananas;

    public GameObject spawnedBullet;

    public float maxSpeed = 10;
    public float acceleration = 5;
    public float deceleration = 5;
    public float angularSpeed = 180f;

    public Vector3 playerInput;
    public Vector3 velocity;
    public Vector3 cancelMomentum;

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
        updateBananaList();
    }

    void Update()
    {
        if (!spinOut)//if the ship hasn't hit a banana...
        {
            //update the player's position and rotation
            playerMovement();
            playerRotation();
        }

        shoot();//test for if the player has shot

        for (int i = 0; i < bananas.Count; i++)
        {
            if (Vector2.Distance(transform.position, bananas[i].transform.position) < bananaCollisionDistance || spinOut)//if the player comes in contact with the banana... 
            {
                Debug.Log("slipping");
                slip();
            }
        }
    }

    public void updateBananaList()
    {
        for (int i = 0; i < bananasParent.transform.childCount; i++)
        {
            bananas.Add(bananasParent.transform.GetChild(i).gameObject);
        }
    }

    public void shoot()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)//if mouse clicked... 
        {
            mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());//... find mouse position, ... 

            direction2Mouse = mousePos - (Vector2) transform.position;//... calculate the direction from the player to the mouse... 
            direction2Mouse.Normalize();//... and normalize it, ... 

            spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);//... then spawn a bullet... 
            spawnedBullet.GetComponent<Bullet>().direction = direction2Mouse;//... and make it go in the direction of the mouse click
            
        }
        //Debug.DrawLine(direction2Mouse, Vector2.zero, Color.magenta);

    }

    public void playerMovement()
    {
        playerInput = Vector3.zero;

        //test for WASD pressed
        if (Keyboard.current.wKey.isPressed)
        {
            playerInput += Vector3.up;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            playerInput += Vector3.right;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            playerInput += Vector3.down;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            playerInput += Vector3.left;
        }

        if (playerInput.sqrMagnitude > 0)//if there is input
        {
            velocity += acceleration * Time.deltaTime * playerInput.normalized;//accelerate relative to input

            if (velocity.sqrMagnitude > maxSpeed * maxSpeed)//going too fast?
            {
                velocity = Vector3.ClampMagnitude(velocity, maxSpeed);//enforce speed limit
            }
        }
        else //no input detected
        {
            float deltaVelocity = deceleration * Time.deltaTime;

            if (velocity.magnitude > deltaVelocity)//if we still need to slow down
            {
                velocity -= deceleration * Time.deltaTime * velocity.normalized;//decelerate
            }
            else
            {
                velocity = Vector3.zero;//complete stop
            }
        }

        transform.position += velocity * Time.deltaTime;//update ship position relative to velocity

    }

    public void playerRotation()
    {
        if (velocity.sqrMagnitude == 0) return;//not moving, do nothing

        float deltaAngle = computeShortestAngle(transform.up, velocity);//find the angle the ship needs to rotate towards

        float rotationDirection = Mathf.Sign(deltaAngle);
        float rotationStep = angularSpeed * Time.deltaTime;

        if (rotationStep < MathF.Abs(deltaAngle))//if we still need to rotate
        {
            transform.Rotate(0, 0, rotationDirection * rotationStep);//rotate relative to angular speed
        }
        else
        {
            transform.Rotate(0, 0, deltaAngle);//lock-on
        }

    }

    public float computeShortestAngle(Vector3 a, Vector3 b)
    {
        float angleA = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg;
        float angleB = Mathf.Atan2(b.y, b.x) * Mathf.Rad2Deg;

        return Mathf.DeltaAngle(angleA, angleB);
    }

    public void decelerate(float deceleration)
    {
        cancelMomentum = velocity.normalized;//normalize the velocity vector...

        if (velocity != Vector3.zero)//... and if the ship is still moving... 
        {
            velocity -= cancelMomentum * Time.deltaTime * deceleration;//... substact the normalized velocity vector from the velocity vector
        }

    }

    public void slip()
    {
        spinOut = true;//disable player control
        spinOutTimer += Time.deltaTime;

        decelerate(spinOutDeceleration);
        transform.position += (Vector3)velocity * Time.deltaTime;//update ship position

        if (spinOutTimer > spinOutMaxTimer)//once the timer has run out... 
        {
            //... reset everything
            spinOut = false;
            spinOutTimer = 0;
            shipSpriteRenderer.transform.eulerAngles = Vector3.zero;
        }

        shipSpriteRenderer.transform.Rotate(0, 0, (spinOutMaxTimer - spinOutTimer) * spinAmount * Time.deltaTime);//make the ship's sprite rotate
    }
}
