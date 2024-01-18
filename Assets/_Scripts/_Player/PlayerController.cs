using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float speed = 5f;

    private float turnSpeed = 360;

    private Vector3 input;

    void GatherInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Move()
    {
        rb.MovePosition(transform.position + (transform.forward * input.magnitude) * speed * Time.deltaTime);
    }

    void Look()
    {
        if (input != Vector3.zero)
        {
            var relative = (transform.position + input.ToIso()) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
    }

    private void Update()
    {
        GatherInput();
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }
}
