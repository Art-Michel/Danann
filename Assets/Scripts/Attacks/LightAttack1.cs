using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttack1 : Attack
{
    void Awake()
    {
        AttackId = Ccl_Attacks.LIGHTATTACK1;
        Sender = CCL;
    }
}
