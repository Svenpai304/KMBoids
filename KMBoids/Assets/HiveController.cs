using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveController : MonoBehaviour
{
    [Header("Setup")]
    public int BeeCount;
    public float RandomPosRange;
    public float RandomVelocityRange;
    public GameObject BeePrefab;
    [Header("Behaviour modifiers")]
    public float Cohesion;
    public float Separation;
    public float SeparationDistance;
    public float Alignment;
    public float SightRange;
    [Header("Bounds settings")]
    public float BoundsRange;
    public float BoundsHitSpeedModifier;

    [HideInInspector]public List<Bee> Bees = new List<Bee>();

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
        foreach(Bee bee in Bees)
        {
            bee.BeeUpdate(Time.fixedDeltaTime);
        }
    }
}
