using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjectFragment : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    [SerializeField] private Vector2 _explosiveForce;
    [SerializeField] private float _torque;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _rigidBody.AddForce(_explosiveForce);
        _rigidBody.AddTorque(_torque);
        Invoke("DestroySelf", 2.5f);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
