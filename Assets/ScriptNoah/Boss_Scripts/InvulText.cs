using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InvulText : MonoBehaviour
{
    [SerializeField]TMP_Text text;
    float alphaValue;
    [SerializeField]AnimationCurve curve;
    float t;
    [SerializeField] float time;
    // Start is called before the first frame update
     private void OnEnable()
    {
        text.color=new Color(1,1,1,alphaValue);
        t=0;
    }

    // Update is called once per frame
    void Update()
    {
        t+=Time.deltaTime;
        text.color=new Color(1,1,1,alphaValue);
        alphaValue=curve.Evaluate(t);
        if (t>time)
        {
            gameObject.SetActive(false);
        }
    }
}
