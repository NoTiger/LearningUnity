using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Learning.Interface;

public class BreakableObject : MonoBehaviour, ITakeDamage
{
    [SerializeField] private float _health = 10;
    [SerializeField] private GameObject _breakableCopy;

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if(_health <= 0)
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        GameObject breakableObject = Instantiate(_breakableCopy);

        breakableObject.transform.position = transform.position;

        Destroy(gameObject);
    }
}
