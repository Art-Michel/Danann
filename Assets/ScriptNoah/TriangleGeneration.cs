using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class TriangleGeneration : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform lSpear;
    [SerializeField] Transform rSpear;
    [SerializeField] Material triMat;
    GameObject latestTriangle;
    Mesh latestMesh;
    MeshFilter latestFilter;
    bool move;
    [SerializeField] Transform target;
    // Start is called before the first frame update
    [Button]
    void Begin()
    {
        Vector3[] vertices=new Vector3[3];
        Vector2[] uv=new Vector2[3];
        int[] tri=new int[6];
        
        Vector3 playPos=new Vector3(player.position.x,player.position.y,player.position.z);
        Vector3 lSpPos=new Vector3(lSpear.position.x,player.position.y,lSpear.position.z);
        Vector3 rSpPos=new Vector3(rSpear.position.x,player.position.y,rSpear.position.z);
        vertices[0]=playPos;
        vertices[1]=lSpPos;
        vertices[2]=rSpPos;

        uv[0]=new Vector2(0,0);
        uv[1]=new Vector2(0,1);
        uv[2]=new Vector2(1,0);

        tri[0]=0;
        tri[1]=1;
        tri[2]=2;        
        tri[3]=2;
        tri[4]=1;
        tri[5]=0;

        Mesh mesh=new Mesh();
        mesh.vertices=vertices;
        mesh.uv=uv;
        mesh.triangles=tri;
        GameObject go=new GameObject("Tri",typeof(MeshFilter),typeof(MeshRenderer));
        go.transform.localScale=new Vector3( 1,1,1);
        go.GetComponent<MeshRenderer>().material=triMat;

        latestFilter=go.GetComponent<MeshFilter>();
        latestFilter.mesh=mesh;
        latestMesh=mesh;
        latestTriangle=go;

        move=true;
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {        
        Vector3 playPos=new Vector3(player.position.x,0,player.position.z);
        Vector3 lSpPos=new Vector3(lSpear.position.x,0,lSpear.position.z);
        Vector3 rSpPos=new Vector3(rSpear.position.x,0,rSpear.position.z);
        Vector3[] vertices=new Vector3[3];

        vertices[0]=playPos;
        vertices[1]=lSpPos;
        vertices[2]=rSpPos;

        latestMesh.vertices=vertices;
        }
    }
    [Button]
    void CheckIsIn()
    {
        float A = Area(player.position,lSpear.position,rSpear.position);
        Debug.Log(A);
        float A1 = Area(target.position,lSpear.position,rSpear.position);
        Debug.Log(A1);
        float A2 = Area(target.position,player.position,rSpear.position);
        Debug.Log(A2);
        float A3 = Area(target.position,lSpear.position,player.position);
        Debug.Log(A3);
        float Af =(A1+A2+A3);
        Debug.Log(Af);
        bool isIn = A==Af;
        Debug.Log(isIn);
    }
    float Area(Vector3 A,Vector3 B, Vector3 C)
    {
    float x1=A.x;
    float y1=A.z;
    float x2=B.x;
    float y2=B.z;
    float x3=C.x; 
    float y3=C.z;
    float area =(x1 * (y2 - y3) +x2 * (y3 - y1) + x3 * (y1 - y2)) / 2.0f;
    area=Mathf.Abs(area);
    return area;    
    }
}
