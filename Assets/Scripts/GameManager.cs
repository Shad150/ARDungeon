using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }


    public int _potionsHeld = 3;
    public bool _roomStart;
    public int _roomCount = 1;
    public int _lootInRoom = 0;
    public bool _lootCollected;
    public bool _gameStarted;
    public bool _eDead;
    public bool _pDead;
    public int _playerCurrentHealth;
    public bool _nextRoom;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _potionsHeld = 3;
    }

}
