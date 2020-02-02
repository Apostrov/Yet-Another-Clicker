using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float secBetweenAttack = 2f;
    public float secToDodge = 1f;
    public float cost = 10f;
    public float maxHp;

    [SerializeField]
    private float _hp = 10f;
    [SerializeField]
    private float _damage = 5f;

    private void Awake()
    {
        maxHp = _hp;
    }


    public float TakeHeat(float damage)
    {
        _hp -= damage;
        if (_hp < 0f)
        {
            _hp = 0f;
        }
        return _hp;
    }

    public float HP()
    {
        return _hp;
    }

    public float Attack()
    {
        return _damage;
    }

}
