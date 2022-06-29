using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TitleScreen : MonoBehaviour
{
    PlayerInputMap _inputs;

    [SerializeField] Volume _volume;
    DepthOfField _dof;
    float _t;
    private bool _shouldPlay = false;

    [SerializeField] Image _blackFade;

    [SerializeField] GameObject _titleMenu;
    [SerializeField] GameObject _playButton;

    [SerializeField] GameObject _commandsMenu;
    [SerializeField] GameObject _punchButton;

    [SerializeField] GameObject _punchExplanationsMenu;
    [SerializeField] GameObject _punchReturn;
    [SerializeField] GameObject _dodgeExplanationsMenu;
    [SerializeField] GameObject _dodgeReturn;
    [SerializeField] GameObject _shieldExplanationsMenu;
    [SerializeField] GameObject _shieldReturn;
    [SerializeField] GameObject _spearsExplanationsMenu;
    [SerializeField] GameObject _spearsReturn;

    [SerializeField] GameObject _creditsMenu;
    [SerializeField] GameObject _creditsReturn;

    GameObject _currentMenu;
    EventSystem _eventSystem;

    void Awake()
    {
        _inputs = new PlayerInputMap();
        _inputs.Actions.Shield.started += _ => Return();
        _inputs.Actions.Dodge.started += _ => Refocus();
        _inputs.Movement.Move.started += _ => Refocus();

        if (_volume)
        {
            _volume.profile.TryGet<DepthOfField>(out _dof);
        }
        _eventSystem = GameObject.FindObjectOfType<EventSystem>();
    }

    void Start()
    {
        if (Gamepad.current != null) Gamepad.current.SetMotorSpeeds(0, 0);
        Cursor.visible = true;
        _shouldPlay = false;
        _dof.active = true;

        _titleMenu.SetActive(false);
        _commandsMenu.SetActive(false);
        _creditsMenu.SetActive(false);
        _punchExplanationsMenu.SetActive(false);
        _dodgeExplanationsMenu.SetActive(false);
        _shieldExplanationsMenu.SetActive(false);
        _spearsExplanationsMenu.SetActive(false);
        GoToAMenu(_titleMenu);
    }

    void Refocus()
    {
        if (_eventSystem.currentSelectedGameObject == null)
        {
            if (_currentMenu == _titleMenu)
            {
                _eventSystem.SetSelectedGameObject(_playButton);
                return;
            }
            else if (_currentMenu == _creditsMenu)
            {
                _eventSystem.SetSelectedGameObject(_creditsReturn);
                return;
            }
            else if (_currentMenu == _commandsMenu)
            {
                _eventSystem.SetSelectedGameObject(_punchButton);
                return;
            }
            else if (_currentMenu == _punchExplanationsMenu)
            {
                _eventSystem.SetSelectedGameObject(_punchReturn);
                return;
            }
            else if (_currentMenu == _dodgeExplanationsMenu)
            {
                _eventSystem.SetSelectedGameObject(_dodgeReturn);
                return;
            }
            else if (_currentMenu == _shieldExplanationsMenu)
            {
                _eventSystem.SetSelectedGameObject(_shieldReturn);
                return;
            }
            else if (_currentMenu == _spearsExplanationsMenu)
            {
                _eventSystem.SetSelectedGameObject(_spearsReturn);
                return;
            }
        }
    }

    public void Return()
    {
        if (_currentMenu == _titleMenu)
            return;
        else if (_currentMenu == _commandsMenu)
        {
            GoToMainMenu();
            SoundManager.Instance.PlayMenuClose();
            return;
        }
        else if (_currentMenu == _creditsMenu)
        {
            GoToMainMenu();
            SoundManager.Instance.PlayMenuClose();
            return;
        }
        else if (_currentMenu == _punchExplanationsMenu || _currentMenu == _dodgeExplanationsMenu || _currentMenu == _shieldExplanationsMenu || _currentMenu == _spearsExplanationsMenu)
        {
            GoToCommandsMenu();
            SoundManager.Instance.PlayMenuClose();
            return;
        }
    }

    #region Title Screen

    public void GoToMainMenu()
    {
        GoToAMenu(_titleMenu);
        _eventSystem.SetSelectedGameObject(_playButton);
    }

    public void Play()
    {
        _t = 0;
        _shouldPlay = true;
    }

    void Update()
    {
        if (_shouldPlay)
        {
            _t += Time.deltaTime;
            _blackFade.color = new Color(0, 0, 0, _t * 2);
            if (_t > 1)
                SceneManager.LoadScene(1);
        }
    }

    public void Quit()
    {
        Invoke("QuitGame", 0.2f);
    }

    void QuitGame()
    {
        Application.Quit();
    }

    #endregion

    #region How to play

    public void GoToCommandsMenu()
    {
        GoToAMenu(_commandsMenu);
        _eventSystem.SetSelectedGameObject(_punchButton);
    }

    public void GoToPunchExplanationsMenu()
    {
        GoToAMenu(_punchExplanationsMenu);
        _eventSystem.SetSelectedGameObject(_punchReturn);
    }
    public void GoToDodgeExplanationsMenu()
    {
        GoToAMenu(_dodgeExplanationsMenu);
        _eventSystem.SetSelectedGameObject(_dodgeReturn);
    }
    public void GoToSpearsExplanationsMenu()
    {
        GoToAMenu(_spearsExplanationsMenu);
        _eventSystem.SetSelectedGameObject(_spearsReturn);
    }
    public void GoToShieldExplanationsMenu()
    {
        GoToAMenu(_shieldExplanationsMenu);
        _eventSystem.SetSelectedGameObject(_shieldReturn);
    }

    #endregion

    #region credits
    public void GoToCreditsMenu()
    {
        GoToAMenu(_creditsMenu);
        _eventSystem.SetSelectedGameObject(_creditsReturn);
    }
    #endregion

    public void GoToAMenu(GameObject menu)
    {
        if (_currentMenu != null)
            _currentMenu.SetActive(false);
        _currentMenu = menu;
        _currentMenu.SetActive(true);
    }

    #region inputs
    void OnEnable()
    {
        _inputs.Enable();
    }

    void OnDisable()
    {
        _inputs.Disable();
    }
    #endregion
}
