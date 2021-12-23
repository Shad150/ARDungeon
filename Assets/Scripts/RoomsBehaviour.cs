using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsBehaviour : MonoBehaviour
{
    [SerializeField] private AudioManager _AM;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _loot;
    [SerializeField] private GameObject _newRoom;

    public bool _enemyRoom;
    public bool _roomCleared;

    void Start()
    {
        InstantiateNewRoom();
    }

    public void InstantiateNewRoom()
    {
        if(_newRoom != null)
        {
            Destroy(_newRoom.gameObject);
        }

        if (Random.Range(0, 100) >= 15)
        {
            _enemyRoom = true;
        }
        else
        {
            _enemyRoom = false;
        }

        if (_enemyRoom)
        {
            _newRoom = Instantiate(_enemy, transform.position, Quaternion.identity);
            _newRoom.transform.parent = transform;
            if (!_AM.GetComponent<AudioSource>().isPlaying)
            {
                _AM.CombatOST();
            }
            else
            {
                _AM.GetComponent<AudioSource>().Stop();
                _AM.CombatOST();
            }
        }
        else
        {
            _newRoom = Instantiate(_loot, gameObject.transform.position, Quaternion.identity);
            _newRoom.transform.parent = transform;
            if (!_AM.GetComponent<AudioSource>().isPlaying)
            {
                _AM.LootOST();
            }
            else
            { 
                _AM.GetComponent<AudioSource>().Stop();
                _AM.LootOST();
            }
        }

        Time.timeScale = 0;
        GameManager.Instance._roomStart = true;
    }
}
