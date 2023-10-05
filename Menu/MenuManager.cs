using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _buttons;
    [SerializeField] GameObject _customize;
    [SerializeField] GameObject _character;
    [SerializeField] GameObject _scoreBoard;
    [SerializeField] GameObject _howToPlay;
    [SerializeField] GameObject _settings;

    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip[] _musics;


    [SerializeField] UnityEngine.UI.Image[] colors;
    [SerializeField] Material _characterColor;
    [SerializeField] Animator _cameraAnimator;
    [SerializeField] TextMeshPro _nameOfCharacter;
    [SerializeField] TextMeshProUGUI[] _topPlayers;

    public PlayerProps playerProps;
    public static MenuManager Instance;
    public bool Processed;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _source.clip = _musics[Random.Range(0,3)];
        _source.Play();
        playerProps = new PlayerProps();
        playerProps.color[0] = _characterColor.color.r;
        playerProps.color[1] = _characterColor.color.g;
        playerProps.color[2] = _characterColor.color.b;
        DontDestroyOnLoad(gameObject);
    }
    public void Play()
    {
        _buttons.SetActive(false);
        _cameraAnimator.SetBool("play", true);
        StartCoroutine("FrogCustomizeMenu");
    }
    public void SetSound(float value)
    {
        _source.volume = value;
    }
    IEnumerator FrogCustomizeMenu()
    {
        yield return new WaitForSeconds(2.2f);
        _customize.SetActive(true);
    }
    public void OnPointerDown(int index)
    {
        _characterColor.color = colors[index].color;
        playerProps.color[0] = colors[index].color.r;
        playerProps.color[1] = colors[index].color.g;
        playerProps.color[2] = colors[index].color.b;
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void TypeName(string name)
    {
        playerProps.name = name;
    }
    public void NameOfCharacter(string name)
    {
        _nameOfCharacter.text = name;
    }
    public void BackToMenu()
    {
        _customize.SetActive(false);
        _cameraAnimator.SetBool("play", false);
        StartCoroutine("GoBackToMenu");
    }
    public void Menu()
    {
        _buttons.SetActive(true);
        _scoreBoard.SetActive(false);
        _howToPlay.SetActive(false);
        _settings.SetActive(false);
    }
    public void HowToPlay()
    {
        _howToPlay.SetActive(true);
        _buttons.SetActive(false);
    }
    public void ScoreBoard()
    {
        if (Processed)
        {
            _buttons.SetActive(false);
            _scoreBoard.SetActive(true);
        }
    }
    public void Setting()
    {
        _settings.SetActive(true);
        _buttons.SetActive(false);
    }
    public void LoadDatas()
    {
        List<PlayerProps> data = JSONdatabase.instance.scoreList;
        for (int i = 0; i < data.Count; i++)
        {
            _topPlayers[i].text = (i + 1) + ")" + "  " + data[(data.Count-1)-i].name + "      " + data[(data.Count-1)-i].score;
        }
        Processed = true;
    }
    
    IEnumerator GoBackToMenu()
    {
        yield return new WaitForSeconds(2.2f);
        _buttons.SetActive(true);
    }
}
