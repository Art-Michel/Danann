using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using NaughtyAttributes;
public class TriangleGeneration : LocalManager<TriangleGeneration>
{
    [SerializeField] Transform player;
    [SerializeField] Transform lSpear;
    [SerializeField] Transform rSpear;
    [SerializeField] Material triMat;
    GameObject latestTriangle;
    Mesh latestMesh;
    MeshFilter latestFilter;
    bool move;
    GameObject _go;

    [SerializeField] Transform target;

    [SerializeField] BossHealth _bossHealth;
    float _damageModifier;
    [SerializeField] ParticleSystem boomVFX;

    // Start is called before the first frame update
    [Button]
    public void Begin()
    {
        gameObject.SetActive(true);
        Vector3[] vertices = new Vector3[3];
        Vector2[] uv = new Vector2[3];
        int[] tri = new int[6];

        Vector3 playPos = new Vector3(player.position.x, player.position.y, player.position.z);
        Vector3 lSpPos = new Vector3(lSpear.position.x, player.position.y, lSpear.position.z);
        Vector3 rSpPos = new Vector3(rSpear.position.x, player.position.y, rSpear.position.z);
        
        vertices[0] = playPos;
        vertices[1] = lSpPos;
        vertices[2] = rSpPos;

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 0);

        tri[0] = 0;
        tri[1] = 1;
        tri[2] = 2;
        tri[3] = 2;
        tri[4] = 1;
        tri[5] = 0;

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = tri;
        _go = new GameObject("Tri", typeof(MeshFilter), typeof(MeshRenderer));
        _go.transform.localScale = new Vector3(1, 1, 1);
        _go.GetComponent<MeshRenderer>().material = triMat;

        latestFilter = _go.GetComponent<MeshFilter>();
        latestFilter.mesh = mesh;
        latestMesh = mesh;
        latestTriangle = _go;

        move = true;
    }
    [Button]
    public void Stop()
    {
        gameObject.SetActive(false);
        Destroy(_go);
        if (boomVFX!=null)
            boomVFX.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);
            Vector3 leftSpearPos = new Vector3(lSpear.position.x, 0, lSpear.position.z);
            Vector3 rightSpearPos = new Vector3(rSpear.position.x, 0, rSpear.position.z);
            Vector3[] vertices = new Vector3[3];

            vertices[0] = playerPos;
            vertices[1] = leftSpearPos;
            vertices[2] = rightSpearPos;

            latestMesh.vertices = vertices;
            _damageModifier = 60 / Area(player.position, lSpear.position, rSpear.position);
            _damageModifier = Mathf.Clamp(_damageModifier, 0.7f, 1.3f);

        }
    }
    [Button]
    public bool CheckIsIn()
    {
        float A = Area(player.position, lSpear.position, rSpear.position);
        float A1 = Area(target.position, lSpear.position, rSpear.position);
        float A2 = Area(target.position, player.position, rSpear.position);
        float A3 = Area(target.position, lSpear.position, player.position);
        float Af = (A1 + A2 + A3);
        return A < Af + 1f && A > Af - 1f;
    }

    public void BlowUpOnBoss()
    {
        _bossHealth.TakeDamage((float)System.Math.Round(80f * _damageModifier), Ccl_Attacks.TRIANGLEBOOM, 0);
        if (boomVFX!=null)
            boomVFX.Play();
    }

    public void TickOnBoss(float elapsedTime)
    {
        _bossHealth.TakeDamage((float)System.Math.Round(1f * _damageModifier * elapsedTime, 1), Ccl_Attacks.TRIANGLETICK, 0);
    }

    float Area(Vector3 A, Vector3 B, Vector3 C)
    {
        float x1 = A.x;
        float y1 = A.z;
        float x2 = B.x;
        float y2 = B.z;
        float x3 = C.x;
        float y3 = C.z;
        float area = (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2)) / 2.0f;
        area = Mathf.Abs(area);
        return area;
    }
}
