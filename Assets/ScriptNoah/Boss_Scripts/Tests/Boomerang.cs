using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [SerializeField] Transform br;
    [SerializeField] float rotSpeed;
    [SerializeField] float rotCurve;
    [SerializeField] bool isCUrving;
    public void SetCurve(bool value){isCUrving=value;}
   private void Update() {
        if (!isCUrving)
            br.transform.Rotate(0,rotSpeed,0);
        else
            br.transform.Rotate(0,rotCurve,0);
   }
}
