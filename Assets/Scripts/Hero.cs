using System;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float weaponDmg = 1f;
    public float armor = 0f;
    public float maxHP = 10f;

    public String currentWeapon = "Stick";
    public String currentArmor = "Nothing";

    private float _hp = 10f;
    private float _money = 0f;

    public float HP()
    {
        return _hp;
    }

    public float Money()
    {
        return _money;
    }

    public float Attack()
    {
        return weaponDmg;
    }

    public float TakeHeat(float damage)
    {
        float realDamage = Mathf.Max(0f, damage - armor);
        _hp -= realDamage;
        if (_hp < 0f)
        {
            _hp = 0f;
        }

        return _hp;
    }

    public float AddMoney(float money)
    {
        _money += money;
        return _money;
    }

    public void ChangeWeapon(string nameStrengthCost = "1,10")
    {
        string[] split = nameStrengthCost.Split(',');
        float strength = float.Parse(split[0]);
        float cost = float.Parse(split[1]);
        if (cost <= _money)
        {
            weaponDmg += strength;
            _money -= cost;
        }
    }

    public void ChangeArmor(string nameStrengthCost = "Body,1,10")
    {
        string[] split = nameStrengthCost.Split(',');
        float newArmor = float.Parse(split[0]);
        float cost = float.Parse(split[1]);
        if (cost <= _money)
        {
            armor += newArmor;
            _money -= cost;
        }
    }

    public void Heal(string hpAndCost = "10,5")
    {
        string[] split = hpAndCost.Split(',');
        float hp = float.Parse(split[0]);
        float cost = float.Parse(split[1]);
        if (cost <= _money)
        {
            _hp += hp;
            if (_hp > maxHP)
            {
                _hp = maxHP;
            }

            _money -= cost;
        }
    }

    public void MaxHpUp(string maxhAndCost = "5,30")
    {
        string[] split = maxhAndCost.Split(',');
        float maxH = float.Parse(split[0]);
        float cost = float.Parse(split[1]);

        if (cost <= _money)
        {
            maxHP += maxH;
            _hp += maxH;
            _money -= cost;
        }
    }
}