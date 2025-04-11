using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;

    float recov = 0;

    public Rigidbody Rigidbody { get; private set; }
    Vector3 origin;
    
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        Wander(distance);
        Pursue(distance);

        if (agent.isStopped == true)
        {
            recov += Time.deltaTime;
            if (recov > 3)
            {
                agent.isStopped = false;
                recov = 0;
            }
        }
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        GetComponent<Rigidbody>().AddForce(knockback, ForceMode.Impulse);
    }

    public void Respawn()
    {
        transform.position = origin;
    }

    public void Attack()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.forward * 3, ForceMode.Impulse);
        Recovery();
        Debug.Log("Attack player");
    }

    public void Pursue(float distance)
    {
        if(distance > 2 && distance <= 7)
        {
            agent.SetDestination(target.position);
        }
        else if (distance <= 2)
        {
            Attack();
        }
    }

    public void Recovery()
    {
        agent.isStopped = true;
    }

    public void Wander(float distance)
    {
        if (distance > 7)
        {
            agent.SetDestination(origin);
        }
    }
}
