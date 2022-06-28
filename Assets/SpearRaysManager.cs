using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearRaysManager : LocalManager<SpearRaysManager>
{
    [SerializeField] GameObject _lR;
    [SerializeField] GameObject _cclL;
    [SerializeField] GameObject _cclR;

    public void EnableRightRay(bool b)
    {
        _cclR.SetActive(b);
    }

    public void EnableLeftRay(bool b)
    {
        _cclL.SetActive(b);
    }

    public void EnableLRRay(bool b)
    {
        _lR.SetActive(b);
    }
}
