using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollide : MonoBehaviour
{
    [SerializeField] private AudioSource explosionSound;
    void Start()
    {
        explosionSound = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Handle collision with player
            explosionSound.Play();
        }
    }
}
