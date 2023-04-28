using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarAI : MonoBehaviour
{
    public Transform player;
    public float chaseRadius = 10.0f;
    public float attackSpeed = 50.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRadius)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {

        // Calculate the direction from the enemy car to the player car
        Vector3 direction = (player.position - transform.position).normalized;

        // Move the enemy car towards the player car using Lerp
        Vector3 targetVelocity = direction * attackSpeed;
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.deltaTime * attackSpeed);

        // Rotate the enemy car to face the player car
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * attackSpeed);
    }

}
