using NUnit.Framework;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject centerOfMass;
    public float orbitDistance = 1;

    public List<GameObject> thingsOrbiting;

    void Start()
    {

    }


    void Update()
    {


        //calculateOrbit();
    }

    public Vector2 calculateOrbit(float orbitPercentage)
    {

        float distanceX = math.cos(orbitPercentage * Mathf.Deg2Rad);
        float distanceY = math.sin(orbitPercentage * Mathf.Deg2Rad);
        Vector2 position = new Vector2(this.transform.position.x + distanceX * orbitDistance, this.transform.position.y + distanceY * orbitDistance);

        return position;
    }

    public void addToList(GameObject bullet)
    {
        thingsOrbiting.Add(bullet);

        for (int i = 0; i < thingsOrbiting.Count; ++i)
        {
            float jumps = 360 / thingsOrbiting.Count;
            thingsOrbiting[i].GetComponent<Bullet>().orbitPercentage = jumps * i;
        }
    }
}
