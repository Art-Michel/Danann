using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1DSpin : Danu_State
{
    public P1DSpin() : base(StateNames.P1D_SPIN) { }
    private GameObject globalGO;
    Transform preview;
    Transform[] bladesPreview = new Transform[3];
    AttackData bladesParentAttackData;
    private Transform dSphereN;
    private Transform dSphereSW;
    private Transform dSphereSE;
    private float dist;
    private float rotationSpeed;
    private bool turningRight;
    private float maxWaitTime;
    private float lifetime;
    Vector3 n;
    Vector3 w;
    Vector3 e;
    List<GameObject> nblades = new List<GameObject>();
    List<GameObject> wblades = new List<GameObject>();
    List<GameObject> eblades = new List<GameObject>();
    private bool wait;
    private float waitTime;
    bool isSetUp;
    Pool pool;
    private Vector3 startPos;
    bool reachedPoint;

    // Start is called before the first frame update
    public override void Begin()
    {
        if (!isInit)
            Init();
        startPos=fsm.transform.position;
        //activation des helices, ou instantiation si c'est la premiere fois
        preview.localScale = new Vector3(1, 1, Vector3.Distance(fsm.transform.position, fsm.agent.GetArenaCenter()));
        preview.position = fsm.transform.position + (fsm.agent.GetArenaCenter() - fsm.transform.position) / 2;
        preview.LookAt(fsm.agent.GetArenaCenter());
        preview.gameObject.SetActive(true);
        waitTime = 0;
        wait = true;
        fsm.agent.vfx[5].SetActive(true);
        reachedPoint=false;
    }
    public override void Init()
    {
        pool = fsm.GetPool();
        //setup des variables
        dist = fsm.GetP1Sp_Dist();
        globalGO = fsm.GetP1GlobalGO();
        bladesParentAttackData = globalGO.GetComponent<AttackData>();
        preview = fsm.GetP1sD_Preview();
        Transform[] nweTrans = fsm.GetP1NWEMax();
        bladesPreview = fsm.GetBladesPreview();
        dSphereN = nweTrans[0];
        dSphereSE = nweTrans[1];
        dSphereSW = nweTrans[2];
        rotationSpeed = fsm.GetP1RotationSpeed();
        turningRight = fsm.GetP1TurningRight();
        maxWaitTime = fsm.GetP1MaxWaitTime();
        lifetime = fsm.GetP1SpinLifeTime();
        base.Init();
    }
    // Update is called once per frame
    public override void Update()
    {
        if (wait)
        {
            waitTime += Time.deltaTime;
            if (!reachedPoint)
            {
                fsm.transform.position = Vector3.Lerp(startPos, fsm.agent.GetArenaCenter(), waitTime / maxWaitTime);
                if (fsm.transform.position == fsm.agent.GetArenaCenter())
                {
                    SpawnBladesPreview();
                    waitTime-=0.7f;
                    reachedPoint=true;
                }
                return;
            }
            if (waitTime >= maxWaitTime)
            {
                wait = false;
                SpawnBlades();
                SoundManager.Instance.PlayBossAllOut();
            }
            else
                return;
        }
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            fsm.agent.vfx[5].SetActive(false);
            if (orig == null)
                fsm.agent.ToIdle();
            else
                orig.progression++;
        }
        Rotate();
    }

    private void SpawnBladesPreview()
    {
        Transform[] nweTrans = fsm.GetP1NWEMax();
        Vector3 center = fsm.agent.GetArenaCenter();
        preview.gameObject.SetActive(false);
        for (int i = 0; i < bladesPreview.Length; i++)
        {
            bladesPreview[i].gameObject.SetActive(true);
            bladesPreview[i].localScale = new Vector3(1, 1, Vector3.Distance(center, nweTrans[i].position));
            bladesPreview[i].position = center + (nweTrans[i].position - center) / 2;
            bladesPreview[i].LookAt(nweTrans[i]);
        }
    }

    private void SpawnBlades()
    {
        float delta = 1 / dist;
        bladesPreview[0].gameObject.SetActive(false);
        bladesPreview[1].gameObject.SetActive(false);
        bladesPreview[2].gameObject.SetActive(false);
        n = dSphereN.position;
        w = dSphereSW.position;
        e = dSphereSE.position;
        for (int i = 0; i < dist; i++)
        {
            SetupBall(pool.SecondGet(), Vector3.Lerp(fsm.transform.position, dSphereN.position, delta * i), nblades);
            SetupBall(pool.SecondGet(), Vector3.Lerp(fsm.transform.position, dSphereSW.position, delta * i), wblades);
            SetupBall(pool.SecondGet(), Vector3.Lerp(fsm.transform.position, dSphereSE.position, delta * i), eblades);
        }
        globalGO.SetActive(true);
        bladesParentAttackData.GetChildrenHitboxes();
        bladesParentAttackData.SetupHitboxes();
        bladesParentAttackData.LaunchAttack();
        isSetUp = true;
    }

    void SetupBall(GameObject ball, Vector3 position, List<GameObject> blades)
    {
        ball.transform.position = position;
        ball.transform.parent = bladesParentAttackData.transform;
        blades.Add(ball);
        ball.SetActive(true);
    }

    public void Regenerate(SpinBullet ded)
    {
        int newind = ded.GetIndex();
        SpinBullet.bladeIndex blade = ded.GetBlade();
        float delta = 1 / dist;
        switch (blade)
        {
            case SpinBullet.bladeIndex.NORTH:
                Vector3 nPos;
                nPos = Vector3.Lerp(fsm.transform.position, dSphereN.position, delta * newind);
                nblades.Add(fsm.InstantiateStaticProjectile(nPos));
                break;
            case SpinBullet.bladeIndex.WEST:
                Vector3 wPos;
                wPos = Vector3.Lerp(fsm.transform.position, dSphereSW.position, delta * newind);
                wblades.Add(fsm.InstantiateStaticProjectile(wPos));
                break;
            case SpinBullet.bladeIndex.EAST:
                Vector3 ePos;
                ePos = Vector3.Lerp(fsm.transform.position, dSphereSE.position, delta * newind);
                eblades.Add(fsm.InstantiateStaticProjectile(ePos));
                break;
        }
    }
    private void Rotate()
    {
        if (turningRight)
            globalGO.transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
        else
            globalGO.transform.Rotate(new Vector3(0, -rotationSpeed * Time.deltaTime, 0));
    }
    public override void End()
    {
        globalGO.SetActive(false);
        lifetime = fsm.GetP1SpinLifeTime();
        fsm.agent.m_anims.SetTrigger("SpinOver");
        bladesParentAttackData.StopAttack();
        globalGO.transform.rotation = Quaternion.identity;
        float delta = 1 / dist;
        for (int i = 0; i < nblades.Count; i++)
        {
            nblades[i].SetActive(false);
            wblades[i].SetActive(false);
            eblades[i].SetActive(false);
        }
    }
}