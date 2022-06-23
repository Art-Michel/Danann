using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class UiManager : LocalManager<UiManager>
{
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

    public Sprite RSpearTransparentIcon { get { return _rSpearTransparentIcon; } }
    #endregion

    [SerializeField] Volume _volume;

    protected override void Awake()
    {
        base.Awake();
        _volume.profile.TryGet<DepthOfField>(out _depthOfField);
    }

    #region pause
    [SerializeField] GameObject _pauseUiParent;
    bool _isPaused = false;
    DepthOfField _depthOfField;

    public void PauseInput()
    {
        _isPaused = !_isPaused;
        if (_isPaused) Pause();
        else Unpause();
    }

    void Pause()
    {
        _ingameUiParent.SetActive(false);
        _pauseUiParent.SetActive(true);
        Time.timeScale = 0;
        _depthOfField.active = true;
    }

    public void Unpause()
    {
        _ingameUiParent.SetActive(true);
        _pauseUiParent.SetActive(false);
        Time.timeScale = 1;
        _depthOfField.active = false;
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitToDesktop()
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
}
