using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] Image _blackFade;
    float _t;
    private bool _shouldPlay = false;

    void Start()
    {
        _shouldPlay = false;
    }

    public void Play()
    {
        _t = 0;
        _shouldPlay = true;
    }

    void Update()
    {
        Debug.Log(_t);
        if (_shouldPlay)
        {
            _t += Time.deltaTime;
            _blackFade.color = new Color(0, 0, 0, _t * 2);
            if (_t > 1)
                SceneManager.LoadScene(1);
        }
    }

    public void Credits()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
