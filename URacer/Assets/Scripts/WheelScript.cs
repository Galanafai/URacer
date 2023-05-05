using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelScript : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10.0f; // speed at which the wheel rotates

    private Rigidbody parentRb; // rigidbody component of the parent object

    void Start()
    {
        parentRb = transform.parent.GetComponent<Rigidbody>(); // get the rigidbody component of the parent object
    }

    void Update()
    {
        // only rotate the wheel if the parent object is moving
        if (parentRb.velocity.magnitude > 0.0f)
        {
            // calculate the rotation of the wheel based on the velocity of the parent object
            float rotation = Vector3.Dot(transform.up, parentRb.velocity.normalized) * Time.deltaTime * rotationSpeed;
            transform.Rotate(new Vector3(0.0f, 0.0f, rotation));
        }
    }
}
