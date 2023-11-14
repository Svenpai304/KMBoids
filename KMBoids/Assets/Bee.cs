using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee
{
    public GameObject Object;
    private Vector3 velocity;
    private HiveController controller;

    public Bee(GameObject prefab, HiveController _controller) 
    {
        Object = UnityEngine.Object.Instantiate(prefab);
        controller = _controller;
    }

    public void Update(float deltaTime)
    {
        List<Bee> influence = FindNearbyBees(controller.bees);
        if (influence.Count > 0)
        {
            Cohere(influence);
            Separate(influence);
            Align(influence);
            Object.transform.Translate(velocity * deltaTime);
        }
    }

    private List<Bee> FindNearbyBees(List<Bee> bees)
    {
        List<Bee> found = new List<Bee>();
        foreach (Bee bee in bees)
        {
            if(bee != this && (bee.Object.transform.position - Object.transform.position).magnitude < controller.SightRange)
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
            c += bee.Object.transform.position;
        }
        c = c / bees.Count;
        Object.transform.Translate((c - Object.transform.position) * controller.Cohesion);
    }

    private void Separate(List<Bee> bees)
    {
        Vector3 c = Vector3.zero;
        foreach (Bee bee in bees)
        {
            if ((bee.Object.transform.position - Object.transform.position).magnitude < controller.SeparationDistance)
            {
                c += bee.Object.transform.position - Object.transform.position;
            }
        }
        Object.transform.Translate(c * controller.Separation);
    }

    private void Align(List<Bee> bees)
    {
        Vector3 c = Vector3.zero;
        foreach (Bee bee in bees)
        {
            c += bee.velocity;
        }
        c /= bees.Count;
        velocity += (c - velocity) * controller.Alignment;
    }

}