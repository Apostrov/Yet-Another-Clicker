using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int[] indexBeforeNextWave = {2, 3, 5};

    public GameObject Hero;
    public GameObject[] EnemyStack;
    public GameObject CurrentEnemy;

    public Animator HeroAnim;

    public KeyCode clickKey = KeyCode.W;
    public KeyCode dodgeKey = KeyCode.S;

    public Text Money;
    public Text HeroLife;
    public Text EnemyLife;
    public Text DodgeAlert;
    
    public Text Win;
    public Text Lose;

    public GameObject shop;

    public Slider hpBar;

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
                _gameOver = true;
                Lose.gameObject.SetActive(true);
            }

            Attacks();
            hpBar.value = _enemyScript.HP() / _enemyScript.maxHp;
        }
    }

    private void Attacks()
    {
        if (!_dodged && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(clickKey)))
        {
            HeroAnim.SetBool("isAttack", true);
            float life = _enemyScript.TakeHeat(_heroScript.Attack());
            EnemyLife.text = $"Bo$$ life: {life}";
        }
        else
        {
            HeroAnim.SetBool("isAttack", false);
        }

        if (_canDodge && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(dodgeKey)))
        {
            _dodged = true;
            var position = Hero.transform.position;
            Hero.transform.position = new Vector3(position.x,  position.y - 2, position.z);
            _canDodge = false;
        }

        _time -= Time.deltaTime;
        CurrentEnemy.GetComponent<Animator>().SetFloat("time", _time);
        if (_time < 0)
        {
            StartCoroutine(WaitBeforeAttack(_enemyScript.secToDodge, _enemyScript.Attack()));
            _canDodge = true;
            // DodgeAlert.gameObject.SetActive(true);
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
            Win.gameObject.SetActive(true);
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
        // DodgeAlert.gameObject.SetActive(false);
        if (_dodged)
        {
            _dodged = false;
            var position = Hero.transform.position;
            Hero.transform.position = new Vector3(position.x,  position.y + 2, position.z);
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