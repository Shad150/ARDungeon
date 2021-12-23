using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private EnemyBehaviour _enemyBehaviour;
    [SerializeField] private ChestBehaviour _chestBehaviour;
    [SerializeField] private RoomsBehaviour _roomBehaviour;
    [SerializeField] private Slider _pHealthBar;
    [SerializeField] private Text _pHealthBarText;
    [SerializeField] private Slider _pSpecialBar;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _pHit;
    [SerializeField] private AudioManager _audioSource;
    public Camera _cam;

    public int _currentHealth;
    public int _maxHealth = 50;
    [SerializeField] private int _baseDamage = 4;

    private bool _attacking;

    public int _specialCount;
    public int _specialMax = 6;
    private int _specialDamage = 10;

    private int _critDmg;
    private int _critChance = 10;

    public int _damageRecieved;
    public int _pDamageDealt;


    void Start()
    {
        Debug.Log(GameManager.Instance._playerCurrentHealth);

        if (!GameManager.Instance._gameStarted)
        {
            _currentHealth = _maxHealth;
            GameManager.Instance._playerCurrentHealth = _currentHealth;
            GameManager.Instance._gameStarted = true;
            Debug.Log("!gameStarted - " + _currentHealth);

        }
        else
        {
            _currentHealth = GameManager.Instance._playerCurrentHealth;

        }
        _audioSource = FindObjectOfType<AudioManager>();
        _roomBehaviour = FindObjectOfType<RoomsBehaviour>();
        _cam = GameObject.Find("ARCamera").GetComponent<Camera>();

        if (_roomBehaviour._enemyRoom) 
        {
            _chestBehaviour = null;
        }
        else
        {
            _enemyBehaviour = null;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_roomBehaviour._enemyRoom)
            {
                if (!GameManager.Instance._eDead)
                {
                    if (!_attacking)
                    {
                        _baseDamage = Random.Range(3, 5);
                        StartCoroutine(PlayerAttack());
                    }
                }
            }
            else
            {
                if (!GameManager.Instance._eDead)
                {
                    if (!_attacking)
                    {
                        _baseDamage = Random.Range(3, 5);
                        StartCoroutine(PlayerAttack());
                    }
                }
            }
            

        }

        _pHealthBar.transform.LookAt(_cam.transform.position);
        _pHealthBar.value = CalculateHealth();
        _pSpecialBar.value = CalculateSpecial();

    }

    public IEnumerator PlayerAttack()
    {

        if (!_attacking)
        {
            _attacking = true;

            if (_specialCount == _specialMax) //Realiza ataque especial
            {
                

                if (Random.Range(0, 100) <= _critChance)
                {
                    _pDamageDealt = _specialDamage * 2;
                }
                else
                {
                    _pDamageDealt = _specialDamage;
                }
                _animator.SetBool("SpecialAttack", true);

                yield return new WaitForSeconds(1.5f);
                if (_roomBehaviour._enemyRoom)
                {
                    _enemyBehaviour._damageRecieved = _pDamageDealt;
                    _enemyBehaviour.EnemyDamaged();
                }
                else
                {
                    _chestBehaviour._damageRecieved = _pDamageDealt;
                    _chestBehaviour.EnemyDamaged();
                }
                
                _attacking = false;
                _audioSource.SoundPSpecial();
                _specialCount = 0;
                _animator.SetBool("SpecialAttack", false);

            }
            else  //Realiza ataque normal
            {
                if (Random.Range(0, 100) <= _critChance)
                {
                    _pDamageDealt = _critDmg;
                }
                else
                {
                    _pDamageDealt = _baseDamage;
                }
                _specialCount++;

                _animator.SetBool("Attack", true);

                yield return new WaitForSeconds(1.5f);
                if (_roomBehaviour._enemyRoom)
                {
                    _enemyBehaviour._damageRecieved = _pDamageDealt;
                    _enemyBehaviour.EnemyDamaged();
                }
                else
                {
                    _chestBehaviour._damageRecieved = _pDamageDealt;
                    _chestBehaviour.EnemyDamaged();
                }
                
                _attacking = false;
                _audioSource.SoundPAttack();

                _animator.SetBool("Attack", false);

            }
        }
        
    }

    public void PlayerHit()
    {


        if (_currentHealth > 0)
        {
            Instantiate(_pHit, transform.position, Quaternion.identity);
            _pHit.transform.localScale = Vector3.one * 0.2f;
            _currentHealth -= _damageRecieved;
            GameManager.Instance._playerCurrentHealth = _currentHealth;
            _pHealthBarText.text = "HP - " + _currentHealth + " / " + _maxHealth;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                GameManager.Instance._playerCurrentHealth = _currentHealth;
                StartCoroutine(PlayerDead());
            }
        }

    }

    public void UsePotion()
    {
        if (GameManager.Instance._potionsHeld > 0)
        {
            _audioSource.SoundPotion();
            GameManager.Instance._potionsHeld--;
            _currentHealth += 6;
            GameManager.Instance._playerCurrentHealth = _currentHealth;

            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
                GameManager.Instance._playerCurrentHealth = _currentHealth;
            }
        }
    }

    private float CalculateHealth()
    {
        return _currentHealth / _maxHealth;
    }

    private float CalculateSpecial()
    {
        return _specialCount / _specialMax;
    }

    IEnumerator PlayerDead()
    {
        _animator.SetBool("Death", true);
        yield return new WaitForSeconds(5f);
        GameManager.Instance._pDead = true;
        Time.timeScale = 0;
    }

}
