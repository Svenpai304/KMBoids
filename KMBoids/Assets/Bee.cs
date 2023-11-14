using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public Vector3 Velocity;
    private Transform model;
    private HiveController controller;

    public void Setup(HiveController _controller)
    {
        controller = _controller;
        transform.position += Random.insideUnitSphere * controller.RandomPosRange;
        Velocity = Random.insideUnitSphere * controller.RandomVelocityRange;
        controller.bees.Add(this);

        model = transform.GetChild(0);
    }

    public void BeeUpdate(float deltaTime)
    {
        List<Bee> influencers = FindNearbyBees(controller.bees);
        if (influencers.Count > 0)
        {
            Cohere(influencers);
            Separate(influencers);
            Align(influencers);
        }
        CheckBounds();
        transform.Translate(Velocity * deltaTime);
        model.rotation = Quaternion.FromToRotation(Vector3.up, Velocity);
        Debug.DrawRay(transform.position, Velocity, Color.red, Time.fixedDeltaTime);
    }

    private List<Bee> FindNearbyBees(List<Bee> bees)
    {
        List<Bee> found = new List<Bee>();
        foreach (Bee bee in bees)
        {
            if(bee != this && (bee.transform.position - transform.position).magnitude < controller.SightRange)
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
        Velocity += (c - transform.position) * controller.Cohesion;
    }

    private void Separate(List<Bee> bees)
    {
        Vector3 c = Vector3.zero;
        foreach (Bee bee in bees)
        {
            if ((bee.transform.position - transform.position).magnitude < controller.SeparationDistance)
            {
                c -= (bee.transform.position - transform.position).normalized;
            }
        }
        Velocity += c * controller.Separation;
    }

    private void Align(List<Bee> bees)
    {
        Vector3 c = Vector3.zero;
        foreach (Bee bee in bees)
        {
            c += bee.Velocity;
        }
        c /= bees.Count;
        Velocity += (c - Velocity) * controller.Alignment;
    }

    private void CheckBounds()
    {
        if(Mathf.Abs(transform.position.x) >= controller.BoundsRange && Mathf.Sign(Velocity.x) == Mathf.Sign(transform.position.x))
        {
            Velocity.x *= -controller.BoundsHitSpeedModifier;
        }
        if (Mathf.Abs(transform.position.y) >= controller.BoundsRange && Mathf.Sign(Velocity.y) == Mathf.Sign(transform.position.y))
        {
            Velocity.y *= -controller.BoundsHitSpeedModifier;
        }
        if (Mathf.Abs(transform.position.z) >= controller.BoundsRange && Mathf.Sign(Velocity.z) == Mathf.Sign(transform.position.z))
        {
            Velocity.z *= -controller.BoundsHitSpeedModifier;
        }

    }
}