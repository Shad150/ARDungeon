using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class LootBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _potion;
    [SerializeField] private GameObject _fullHeal;

    private int _potionsAmount;

    private int _common = 75;
    private int _rare = 24;
    private int _obamium = 1;

    private int _chestRarity;

    private void Start()
    {

        _chestRarity = Random.Range(0, 100);

        if (_chestRarity <= _obamium)
        {
            _potionsAmount = Random.Range(1, 3);
        }
        else if ((_chestRarity <= _rare) && (_chestRarity > _obamium))
        {
            _potionsAmount = Random.Range(2, 7);
        }
        else
        {
            
        }
    }

}
