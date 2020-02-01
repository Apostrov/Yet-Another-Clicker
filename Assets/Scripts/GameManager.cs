using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int[] indexBeforeNextWave = {2, 3};

    public GameObject Hero;
    public GameObject[] EnemyStack;
    public GameObject CurrentEnemy;

    public KeyCode clickKey = KeyCode.W;
    public KeyCode dodgeKey = KeyCode.S;

    public Text Money;
    public Text HeroLife;
    public Text EnemyLife;
    public Text DodgeAlert;

    public GameObject shop;

    private int enemyIndex = 0;
    private int _waveIndex = 0;
    private Hero _heroScript;
    private Enemy _enemyScript;
    private bool _gameOver = false;
    private bool _dodged = false;
    private bool _canDodge = false;


    private float _time;

    private void Awake()
    {
        _heroScript = Hero.GetComponent<Hero>();
        CurrentEnemy = Instantiate(EnemyStack[enemyIndex]);
        _enemyScript = CurrentEnemy.GetComponent<Enemy>();
        _time = _enemyScript.secBetweenAttack;

        Money.text = $"Money: {_heroScript.Money()}$";
        HeroLife.text = $"Life: {_heroScript.HP()}/{_heroScript.maxHP}";
        EnemyLife.text = $"Bo$$ life: {_enemyScript.HP()}";
    }

    private void Update()
    {
        if (!_gameOver && !shop.activeSelf)
        {
            if (_enemyScript.HP() <= 0.01f)
            {
                KillEnemy();
            }


            if (_heroScript.HP() <= 0.01f)
            {
                Debug.Log("Lose");
                _gameOver = true;
            }

            Attacks();
        }
    }

    private void Attacks()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(clickKey))
        {
            float life = _enemyScript.TakeHeat(_heroScript.Attack());
            EnemyLife.text = $"Bo$$ life: {life}";
        }

        if (_canDodge && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(dodgeKey)))
        {
            _dodged = true;
            _canDodge = false;
        }

        _time -= Time.deltaTime;
        if (_time < 0)
        {
            StartCoroutine(WaitBeforeAttack(_enemyScript.secToDodge, _enemyScript.Attack()));
            _canDodge = true;
            DodgeAlert.gameObject.SetActive(true);
            _time = _enemyScript.secBetweenAttack;
        }
    }

    private void KillEnemy()
    {
        _heroScript.AddMoney(_enemyScript.cost);
        updateMoney();

        Destroy(CurrentEnemy);
        enemyIndex++;
        if (enemyIndex >= EnemyStack.Length)
        {
            _gameOver = true;
        }
        else
        {
            if (enemyIndex >= indexBeforeNextWave[_waveIndex])
            {
                shop.SetActive(true);
                _waveIndex++;
            }
            CurrentEnemy = Instantiate(EnemyStack[enemyIndex]);
            _enemyScript = CurrentEnemy.GetComponent<Enemy>();
        }
    }

    private IEnumerator WaitBeforeAttack(float waitTime, float damage)
    {
        yield return new WaitForSeconds(waitTime);
        DodgeAlert.gameObject.SetActive(false);
        if (_dodged)
        {
            _dodged = false;
        }
        else
        {
            _heroScript.TakeHeat(damage);
            updateHp();
        }
    }

    public void updateMoney()
    {
        Money.text = $"Money: {_heroScript.Money()}$";
    }

    public void updateHp()
    {
        HeroLife.text = $"Life: {_heroScript.HP()}/{_heroScript.maxHP}";
    }
}