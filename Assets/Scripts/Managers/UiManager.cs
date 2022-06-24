using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

public class UiManager : LocalManager<UiManager>
{
    EventSystem m_EventSystem;
    #region ingameui
    [SerializeField] GameObject _ingameUiParent;

    [Foldout("ingameUI")]
    [SerializeField] TextMeshProUGUI _nText;
    public TextMeshProUGUI NText { get { return _nText; } }

    [Foldout("ingameUI")]
    [SerializeField] TextMeshProUGUI _sText;
    public TextMeshProUGUI SText { get { return _sText; } }

    [Foldout("ingameUI")]
    [SerializeField] TextMeshProUGUI _eText;
    public TextMeshProUGUI EText { get { return _eText; } }

    [Foldout("ingameUI")]
    [SerializeField] TextMeshProUGUI _wText;
    public TextMeshProUGUI WText { get { return _wText; } }

    [Foldout("ingameUI")]
    [SerializeField] TextMeshProUGUI _ltText;
    public TextMeshProUGUI LtText { get { return _ltText; } }

    [Foldout("ingameUI")]
    [SerializeField] TextMeshProUGUI _rtText;
    public TextMeshProUGUI RtText { get { return _rtText; } }

    [Foldout("ingameUI")]
    [SerializeField] TextMeshProUGUI _dodgesCount;
    public TextMeshProUGUI DodgesCount { get { return _dodgesCount; } }

    Color _opaque = new Color(1, 1, 1, 1f);
    Color _transparent = new Color(1, 1, 1, 0.25f);

    [Foldout("ingameUI")]
    [SerializeField] Image _dodgeImage;
    [Foldout("ingameUI")]
    [SerializeField] Image _dodgeFrameImage;

    [Foldout("ingameUI")]
    [SerializeField] GameObject _dodge;
    [Foldout("ingameUI")]
    [SerializeField] GameObject _dash;
    [Foldout("ingameUI")]
    [SerializeField] GameObject _cancel;
    [Foldout("ingameUI")]
    [SerializeField] Image _eCost;
    [Foldout("ingameUI")]
    [SerializeField] Image _dodgeFill;
    public GameObject DodgeWhiteFrame;

    [Foldout("ingameUI")]
    [SerializeField] Image _eButton;
    public Image EButton { get { return _eButton; } }

    [Foldout("ingameUI")]
    [SerializeField] Image _wButton;
    public Image WButton { get { return _wButton; } }

    [Foldout("ingameUI")]
    [SerializeField] Image _ltButton;
    public Image LtButton { get { return _ltButton; } }

    [Foldout("ingameUI")]
    [SerializeField] Image _rtButton;
    public Image RtButton { get { return _rtButton; } }

    [Foldout("ingameUI")]
    [SerializeField] Sprite _punchIcon;
    public Sprite PunchIcon { get { return _punchIcon; } }

    [Foldout("ingameUI")]
    [SerializeField] Sprite _dodgeIcon;
    public Sprite DodgeIcon { get { return _dodgeIcon; } }

    [Foldout("ingameUI")]
    [SerializeField] Sprite _dashIcon;
    public Sprite DashIcon { get { return _dashIcon; } }

    [Foldout("ingameUI")]
    [SerializeField] Sprite _shieldIcon;
    public Sprite ShieldIcon { get { return _shieldIcon; } }

    [Foldout("ingameUI")]
    [SerializeField] Sprite _cancelIcon;
    public Sprite CancelIcon { get { return _cancelIcon; } }

    [Foldout("ingameUI")]
    [SerializeField] Sprite _lSpearIcon;
    public Sprite LSpearIcon { get { return _lSpearIcon; } }

    [Foldout("ingameUI")]
    [SerializeField] Sprite _rSpearIcon;
    public Sprite RSpearIcon { get { return _rSpearIcon; } }

    [Foldout("ingameUI")]
    [SerializeField] Sprite _lSpearTransparentIcon;


    public Sprite LSpearTransparentIcon { get { return _lSpearTransparentIcon; } }

    [Foldout("ingameUI")]
    [SerializeField] Sprite _rSpearTransparentIcon;

    [Foldout("ingameUI")]
    [SerializeField] Image _dashPng;
    [Foldout("ingameUI")]
    [SerializeField] Image _dashFramePng;
    [Foldout("ingameUI")]
    [SerializeField] Image _parryPng;
    [Foldout("ingameUI")]
    [SerializeField] Image _parryFramePng;

    public Sprite RSpearTransparentIcon { get { return _rSpearTransparentIcon; } }
    #endregion

    #region Win and Loss

    [SerializeField] GameObject _winScreen;
    [SerializeField] GameObject _lossScreen;
    [SerializeField] Image _lossBlack;
    [SerializeField] Image _winBlack;
    bool _shouldFadeToWin = false;
    [SerializeField] Image _preWinFlash;

    internal void PreWinScreen()
    {
        _t = 0;
        _preWinFlash.gameObject.SetActive(true);
        _shouldFadeToWin = true;
    }
    [SerializeField] GameObject _winButton;
    public void DisplayWinScreen()
    {
        _shouldFadeToWin = false;
        _playerActions.gameObject.SetActive(false);
        _canPause = false;
        _playerMovement.enabled = false;
        _blackFade = _winBlack;
        _playerActions.enabled = false;
        _ingameUiParent.SetActive(false);
        _dof.active = true;
        _winScreen.SetActive(true);
        m_EventSystem.SetSelectedGameObject(_winButton);
    }

    [SerializeField] GameObject _lossButton;
    bool _shouldFadeToLose = false;
    public void DisplayLossScreen()
    {
        _canPause = false;
        _t = 0;
        Time.timeScale = 0.1f;
        _shouldFadeToLose = true;
        _playerMovement.enabled = false;
        _playerActions.enabled = false;
        _ingameUiParent.SetActive(false);
        _blackFade = _lossBlack;
        _dof.active = true;
        _lossScreen.SetActive(true);
        m_EventSystem.SetSelectedGameObject(_lossButton);
    }
    #endregion

    [SerializeField] Volume _volume;
    PlayerInputMap _inputs;
    protected override void Awake()
    {
        base.Awake();
        _inputs = new PlayerInputMap();
        _volume.profile.TryGet<DepthOfField>(out _dof);
        _inputs.Actions.Pause.started += _ => PauseInput();
        m_EventSystem = EventSystem.current;
    }

    void Start()
    {
        _dof.active = false;
        _isPaused = false;
    }

    #region pause
    bool _canPause = true;
    [SerializeField] GameObject _pauseUiParent;
    bool _isPaused;
    DepthOfField _dof;

    [SerializeField] Image _blackFade;
    float _t;
    private bool _ShouldFadeToQuit = false;
    private bool _ShouldFadeToRestart = false;

    [SerializeField] PlayerActions _playerActions;
    [SerializeField] PlayerMovement _playerMovement;

    void PauseInput()
    {
        Debug.Log(_isPaused);
        if (!_isPaused) Pause();
        else Unpause();
    }

    [SerializeField] GameObject _pauseButton;
    void Pause()
    {
        _isPaused = true;
        _playerActions.enabled = false;
        _playerMovement.enabled = false;
        SoundManager.Instance.PlayMenuOpen();
        _ingameUiParent.SetActive(false);
        _pauseUiParent.SetActive(true);
        Time.timeScale = 0;
        _dof.active = true;
        m_EventSystem.SetSelectedGameObject(_pauseButton);
    }

    public void Unpause()
    {
        _isPaused = false;
        _ingameUiParent.SetActive(true);
        _playerActions.enabled = true;
        _playerMovement.enabled = true;
        _pauseUiParent.SetActive(false);
        Time.timeScale = 1;
        _dof.active = false;
        SoundManager.Instance.PlayMenuClose();
    }

    public void Quit()
    {
        Time.timeScale = 1;
        _ShouldFadeToQuit = true;
        _t = 0.6f;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        _ShouldFadeToRestart = true;
        _t = 0.6f;
    }

    void Update()
    {
        if (_ShouldFadeToQuit)
        {
            _t += Time.deltaTime * .7f;
            _blackFade.color = new Color(0, 0, 0, _t);
            if (_t > 1)
                SceneManager.LoadScene(0);
        }
        if (_ShouldFadeToRestart)
        {
            _t += Time.deltaTime * .35f;
            _blackFade.color = new Color(0, 0, 0, _t);
            if (_t > 1)
                SceneManager.LoadScene(1);
        }
        if (_shouldFadeToWin)
        {
            _t += Time.deltaTime;
            _preWinFlash.color = new Color(1, 1, 1, _t);
            if (_t > 1)
                DisplayWinScreen();
        }
        if(_shouldFadeToLose)
        {
            _t += Time.deltaTime;
            if (_t > .2f)
                Time.timeScale = 1;
        }
    }

    public void QuitToDesktop()
    {
        Time.timeScale = 1;
        Invoke("QuitGame", 0.2f);
    }

    void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    void SetSprite(Image image, Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void EnableDash()
    {
        _cancel.SetActive(false);
        _dodge.SetActive(false);
        _dash.SetActive(true);
    }

    public void EnableDodge()
    {
        _cancel.SetActive(false);
        _dodge.SetActive(true);
        _dash.SetActive(false);
    }

    public void EnableCancel()
    {
        _dodge.SetActive(false);
        _dash.SetActive(false);
        _cancel.SetActive(true);
    }

    public void SetSpearImage(bool isLeft, bool transparent)
    {
        if (isLeft)
            if (transparent) LtButton.sprite = LSpearTransparentIcon;
            else LtButton.sprite = LSpearIcon;
        else
            if (transparent) RtButton.sprite = RSpearTransparentIcon;
        else RtButton.sprite = RSpearIcon;
    }

    public void AimHud()
    {
        EButton.sprite = CancelIcon;
        WButton.sprite = CancelIcon;
        _eCost.enabled = false;
        EnableCancel();
    }

    public void UnaimHud()
    {
        EButton.sprite = ShieldIcon;
        WButton.sprite = PunchIcon;
        _eCost.enabled = true;
        EnableDodge();
    }

    public void TargetHud()
    {
        EnableDash();
        EButton.sprite = CancelIcon;
        WButton.sprite = CancelIcon;
        _eCost.enabled = false;
    }

    public void UntargetHud()
    {
        EnableDodge();
        EButton.sprite = ShieldIcon;
        WButton.sprite = PunchIcon;
        _eCost.enabled = true;
    }

    public void OnePlasmaFilled()
    {
        Color opaque = new Color(1, 1, 1, 1f);
        _dashPng.color = opaque;
        _dashFramePng.color = opaque;
        _parryFramePng.color = opaque;
        _parryPng.color = opaque;
    }
    public void OnePlasmaEmptied()
    {
        Color transparent = new Color(1, 1, 1, 0.5f);
        _dashPng.color = transparent;
        _dashFramePng.color = transparent;
        _parryFramePng.color = transparent;
        _parryPng.color = transparent;
    }

    public void SetText(TextMeshProUGUI textU, string text)
    {
        textU.text = text;
    }

    public void FillDodge(float f)
    {
        _dodgeFill.fillAmount = f;
    }

    public void SetDodgeTransparency(bool transparent)
    {
        if (transparent) _dodgeImage.color = _transparent;
        else _dodgeImage.color = _opaque;
    }

    #region disable inputs on Player disable to avoid weird inputs
    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }
    #endregion
}
