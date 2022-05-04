using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LingeringHitbox : Hitbox
{
    bool _isActive = true;

    void Update()
    {
        if (_isActive)
        {
            CheckIntersection();
        }
    }
}
