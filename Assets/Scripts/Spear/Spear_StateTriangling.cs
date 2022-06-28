using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_StateTriangling : Spear_State
{
    public Spear_StateTriangling() : base(Spear_StateNames.TRIANGLING)
    {

    }

    public override void Begin()
    {
        SpearRaysManager.Instance.EnableLeftRay(true);
        SpearRaysManager.Instance.EnableRightRay(true);
        SpearRaysManager.Instance.EnableLRRay(true);
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        SpearRaysManager.Instance.EnableLeftRay(false);
        SpearRaysManager.Instance.EnableRightRay(false);
        SpearRaysManager.Instance.EnableLRRay(false);
    }
}
