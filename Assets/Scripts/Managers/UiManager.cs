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


    [SerializeField] Image _nButton;
    public Image NButton { get { return _nButton; } }

    [SerializeField] Image _sButton;
    public Image SButton { get { return _sButton; } }

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

    [SerializeField] Sprite _parryIcon;
    public Sprite ParryIcon { get { return _parryIcon; } }

    [SerializeField] Sprite _cancelIcon;
    public Sprite CancelIcon { get { return _cancelIcon; } }

    [SerializeField] Sprite _lSpearIcon;
    public Sprite LSpearIcon { get { return _lSpearIcon; } }

    [SerializeField] Sprite _rSpearIcon;
    public Sprite RSpearIcon { get { return _rSpearIcon; } }

    void SetSprite(Image image, Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void SetText(TextMeshProUGUI textU, string text)
    {
        textU.text = text;
    }
}
