using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttack1 : AttackData
{
    void Awake()
    {
        AttackId = Ccl_Attacks.LIGHTATTACK1;
        Sender = CCL;
    }
}
