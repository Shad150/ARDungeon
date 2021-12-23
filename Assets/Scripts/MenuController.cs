using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private RoomsBehaviour _RB;
    [SerializeField] private PlayerController _PC;
    [SerializeField] private AudioManager _AM;

    [SerializeField] private GameObject _startRoom;
    [SerializeField] private GameObject _potionButton;
    [SerializeField] private GameObject _transitionPanel;
    [SerializeField] private Text _potionsText;
    [SerializeField] private Text _lootedPotions;
    [SerializeField] private Text _roomCount;
    [SerializeField] private GameObject _nextRoomButton;
    [SerializeField] private ParticleSystem _transitionSmoke;
    [SerializeField] private GameObject _pDeadMenu;

    public Text _potionsHeldText;
    private bool _transition;

    private void Start()
    {
        _pDeadMenu.SetActive(false);
        _lootedPotions.GetComponent<Animator>().enabled = false;
        _roomCount.text = "Room: " + GameManager.Instance._roomCount;
        _nextRoomButton.SetActive(false);
        _RB = FindObjectOfType<RoomsBehaviour>();
        _PC = FindObjectOfType<PlayerController>();
        _transitionPanel.SetActive(false);
    }

    private void Update()
    {
        if(!_PC)
        {
            _PC = FindObjectOfType<PlayerController>();
        }

        if (GameManager.Instance._roomStart)
        {
            _startRoom.SetActive(true);
        }

        if (_transition)
        {
            _transitionSmoke.Play();
        }
        else
        {
            _transitionSmoke.Stop();
        }

        if (GameManager.Instance._eDead)
        {
            if (GameManager.Instance._nextRoom)
            {
                _nextRoomButton.SetActive(true);
                GameManager.Instance._nextRoom = false;
            }
        }

        if (GameManager.Instance._lootCollected)
        {
            _lootedPotions.text = "+" + GameManager.Instance._lootInRoom;
            StartCoroutine(PotionsLooted());
        }

        _potionsText.text = GameManager.Instance._potionsHeld.ToString();

        if (GameManager.Instance._pDead)
        {
            _pDeadMenu.SetActive(true);
        }
    }

    public void UsePotion()
    {

        if(GameManager.Instance._potionsHeld > 0)
        {
            _potionButton.GetComponent<Image>().color = Color.white;
            _PC.UsePotion();
        }
        else
        {
            _potionButton.GetComponent<Image>().color = Color.grey;
        }
    }

    public void NextRoom()
    {
        _AM.SoundDrum();
        _nextRoomButton.gameObject.SetActive(false);
        if (!_transition)
        {

            GameManager.Instance._roomCount++;
            StartCoroutine(NextRoomTransition());
        }
    }

    public void StartRoom()
    {
        GameManager.Instance._roomStart = false;
        Time.timeScale = 1;
        _startRoom.SetActive(false);
    }

    public IEnumerator NextRoomTransition()
    {
        _transitionPanel.SetActive(true);
        _transition = true;
        _transitionPanel.GetComponent<Animator>().Play("RoomTransition");
        _AM.SoundBamboo();
        yield return new WaitForSeconds(1.5f);
        _transitionPanel.GetComponent<Animator>().Play("RoomTransitionEnd");
        _roomCount.text = "Room: " + GameManager.Instance._roomCount;
        _AM.SoundBamboo();
        yield return new WaitForSeconds(1.5f);
        _RB.InstantiateNewRoom();
        _RB = FindObjectOfType<RoomsBehaviour>();
        _PC = FindObjectOfType<PlayerController>();
        _transitionPanel.SetActive(false);
        _transition = false;
    }

    private IEnumerator PotionsLooted()
    {
        _lootedPotions.GetComponent<Animator>().enabled = true;
        _lootedPotions.text = "+" + GameManager.Instance._lootInRoom;
        yield return new WaitForSeconds(3f);
        _lootedPotions.GetComponent<Animator>().enabled = false;
    }

    public void CRRestart()
    {
        _AM.SoundDrum();
        StartCoroutine(Restart());
    }

    public void CRMainMenu()
    {
        _AM.SoundBamboo();
        StartCoroutine(MainMenu());
    }

    private IEnumerator Restart()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Game");
    }

    private IEnumerator MainMenu()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainMenu");
    }

}
