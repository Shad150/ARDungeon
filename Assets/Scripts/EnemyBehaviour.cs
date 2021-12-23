using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] PlayerController _PC;

    [SerializeField] private ParticleSystem _hit;
    [SerializeField] private Rigidbody _eBody;
    [SerializeField] private Rigidbody _eCore;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _specialBar;
    [SerializeField] private Text _healthText;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioManager _audioManager;

    public int _maxHealth = 30;
    public int _currentHealth;
    [SerializeField] private int _baseDamage = 3;
    private int _potionsDropped = 0;
    

    public int _specialCount;
    public int _specialMax = 4;
    private int _specialDamage = 5;

    private int _damageDealt;
    public int _damageRecieved;

    private int _critDmg;
    private int _critChance = 10;

    void Start()
    {
        GameManager.Instance._eDead = false;
        GameManager.Instance._lootCollected = false;
        _maxHealth = Random.Range(20, 70);
        _baseDamage = Random.Range(2, 7);
        _currentHealth = _maxHealth;
        _eBody.isKinematic = true;
        _eCore.isKinematic = true;
        _audioManager = FindObjectOfType<AudioManager>();
        _PC = FindObjectOfType<PlayerController>();
        _critDmg = _baseDamage * 2;
        _specialCount = 0;

        StartCoroutine(EnemyAttack());
    }

    private void Update()
    {
        if(_currentHealth <= 0)
        {
            _currentHealth = 0;
            _animator.enabled = false;
            GameManager.Instance._eDead = true;
            StartCoroutine(Death());
        }

        if (GameManager.Instance._eDead)
        {
            StartCoroutine(Death());
        }

        _healthBar.transform.LookAt(_PC._cam.transform.position);
        //_specialBar.transform.LookAt(_PC._cam.transform.position);
        _healthBar.value = CalculateHealth();
        _specialBar.value = CalculateSpecial();
    }

    private IEnumerator EnemyAttack()
    {
        if (!GameManager.Instance._eDead)
        {
            if (_specialCount == _specialMax) //Realiza ataque especial
            {
                _specialCount = 0;

                if (Random.Range(0, 100) <= _critChance)
                {
                    _damageDealt = _specialDamage * 2;
                }
                else
                {
                    _damageDealt = _specialDamage;
                }
                _animator.SetBool("Idle", false);
                _animator.SetBool("Special", true);
                _PC._damageRecieved = _damageDealt;
                _PC.PlayerHit();
                StartCoroutine(AttackA());
                _audioManager.SoundESpecial();
                yield return new WaitForSeconds(3f);

                StartCoroutine(EnemyAttack());

            }
            else  //Realiza ataque normal
            {
                if (Random.Range(0, 100) <= _critChance)
                {
                    _damageDealt = _critDmg;
                }
                else
                {
                    _damageDealt = _baseDamage;
                }
                _animator.SetBool("Idle", false);
                _animator.SetBool("Attack", true);
                _specialCount++;
                _PC._damageRecieved = _damageDealt;
                _PC.PlayerHit();
                _audioManager.SoundEAttack();
                StartCoroutine(AttackA());
                yield return new WaitForSeconds(3f);

                StartCoroutine(EnemyAttack());

            }
        }
        
    }

    public void EnemyDamaged()
    {
        if (!GameManager.Instance._eDead)
        {
            Instantiate(_hit, transform.position, Quaternion.identity);
            _hit.transform.localScale = Vector3.one * 0.2f;
            _currentHealth -= _damageRecieved;
            _healthText.text = "Drone - " + _currentHealth + " / " + _maxHealth;

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                GameManager.Instance._eDead = true;
                GameManager.Instance._nextRoom = true;
                if (_audioManager.GetComponent<AudioSource>().isPlaying)
                {
                    _audioManager.GetComponent<AudioSource>().Stop();
                    _audioManager.DungeonOST();
                }
                StartCoroutine(Death());
            }

            _damageRecieved = 0;
        }
        
    }

    private IEnumerator AttackA()
    {
        yield return new WaitForSeconds(2f);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Special", false);
        _animator.SetBool("Idle", true);

    }

    private IEnumerator Death()
    {

        if(Random.Range(0, 100) < 20)
        {
            if (!GameManager.Instance._lootCollected)
            {
                _potionsDropped = Random.Range(1, 3);
                GameManager.Instance._potionsHeld += _potionsDropped;
                GameManager.Instance._lootInRoom = _potionsDropped;
                GameManager.Instance._lootCollected = true;
            }
            
        }

        _eBody.isKinematic = false;
        _eCore.isKinematic = false;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private float CalculateHealth()
    {
        return _currentHealth / _maxHealth;
    }
    
    private float CalculateSpecial()
    {
        return _specialCount / _specialMax;
    }
}
