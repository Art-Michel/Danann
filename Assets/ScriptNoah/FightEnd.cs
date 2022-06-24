using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FightEnd : MonoBehaviour
{
    [SerializeField] Image fade;
     [SerializeField]bool startFade;
    float t;
     [SerializeField]float fadeTime;
     [SerializeField] float timeSlow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startFade)
        {
            Time.timeScale=Mathf.Lerp(1,timeSlow,t/0.5f);
            fade.color=new Color(1,1,1,t/fadeTime);
            t+=Time.deltaTime/Time.timeScale;
        }
    }
}
