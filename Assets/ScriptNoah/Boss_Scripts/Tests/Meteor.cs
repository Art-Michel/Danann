using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField]Vector3 _frames;
    [SerializeField]GameObject preview;
    [SerializeField]GameObject boombox;
    public void SetFrames(Vector3 _newFrames){_frames=_newFrames;}
    public void SetDamage(int damage){_attackData.SetDamage(damage);}
    float _timer;
    int _state;
    int _index=0;
    Pool orig;
    public void SetOrig(Pool newOrig){orig=newOrig;}
    AttackData _attackData;
    private bool isSetup;

    // Start is called before the first frame update
    private void Awake() {
        _attackData=GetComponent<AttackData>();

    }
    public void SetUp()
    {
        _state=0;
        _timer=0;
        isSetup=true;
        preview.SetActive(true);
        boombox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSetup)
            return;
        _timer += Time.deltaTime;
        Debug.Log("e");
        if (_state == 0 && _timer > _frames.x)
        {
            StartAttack();
            Debug.Log("ee");
        }
        if (_state == 1 && _timer > _frames.y)
            StartRecovery();
        if (_state == 2 && _timer > _frames.z)
        {
            Debug.Log("done");
            gameObject.SetActive(false);
            orig.Back(gameObject);
        }
        //GoBackToPool
    }

    private void StartAttack()
    {
        _state++;
       _timer=0;

        preview.SetActive(false);
        boombox.SetActive(true);
        //_attackData.LaunchAttack();

    }

    private void StartRecovery()
    {
        _state++;
        _timer=0;
        boombox.SetActive(false);
        //_attackData.StopAttack();
    }
}
