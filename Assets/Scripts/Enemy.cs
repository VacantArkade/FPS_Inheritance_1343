using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;

    float recov;

    public Rigidbody Rigidbody { get; private set; }
    Vector3 origin;
    
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        origin = transform.position;

        recov = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        

        if (agent.isStopped)
        {
            recov += Time.deltaTime;
            if (recov > 3)
            {
                agent.isStopped = false;
                recov = 0;
            }
        }

        else if (distance > 7)
        {
            Wander();
        }

        else if (distance <= 7)
        {
            Pursue(distance);
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
        Vector3 directionToPlayer = (target.position - transform.position).normalized;
        GetComponent<Rigidbody>().AddForce(directionToPlayer * 5, ForceMode.Impulse);
        Recovery();
        Debug.Log("Attack player");
    }

    public void Pursue(float distance)
    {
        agent.SetDestination(target.position);
        if (distance <= 2)
        {
            Attack();
        }
    }

    public void Recovery()
    {
        agent.isStopped = true;
    }

    public void Wander()
    {
        agent.SetDestination(origin);
    }
}
