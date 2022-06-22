using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2DSpin: Danu_State
{
    public P2DSpin() : base(StateNames.P2D_SPIN){}

    private GameObject globalGO;
    GameObject preview;
    Transform[] bladesPreview = new Transform[4];
    AttackData bladesParentAttackData;
    private Transform dSphereN,dSphereW,dSphereE,dSphereS;
    private float dist;
    private float rotationSpeed;
    private bool turningRight;
    private float maxWaitTime;
    private float lifetime;
    Vector3 n,w,e,s;

    List<GameObject> nblades = new List<GameObject>();
    List<GameObject> wblades = new List<GameObject>();
    List<GameObject> eblades = new List<GameObject>();
    List<GameObject> sblades = new List<GameObject>();
    private bool wait;
    private float waitTime;
    bool isSetUp;
    Pool pool;
    // Start is called before the first frame update
    public override void Begin()
    {
        if (!isInit)
            Init();
        //activation des helices, ou instantiation si c'est la premiere fois
        fsm.transform.LookAt(fsm.agent.GetArenaCenter());
        preview.gameObject.SetActive(true);
        waitTime = 0;
        wait = true;
        fsm.agent.vfx[5].SetActive(true);
    }
    public override void Init()
    {
        pool = fsm.GetPool();
        //setup des variables
        dist = fsm.GetP2Sp_Dist();
        globalGO = fsm.GetP2GlobalGO();
        bladesParentAttackData = globalGO.GetComponent<AttackData>();
        preview = fsm.GetP1sD_Preview();
        Transform[] nwesTrans = fsm.GetP2NWESMax();
        bladesPreview = fsm.Getp2BladesPreview();
        dSphereN = nwesTrans[0];
        dSphereE = nwesTrans[1];
        dSphereW = nwesTrans[2];
        dSphereS = nwesTrans[3];
        rotationSpeed = fsm.GetP2RotationSpeed();
        turningRight = fsm.GetP2TurningRight();
        maxWaitTime = fsm.GetP2MaxWaitTime();
        lifetime = fsm.GetP2SpinLifeTime();
        base.Init();
    }
    // Update is called once per frame
    public override void Update()
    {
        if (wait)
        {
            waitTime += Time.deltaTime;
            fsm.transform.position = Vector3.Lerp(fsm.transform.position, fsm.agent.GetArenaCenter(), waitTime / maxWaitTime);
            if (fsm.transform.position == fsm.agent.GetArenaCenter())
            {
                SpawnBladesPreview();
            }
            if (waitTime >= maxWaitTime)
            {
                wait = false;
                SpawnBlades();
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
        Transform[] nwesTrans = fsm.GetP2NWESMax();
        Vector3 center = fsm.agent.GetArenaCenter();
        preview.gameObject.SetActive(false);
        for (int i = 0; i < bladesPreview.Length; i++)
        {
                        bladesPreview[i].gameObject.SetActive(true);
            bladesPreview[i].position = center;
            bladesPreview[i].LookAt(nwesTrans[i]);
        }
    }

    private void SpawnBlades()
    {
        float delta = 1 / dist;
        bladesPreview[0].gameObject.SetActive(false);
        bladesPreview[1].gameObject.SetActive(false);
        bladesPreview[2].gameObject.SetActive(false);
        bladesPreview[3].gameObject.SetActive(false);
        n = dSphereN.position;
        w = dSphereW.position;
        e = dSphereE.position;
        s = dSphereS.position;
        for (int i = 0; i < dist; i++)
        {
            SetupBall(pool.SecondGet(), Vector3.Lerp(fsm.transform.position, dSphereN.position, delta * i), nblades);
            SetupBall(pool.SecondGet(), Vector3.Lerp(fsm.transform.position, dSphereW.position, delta * i), wblades);
            SetupBall(pool.SecondGet(), Vector3.Lerp(fsm.transform.position, dSphereE.position, delta * i), eblades);
            SetupBall(pool.SecondGet(), Vector3.Lerp(fsm.transform.position, dSphereS.position, delta * i), sblades);
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
                wPos = Vector3.Lerp(fsm.transform.position, dSphereW.position, delta * newind);
                wblades.Add(fsm.InstantiateStaticProjectile(wPos));
                break;
            case SpinBullet.bladeIndex.EAST:
                Vector3 ePos;
                ePos = Vector3.Lerp(fsm.transform.position, dSphereE.position, delta * newind);
                eblades.Add(fsm.InstantiateStaticProjectile(ePos));
                break;
            case SpinBullet.bladeIndex.SOUTH:
                Vector3 sPos;
                sPos = Vector3.Lerp(fsm.transform.position, dSphereS.position, delta * newind);
                sblades.Add(fsm.InstantiateStaticProjectile(sPos));
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
        bladesParentAttackData.StopAttack();
        globalGO.transform.rotation = Quaternion.identity;
        float delta = 1 / dist;
        for (int i = 0; i < nblades.Count; i++)
        {
            Debug.Log(nblades.Count);
            Debug.Log(wblades.Count);
            Debug.Log(eblades.Count);
            Debug.Log(sblades.Count);
            pool.SecondBack(nblades[i]);
            pool.SecondBack(wblades[i]);
            pool.SecondBack(eblades[i]);
            pool.SecondBack(sblades[i]);
        }
    }
}
