using Unity.Mathematics;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject centerOfMass;
    public float orbitSpeed = 150;
    public float orbitDistance = 1;
    public float orbitPercentage;

    void Start()
    {

    }


    void Update()
    {
        orbitPercentage += Time.deltaTime * orbitSpeed;

        if (orbitPercentage >= 360)
        {
            orbitPercentage = 0;
        }

        Vector2 position = new Vector2(centerOfMass.transform.position.x + math.cos(orbitPercentage * Mathf.Deg2Rad) * orbitDistance, centerOfMass.transform.position.y + math.sin(orbitPercentage * Mathf.Deg2Rad) * orbitDistance);
        transform.position = position;
    }
}
