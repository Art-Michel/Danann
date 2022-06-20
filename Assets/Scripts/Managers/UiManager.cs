using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : LocalManager<UiManager>
{
    [SerializeField] TextMeshProUGUI _nText;
    public TextMeshProUGUI NText { get { return _nText; } }

    [SerializeField] TextMeshProUGUI _sText;
    public TextMeshProUGUI SText { get { return _sText; } }

    [SerializeField] TextMeshProUGUI _eText;
    public TextMeshProUGUI EText { get { return _eText; } }

    [SerializeField] TextMeshProUGUI _wText;
    public TextMeshProUGUI WText { get { return _wText; } }

    [SerializeField] TextMeshProUGUI _ltText;
    public TextMeshProUGUI LtText { get { return _ltText; } }

    [SerializeField] TextMeshProUGUI _rtText;
    public TextMeshProUGUI RtText { get { return _rtText; } }

    [SerializeField] TextMeshProUGUI _dodgesCount;
    public TextMeshProUGUI DodgesCount { get { return _dodgesCount; } }

    Color _opaque = new Color(1, 1, 1, 1f);
    Color _transparent = new Color(1, 1, 1, 0.25f);

    [SerializeField] Image _dodgeImage;
    [SerializeField] Image _dodgeFrameImage;

    [SerializeField] GameObject _dodge;
    [SerializeField] GameObject _dash;
    [SerializeField] GameObject _cancel;
    [SerializeField] Image _eCost;
    [SerializeField] Image _dodgeFill;
    public GameObject DodgeWhiteFrame;

    [SerializeField] Image _eButton;
    public Image EButton { get { return _eButton; } }

    [SerializeField] Image _wButton;
    public Image WButton { get { return _wButton; } }

    [SerializeField] Image _ltButton;
    public Image LtButton { get { return _ltButton; } }

    [SerializeField] Image _rtButton;
    public Image RtButton { get { return _rtButton; } }


    [SerializeField] Sprite _punchIcon;
    public Sprite PunchIcon { get { return _punchIcon; } }

    [SerializeField] Sprite _dodgeIcon;
    public Sprite DodgeIcon { get { return _dodgeIcon; } }

    [SerializeField] Sprite _dashIcon;
    public Sprite DashIcon { get { return _dashIcon; } }

    [SerializeField] Sprite _shieldIcon;
    public Sprite ShieldIcon { get { return _shieldIcon; } }

    [SerializeField] Sprite _cancelIcon;
    public Sprite CancelIcon { get { return _cancelIcon; } }

    [SerializeField] Sprite _lSpearIcon;
    public Sprite LSpearIcon { get { return _lSpearIcon; } }

    [SerializeField] Sprite _rSpearIcon;
    public Sprite RSpearIcon { get { return _rSpearIcon; } }

    [SerializeField] Sprite _lSpearTransparentIcon;
    public Sprite LSpearTransparentIcon { get { return _lSpearTransparentIcon; } }

    [SerializeField] Sprite _rSpearTransparentIcon;

    public Sprite RSpearTransparentIcon { get { return _rSpearTransparentIcon; } }

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
