using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
public class Laser : MonoBehaviour
{
    [SerializeField]private int firstShakeTime = 5;
    [SerializeField]private int secondShakeTime = 5;
    [SerializeField]private int firstShakeForce = 5;
    [SerializeField]private int secondShakeForce = 20;
    [SerializeField]List<Transform> children=new List<Transform>();
    [SerializeField]float maxRange;
    [SerializeField] float range;
    [SerializeField] GameObject laserTip;
    float t;
    [SerializeField] float speed;
    private float size;
    float delay;
    [SerializeField] GameObject gather;
    [SerializeField] GameObject gather1;
    [SerializeField] AttackData aData;
    [SerializeField] float lifetime;
    [SerializeField] List<ParticleSystem> vfx;
    [SerializeField] Volume _volume;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject preview;
    // Start is called before the first frame update
    [Button]
    void Start()
    {
        var main = this.GetComponent<ParticleSystem>().main;
        delay=main.startDelay.constant;
                StartShaking(firstShakeTime,firstShakeForce);
        preview.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        ShakeUpdate();
        if (delay>=0)
        {
            Delay();
            return;
        }
        MoveTip();
        Live();
        
    }
    void Delay()
    {
            delay-=Time.deltaTime;
            if (delay<=0)
            {
                preview.SetActive(false);
                StartShaking(lifetime, secondShakeForce);
                gather.SetActive(false);
                gather1.SetActive(false);
                aData.LaunchAttack();
            }


    }
    void Live()
    {
        lifetime-=Time.deltaTime;
        if (lifetime<=0)
        {
            aData.StopAttack();
            foreach(ParticleSystem ps in vfx)
            {
                ps.Stop();
            }
        }
    }

    private void ExtendHitbox()
    {
        float delta = 1f/children.Count;
        for (int i=0;i<children.Count;i++)
        {
            float nX=Mathf.Lerp(0,range*2.8f,delta*i);
            children[i].localPosition=new Vector3(0,0,nX);
        }  
    }

    void MoveTip()
    {
        if (laserTip.transform.localPosition.x<=maxRange)
        {
            t+=Time.deltaTime*speed;
            Debug.Log(t);
            float nX=Mathf.Lerp(0,maxRange,t);
            range=nX;
            Vector3 nPos=new Vector3(nX,0,0);
            laserTip.transform.localPosition=nPos;
            ExtendHitbox();
        }
    }
    #region Shake
    private List<ShakeConfig> m_configs = new List<ShakeConfig>();
	private bool m_isShaking = false;
	private Vector3 m_originalPos = Vector3.zero;

	public void StartShaking( float time, float force ) {
		this.m_originalPos = cam.transform.position;

		ShakeConfig config = new ShakeConfig( time, force );
		this.m_configs.Add( config );
		this.m_isShaking = true;
	}
	
	// Update is called once per frame
	void ShakeUpdate () {
		if( !this.m_isShaking ) {
			return;
		}

		int len = this.m_configs.Count;
		if( len == 0 ) {
			// return to our default position
			cam.transform.position = this.m_originalPos;
			this.m_isShaking = false;
			return;
		}
		
		// get the stongest shake
		float force = 0.0f;
		for( int i = len - 1; i >= 0; i-- ) {
			ShakeConfig config = this.m_configs[i];
			if( config.force > force ) {
				force = config.force;
			}
			config.time -= Time.deltaTime;
			if( config.time <= 0.0f ) {
				this.m_configs.RemoveAt( i );
			}
		}

        // pick a random force
        float forceRight = Random.Range( -force, force );
        float forceUp = Random.Range( -force, force );

        // get our move direction based on our force
        Vector3 moveRight = this.transform.right * forceRight;
        Vector3 moveUp = this.transform.up * forceUp;

		cam.transform.position = this.m_originalPos + moveRight + moveUp;
	}
}

class ShakeConfig {
	public float time;
	public float force;
	public ShakeConfig( float time, float force ) {
		this.time = time;
		this.force = force;
	}
    #endregion
}
