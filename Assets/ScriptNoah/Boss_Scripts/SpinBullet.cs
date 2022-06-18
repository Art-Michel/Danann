using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBullet : MonoBehaviour
{
    public enum bladeIndex{
        NORTH,
        WEST,
        EAST,
        SOUTH
    }
    P1DSpin state;
    int index;
    public int GetIndex(){return index;}
    bladeIndex blade;
    public bladeIndex GetBlade(){return blade;}
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Initialize(int newIndex,bladeIndex newblade)
    {
        index=newIndex;
        blade=newblade;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnShield()
    {
        state.Regenerate(this);
    }
}
