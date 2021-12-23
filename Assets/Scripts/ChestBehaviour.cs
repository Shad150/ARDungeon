using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestBehaviour : MonoBehaviour
{
    [SerializeField] PlayerController _PC;

    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _hit;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _specialBar;
    [SerializeField] private Text _healthText;

    public int _maxHealth = 3;
    public int _currentHealth;
    //public bool _eDead;
    private int _potionsDropped = 3;


    public int _specialCount;
    public int _specialMax = 0;

    public int _damageRecieved;


    void Start()
    {
        GameManager.Instance._eDead = false;
        GameManager.Instance._lootCollected = false;
        _currentHealth = _maxHealth;
        _PC = FindObjectOfType<PlayerController>();
        _specialCount = 0;
    }

    private void Update()
    {
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            GameManager.Instance._eDead = true;
            _animator.Play("OpenChest");
            StartCoroutine(Death());
        }

        if (GameManager.Instance._eDead)
        {
            StartCoroutine(Death());
        }

        _healthBar.transform.LookAt(_PC._cam.transform.position);
        _healthBar.value = CalculateHealth();
    }

    public void EnemyDamaged()
    {
        if (!GameManager.Instance._eDead)
        {
            _animator.SetTrigger("Hit");
            Instantiate(_hit, transform.position, Quaternion.identity);
            _hit.transform.localScale = Vector3.one * 0.2f;
            _currentHealth--;
            _healthText.text = "Chest - " + _currentHealth + " / " + _maxHealth;

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                _animator.Play("OpenChest");
                GameManager.Instance._eDead = true;
                GameManager.Instance._nextRoom = true;
                StartCoroutine(Death());
            }

            _damageRecieved = 0;
        }

    }

    private IEnumerator Death()
    {
        if (!GameManager.Instance._lootCollected)
        {
            _potionsDropped = Random.Range(3, 6);
            GameManager.Instance._potionsHeld += _potionsDropped;
            GameManager.Instance._lootInRoom = _potionsDropped;
            GameManager.Instance._lootCollected = true;
        }
        

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private float CalculateHealth()
    {
        return _currentHealth / _maxHealth;
    }

}
