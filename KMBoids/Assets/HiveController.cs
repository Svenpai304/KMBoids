using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveController : MonoBehaviour
{
    public int BeeCount;
    public float Cohesion;
    public float Separation;
    public float SeparationDistance;
    public float Alignment;
    public float SightRange;
    public float RandomPosRange;
    public float RandomVelocityRange;
    public float BoundsRange;
    public float BoundsHitSpeedModifier;
    public GameObject BeePrefab;

    public List<Bee> bees = new List<Bee>();

    private void Start()
    {
        for(int i = 0; i < BeeCount; i++)
        {
            Bee bee = Instantiate(BeePrefab).GetComponent<Bee>();
            bee.Setup(this);
        }
    }

    void FixedUpdate()
    {
        foreach(Bee bee in bees)
        {
            bee.BeeUpdate(Time.fixedDeltaTime);
        }
    }
}
