using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackShake
{
    static public Dictionary<string, float> ShakeValue = new Dictionary<string, float>()
    {
        {Ccl_Attacks.LIGHTATTACK1, 0.2f},
        {Ccl_Attacks.LIGHTATTACK2, 0.3f},
        {Ccl_Attacks.LIGHTATTACK3, 0.5f},
        {Ccl_Attacks.TRIANGLE, 0.5f},
        {Ccl_Attacks.DASHONSPEAR, 0.75f},
        {Ccl_Attacks.SPEARSWINGL, 0.5f},
        {Ccl_Attacks.SPEARSWINGR, 0.5f},
        {Ccl_Attacks.TRAVELINGSPEAR, 0.2f},
    };
}