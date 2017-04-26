using UnityEngine;
using System.Collections.Generic;

public class SimpleCharacterControl : MonoBehaviour {

    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float turnSpeed = 200;
    private float m_jumpForce = 0.2f;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private bool m_isGrounded;
    private List<Collider> m_collisions = new List<Collider>();

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for(int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider)) {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if(validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        } else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

	void Update () {
        anim.SetBool("Grounded", m_isGrounded);
        m_wasGrounded = m_isGrounded;
    }

    void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Transform camera = Camera.main.transform;

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;
        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;
        if (direction != Vector3.zero)
        {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);
            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            if (m_isGrounded)
                rb.velocity = m_currentDirection * moveSpeed * Time.deltaTime;
            anim.SetFloat("MoveSpeed", direction.magnitude);
        }
        else if (m_isGrounded)
        {
            print("vel is zero");
            rb.velocity = Vector3.zero;
        }
    }
}
