using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public Vector3 Velocity;
    public HiveController Controller;

    public void BeeUpdate(float deltaTime)
    {
        List<Bee> influencers = FindNearbyBees(Controller.bees);
        if (influencers.Count > 0)
        {
            Cohere(influencers);
            Separate(influencers);
            Align(influencers);
        }
        CheckBounds();
        transform.Translate(Velocity * deltaTime);

    }

    private List<Bee> FindNearbyBees(List<Bee> bees)
    {
        List<Bee> found = new List<Bee>();
        foreach (Bee bee in bees)
        {
            if(bee != this && (bee.transform.position - transform.position).magnitude < Controller.SightRange)
            {
                found.Add(bee);
            }
        }
        return found;
    }

    private void Cohere(List<Bee> bees)
    {
        Vector3 c = Vector3.zero;
        foreach (Bee bee in bees)
        {
            c += bee.transform.position;
        }
        c = c / bees.Count;
        Velocity += (c - transform.position) * Controller.Cohesion;
    }

    private void Separate(List<Bee> bees)
    {
        Vector3 c = Vector3.zero;
        foreach (Bee bee in bees)
        {
            if ((bee.transform.position - transform.position).magnitude < Controller.SeparationDistance)
            {
                c -= bee.transform.position - transform.position;
            }
        }
        Velocity += c * Controller.Separation;
    }

    private void Align(List<Bee> bees)
    {
        Vector3 c = Vector3.zero;
        foreach (Bee bee in bees)
        {
            c += bee.Velocity;
        }
        c /= bees.Count;
        Velocity += (c - Velocity) * Controller.Alignment;
    }

    private void CheckBounds()
    {
        if(Mathf.Abs(transform.position.x) >= Controller.BoundsRange && Mathf.Sign(Velocity.x) == Mathf.Sign(transform.position.x))
        {
            Velocity.x *= -Controller.BoundsHitSpeedModifier;
        }
        if (Mathf.Abs(transform.position.y) >= Controller.BoundsRange && Mathf.Sign(Velocity.y) == Mathf.Sign(transform.position.y))
        {
            Velocity.y *= -Controller.BoundsHitSpeedModifier;
        }
        if (Mathf.Abs(transform.position.z) >= Controller.BoundsRange && Mathf.Sign(Velocity.z) == Mathf.Sign(transform.position.z))
        {
            Velocity.z *= -Controller.BoundsHitSpeedModifier;
        }

    }
}