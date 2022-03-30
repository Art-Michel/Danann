using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;


public class AIAgent : MonoBehaviour
{

    public Transform[] path;
    public float minDistBeforePathChange = 0.5f;
    public Transform target;
    [HideInInspector] public Vector3 targetLastPos;

    [Header("External view cone")]
    public LineRenderer externalViewLine = null;
    public float externalViewDist = 20.0f;
    public float externalViewFOV = 120.0f;

    [Header("Internal view cone")]
    public LineRenderer internalViewLine = null;
    public float internalViewDist = 20.0f;
    public float internalViewFOV = 120.0f;

    [Range(1, 90)] public int viewSegments = 12;

    private FSM m_fsm = null;
    private LineRenderer m_line = null;

    public NavMeshAgent navAgent { get; private set; }

    //public Weapon weapon { get; private set; }

    //public Reticule reticule = null;

    // Start is called before the first frame update
    void Start()
    {
        this.navAgent = this.gameObject.GetComponent<NavMeshAgent>();
      //  this.weapon = this.gameObject.GetComponent<Weapon>();
        this.m_line = this.gameObject.GetComponent<LineRenderer>();

        this.m_fsm = this.gameObject.GetComponent<FSM>();
        this.m_fsm.agent = this;

        this.m_fsm.AddState( new AIStatePatrol() );
        this.m_fsm.AddState( new AIStateInvestigate() );
        this.m_fsm.AddState( new AIStateAttack() );
        this.m_fsm.AddState( new AIStateOnLoseSight() );
        this.m_fsm.AddState( new AIStateLookAround() );

        this.m_fsm.ChangeState( StateNames.P1IDLE);
    }

    public void SetConeColour(Color c ) {
        this.externalViewLine.startColor = this.externalViewLine.endColor = c;
        this.internalViewLine.startColor = this.internalViewLine.endColor = c;
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetMouseButtonDown(0)) {
        //     Vector3 pos = Input.mousePosition;
        //     RaycastHit hit;
        //     Ray ray = Camera.main.ScreenPointToRay(pos, Camera.MonoOrStereoscopicEye.Mono);
        //     Physics.Raycast(ray, out hit);

        //     this.m_navAgent.destination = hit.point;
        // }

        if(this.navAgent.hasPath) {
            // debug visual
            NavMeshPath path = this.navAgent.path;
            Vector3[] corners = path.corners;

            int len = corners.Length;
            for( int i = 0; i < len; i++ ) {
                corners[i].y += 1.0f;
            }

            this.m_line.positionCount = len;
            this.m_line.SetPositions(corners);
        }
    }

    void OnValidate() {
        this._drawView(this.externalViewLine, this.viewSegments, this.externalViewFOV, this.externalViewDist);
        this._drawView(this.internalViewLine, this.viewSegments, this.internalViewFOV, this.internalViewDist);
    }

    /*public CanSeeStatus CanSee(Transform target) {
        // if the target is dead/not visible, we see
        if( !target.gameObject.activeSelf ) {
            return CanSeeStatus.CANT_SEE;
        }

        // check external radius
        if( Vector3.Distance(this.transform.position, target.position) > this.externalViewDist ) {
            return CanSeeStatus.CANT_SEE; // outside external radius - can't see
        }

        // check external angle
        Vector3 toTargetDir = (target.position - this.transform.position).normalized;
        float dot = Vector3.Dot( toTargetDir, this.transform.forward );
        float degrees = Mathf.Acos( dot ) * Mathf.Rad2Deg;
        if ( degrees > this.externalViewFOV * 0.5f ) {
            return CanSeeStatus.CANT_SEE; // outside of our external fov, can't see
        }

        // check if the target is inside our internal cone
        bool isInsideInternal = false;
        if( Vector3.Distance(this.transform.position, target.position) < this.internalViewDist ) { // internal distance
            if ( degrees < this.internalViewFOV * 0.5f ) { // internal fov
                isInsideInternal = true;
            }
        }

        // raycast (NOTE: this is going from our position, probably better if it went from our eyes)
        Vector3 dir = (target.position - this.transform.position);
        RaycastHit hit;
        bool hitAnything = Physics.Raycast(this.transform.position, dir.normalized, out hit, dir.magnitude);
        if( hitAnything &&
            hit.collider.gameObject != this.target.gameObject &&
            Vector3.Distance(this.transform.position, hit.point) < Vector3.Distance(this.transform.position, target.position)) {
            return CanSeeStatus.CANT_SEE; // they're behind something
        }

        // we can see the target, so return whether they're inside the internal cone, or the external one
        if( isInsideInternal ) {
            return CanSeeStatus.CAN_SEE;
        }
        return CanSeeStatus.CAN_KINDA_SEE;
    }*/

    private void _drawView(LineRenderer lineRenderer, int segments, float fov, float dist) {
        lineRenderer.positionCount = segments + 2; // +1 for the center, +1 for the last arc point
        lineRenderer.loop = true; // loop as we want to go back to the center
        lineRenderer.useWorldSpace = false; // points are local; we want to only generate once
        lineRenderer.alignment = LineAlignment.TransformZ;

        float fovRads = fov * Mathf.Deg2Rad;
        float startRad = -fovRads * 0.5f;
        float rads = fovRads / segments;
        float y = 0.0f;
        lineRenderer.SetPosition( 0, new Vector3( 0.0f, y, 0.0f ) );
        for( int i = 0; i < segments + 1; i++ ) {
            float x = Mathf.Sin( startRad + rads * i ) * dist;
            float z = Mathf.Cos( startRad + rads * i ) * dist;
            lineRenderer.SetPosition( i + 1, new Vector3( x, y, z ) );
        }
    }
}
