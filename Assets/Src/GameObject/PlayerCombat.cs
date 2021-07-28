using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator _animator;

    // Start is called before the first frame update
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        };
    }

    private void Attack()
    {
        // Play animation
        _animator.SetTrigger("attackTrigger");
        // Detect enimies in range of attack
        // decrease hp
    }
}
