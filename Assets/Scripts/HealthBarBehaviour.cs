using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    [SerializeField] PlayerController _PC;
    [SerializeField] EnemyBehaviour _EB;
    [SerializeField] ChestBehaviour _CB;
    [SerializeField] private Camera _cam;
    [SerializeField] private Slider _cHealthBar = null;
    [SerializeField] private Slider _healthBar  = null;
    [SerializeField] private Slider _specialBar = null;
    [SerializeField] private Slider _pSpecialBar = null;
    [SerializeField] private Slider _pHealthBar = null;

    private void Start()
    {
        _PC = FindObjectOfType<PlayerController>();
        _cam = GameObject.Find("ARCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (_CB != null)
        {
            _cHealthBar.maxValue = _CB._maxHealth;
            _cHealthBar.minValue = 0;
            _cHealthBar.value = _CB._currentHealth;
        }

        if (_EB != null)
        {
            _healthBar.maxValue = _EB._maxHealth;
            _healthBar.minValue = 0;
            _healthBar.value = _EB._currentHealth;

            _specialBar.maxValue = _EB._specialMax;
            _specialBar.minValue = 0;
            _specialBar.value = _EB._specialCount;
        }
        

        _pHealthBar.maxValue = _PC._maxHealth;
        _pHealthBar.minValue = 0;
        _pHealthBar.value = _PC._currentHealth;

        _pSpecialBar.maxValue = _PC._specialMax;
        _pSpecialBar.minValue = 0;
        _pSpecialBar.value = _PC._specialCount;
    }

    
}
