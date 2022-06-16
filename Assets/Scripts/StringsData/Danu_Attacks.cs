using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danu_Attacks
{
    public const string SLAM1 = "Slam1";
    public const string SLAM2 = "Slam2";
    public const string SLAM3 = "Slam3";
    public const string DASH = "Dash";
    public const string DASH2 = "Dash2";
    public const string TP = "Tp";
    public const string SHOCKWAVE = "Shockwave";
    public const string PROJECTILE = "Projectile";

    static public Dictionary<string, bool> AttackIsMelee = new Dictionary<string, bool>
    {
        {SLAM1, true},
        {SLAM2, true},
        {SLAM3, true},
        {DASH, true},
        {DASH2, true},
        {TP, true},
        {PROJECTILE, false},
    };
/*
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
    };*/

}
