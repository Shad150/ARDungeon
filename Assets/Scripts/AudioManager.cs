using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private RoomsBehaviour _RB;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _OST;
    [SerializeField] private AudioClip[] _SFX;

    private void Update()
    {
        //if (_RB._enemyRoom)
        //{
        //    _audioSource.PlayOneShot(_OST[2]);
        //}
        //else
        //{
        //    _audioSource.PlayOneShot(_OST[3]);
        //}
        //if (GameManager.Instance._eDead)
        //{
        //    _audioSource.PlayOneShot(_OST[1]);
        //}


    }
    public void MainMenuOST()
    {
        _audioSource.PlayOneShot(_OST[0]);
    }
    public void DungeonOST()
    {
        _audioSource.PlayOneShot(_OST[1]);
    }
    public void CombatOST()
    {
        _audioSource.PlayOneShot(_OST[2]);
    }
    public void LootOST()
    {
        _audioSource.PlayOneShot(_OST[3]);
    }
    

    // SFX

    public void SoundPAttack()
    {
        _audioSource.PlayOneShot(_SFX[0]);
    }
    public void SoundPSpecial()
    {
        _audioSource.PlayOneShot(_SFX[1]);
    }
    public void SoundEAttack()
    {
        _audioSource.PlayOneShot(_SFX[2]);
    }
    public void SoundESpecial()
    {
        _audioSource.PlayOneShot(_SFX[3]);
    }
    public void SoundPotion()
    {
        _audioSource.PlayOneShot(_SFX[4], 0.6f);
    }
    public void SoundLoot()
    {
        _audioSource.PlayOneShot(_SFX[5]);
    }
    public void SoundDrum()
    {
        _audioSource.PlayOneShot(_SFX[6], 1f);
    }
    public void SoundBamboo()
    {
        _audioSource.PlayOneShot(_SFX[7], 1f);
    }

}
