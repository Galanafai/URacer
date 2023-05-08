using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarAI : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player"; // Tag of the player game object
    [SerializeField] private float chaseRadius = 10.0f;
    [SerializeField] private float attackForce = 15;
    [SerializeField] private float explosionForce = 50000.0f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float pushDuration = 100000.0f; // duration of the push in seconds
    [SerializeField] private AudioSource chaseSound;


    private bool isMovingTowardsTarget = false; // flag to indicate whether the enemy car is currently moving towards the target
    private Vector3 targetPosition; // position towards which the enemy car will move when it is within the target's radius

    private Rigidbody rb;
    private Transform playerTransform;

    void Start()
    {
        chaseSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("No game object with tag '" + playerTag + "' found!");
        }
    }

    void Update()
    {
        
        AttackPlayer();
    }
    void Death()
    {
        // Instantiate the explosion prefab at the position of the enemy car
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
       
        //explosionSound.Play();
        Destroy(gameObject);
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float pushDuration = 100000.0f; // duration of the push in seconds
        float pushForce = attackForce * explosionForce; // magnitude of the push force
        StartCoroutine(PushPlayer(directionToPlayer, pushDuration, pushForce));
    }
    private IEnumerator PushPlayer(Vector3 direction, float duration, float force)
    {
        Rigidbody playerRb = playerTransform.GetComponent<Rigidbody>();
        playerRb.AddExplosionForce(force, playerTransform.position, 5.0f);
        yield return new WaitForSeconds(duration);
        playerRb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle collision with player
            Death();
        }
    }


    void AttackPlayer()
    {

        if (playerTransform == null)
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToTarget <= chaseRadius)
        {
            if (!isMovingTowardsTarget)
            {
                isMovingTowardsTarget = true;
                targetPosition = playerTransform.position;
                if (!chaseSound.isPlaying)
                {
                    chaseSound.Play();
                }
            }
        }
        else
        {
            isMovingTowardsTarget = false;
            if (chaseSound.isPlaying)
            {
                chaseSound.Stop();
            }
        }

        if (isMovingTowardsTarget)
        {
           
       
            
            Vector3 playerVelocity = playerTransform.GetComponent<Rigidbody>().velocity;
            Vector3 predictedPlayerPosition = playerTransform.position + playerVelocity * (distanceToTarget / attackForce);

            Vector3 directionToTarget = (predictedPlayerPosition - transform.position).normalized;
            Vector3 directionToTargetLeft = Quaternion.AngleAxis(-90.0f, Vector3.up) * directionToTarget;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTargetLeft, Vector3.up);
            transform.rotation = targetRotation;

            Vector3 rotatedForward = Quaternion.Euler(0, 90, 0) * transform.forward;

            
            transform.position += rotatedForward * attackForce * Time.deltaTime;
            

        }
    }



}