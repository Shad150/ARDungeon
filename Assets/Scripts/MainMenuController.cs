using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private AudioSource _AS;
    [SerializeField] private AudioClip _drums;
    [SerializeField] private AudioClip _bamboo;
    [SerializeField] private int _bambooCount;
    [SerializeField] private GameObject _masked;
    public void ButtonClick()
    {
        _AS.PlayOneShot(_drums, 1f);
    }

    public void Bamboo()
    {
        _bambooCount++;
        _AS.PlayOneShot(_bamboo, 1.5f);

        if (_bambooCount >= 5)
        {
            _masked.GetComponent<Animator>().SetBool("Bamboo", true);
        }
        Debug.Log(_bambooCount);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SCGame()
    {
        _masked.GetComponent<Animator>().SetBool("Bamboo", false);
        _bambooCount = 0;
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene("Game");
    }
}
