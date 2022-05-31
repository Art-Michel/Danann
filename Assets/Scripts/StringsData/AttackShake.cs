using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackShake
{
    static public Dictionary<string, Vector2> ShakeValue = new Dictionary<string, Vector2>()
    {
        {Ccl_Attacks.LIGHTATTACK1, new Vector2(0.05f, 0.2f)},
        {Ccl_Attacks.LIGHTATTACK2, new Vector2(0.1f, 0.3f)},
        {Ccl_Attacks.LIGHTATTACK3, new Vector2(0.2f, 0.75f)},
        {Ccl_Attacks.TRIANGLE,    new Vector2(0.1f, 0.5f)},
        {Ccl_Attacks.DASHONSPEAR, new Vector2(0.2f, 0.75f)},
        {Ccl_Attacks.SPEARSWINGL, new Vector2(0.15f, 0.4f)},
        {Ccl_Attacks.SPEARSWINGR, new Vector2(0.15f, 0.4f)},
        {Ccl_Attacks.TRAVELINGSPEAR, new Vector2(0.1f, 0.2f)},
    };
}