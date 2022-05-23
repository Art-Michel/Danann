using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part2Heli : MonoBehaviour
{
    [SerializeField]private GameObject globalGO;
   [SerializeField] Transform preview;
    [SerializeField]Transform[] bladesPreview = new Transform[4];
   [SerializeField] AttackData bladesParentAttackData;
  [SerializeField]  private Transform dSphereN,dSphereW,dSphereE,dSphereS;
  [SerializeField]  private float dist;
   [SerializeField] private float rotationSpeed;
  [SerializeField]  private bool turningRight;
 [SerializeField]   private float maxWaitTime;
 [SerializeField]   private float lifetime;
[SerializeField]    Vector3 n,w,e,s;
 [SerializeField]   Danu_FSM fsm;
[SerializeField]    List<GameObject> nblades = new List<GameObject>();
[SerializeField]    List<GameObject> wblades = new List<GameObject>();
 [SerializeField]   List<GameObject> eblades = new List<GameObject>();
  [SerializeField]  List<GameObject> sblades = new List<GameObject>();
 [SerializeField]   private bool wait;
[SerializeField]   private float waitTime;
 [SerializeField]   Pool pool;
    // Start is called before the first frame update
    void Start()
    {
        
            Init();
            SpawnBladesPreview();
        //activation des helices, ou instantiation si c'est la premiere fois
        waitTime = 0;
        wait = true;
    }
    void Init()
    {
        pool = GetComponent<Pool>();
        //setup des variables
        dist = fsm.GetP1Sp_Dist();
        globalGO = fsm.GetP1GlobalGO();
        bladesParentAttackData = globalGO.GetComponent<AttackData>();
        
        Transform[] nwesTrans = fsm.GetP1NWEMax();
        bladesPreview = fsm.GetBladesPreview();
        dSphereN = nwesTrans[0];
        dSphereE = nwesTrans[1];
        dSphereW = nwesTrans[2];
        dSphereS = nwesTrans[3];
        rotationSpeed = fsm.GetP1RotationSpeed();
        turningRight = fsm.GetP1TurningRight();
        maxWaitTime = fsm.GetP1MaxWaitTime();
        lifetime = fsm.GetP1SpinLifeTime();
    }
    // Update is called once per frame
    void Update()
    {
        if (wait)
        {
            waitTime += Time.deltaTime;
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
            Debug.Log("over");
            this.enabled=false;
        }
        Rotate();
    }

    private void SpawnBladesPreview()
    {
        Transform[] nwesTrans = fsm.GetP1NWEMax();
        Vector3 center = transform.position;
        preview.gameObject.SetActive(false);
        for (int i = 0; i < bladesPreview.Length; i++)
        {
            bladesPreview[i].gameObject.SetActive(true);
            bladesPreview[i].localScale = new Vector3(1, 1, Vector3.Distance(center, nwesTrans[i].position));
            bladesPreview[i].position = center + (nwesTrans[i].position - center) / 2;
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
            SetupBall(pool.SecondGet(), Vector3.Lerp(transform.position, dSphereN.position, delta * i), nblades);
            SetupBall(pool.SecondGet(), Vector3.Lerp(transform.position, dSphereW.position, delta * i), wblades);
            SetupBall(pool.SecondGet(), Vector3.Lerp(transform.position, dSphereE.position, delta * i), eblades);
            SetupBall(pool.SecondGet(), Vector3.Lerp(transform.position, dSphereS.position, delta * i), sblades);
        }
        globalGO.SetActive(true);
        bladesParentAttackData.GetChildrenHitboxes();
        bladesParentAttackData.SetupHitboxes();
        bladesParentAttackData.LaunchAttack();
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
                eblades.Add(fsm.InstantiateStaticProjectile(sPos));
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
    void End()
    {
        globalGO.SetActive(false);
        lifetime = fsm.GetP1SpinLifeTime();
        bladesParentAttackData.StopAttack();
        globalGO.transform.rotation = Quaternion.identity;
        float delta = 1 / dist;
        for (int i = 0; i < nblades.Count; i++)
        {
            nblades[i].SetActive(false);
            wblades[i].SetActive(false);
            eblades[i].SetActive(false);
            sblades[i].SetActive(false);
        }
    }
}

