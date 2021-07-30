using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Learning.Interface;

public class PlayerCombat : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] float attackDamages = 5f;

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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(_attackPoint.position, attackRange);
        foreach (Collider2D collider in hitColliders)
        {
            ITakeDamage damageTaker = collider.GetComponent<ITakeDamage>();
            if (damageTaker != null)
            {
                // decrease hp
                damageTaker.TakeDamage(attackDamages);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, attackRange);
    }
}
