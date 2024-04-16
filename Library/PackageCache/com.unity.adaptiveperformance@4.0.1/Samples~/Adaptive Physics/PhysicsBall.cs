using UnityEngine;

public class PhysicsBall : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, 1, 0) * speed, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
    }
}
