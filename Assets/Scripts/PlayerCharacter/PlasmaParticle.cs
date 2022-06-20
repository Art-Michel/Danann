using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlasmaParticle : PooledObject
{
    Transform destination;
    Vector2 vDest;
    Vector2 origin;
    Vector2 p1;

    [SerializeField] GameObject particle;

    public int plasmaToAdd;
    public GameObject player;

    GameObject[] medianPoints;

    [SerializeField] Vector2 MinMaxSpeed;
    float speed;

    float offset;
    float t = 0;

    // Start is called before the first frame update
    void Start()
    {
        destination = GameObject.Find("ParticlePoint").transform;
        gameObject.transform.SetParent(destination);
        vDest = new Vector2(destination.position.x, destination.position.y);

        int i = Random.Range(0, 4);
        medianPoints = GameObject.FindGameObjectsWithTag("Median");
        p1 = medianPoints[i].transform.position;

        origin = gameObject.transform.position;

        offset = Random.Range(-2.5f, 2.5f);
        speed = Random.Range(MinMaxSpeed.x, MinMaxSpeed.y);
    }

    void Update()
    {
        if (t <= 1)
        {
            t += Time.deltaTime * speed;
            transform.position = BezierCurve();
            vDest = new Vector2(destination.position.x + offset, destination.position.y);
        }
        else
        {
            Hit();
        }
    }

    void Hit()
    {
        Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    Vector2 BezierCurve()
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector2 point = uu * origin;
        point += 2 * u * t * p1;
        point += tt * vDest;
        return point;
    }
}