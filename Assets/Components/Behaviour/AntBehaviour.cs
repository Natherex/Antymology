using Antymology.Helpers;
using Antymology.Terrain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AntBehaviour : MonoBehaviour
{
    private int health = 100;
    public int seed;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(movement());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator movement()
    {
        var rand = new System.Random(seed);
        while(true)
        {
            int nextMove = rand.Next(0,4);
            yield return new WaitForSeconds(1);   
            int currentX = (int)this.transform.position.x;
            int currentY = (int)this.transform.position.y;
            int currentZ = (int)this.transform.position.z;
            int movementHeight;
            switch(nextMove)
            {
                case 0:
                        movementHeight = validMovement(currentX+1,currentY,currentZ);
                        if(movementHeight != 9)
                            this.transform.position += new Vector3(1,movementHeight ,0);
                break;
                case 1:
                        movementHeight = validMovement(currentX,currentY,currentZ+1);
                        if(movementHeight != 9)
                            this.transform.position += new Vector3(0,movementHeight ,1);
                break;
                case 2:
                        movementHeight = validMovement(currentX-1,currentY,currentZ);
                        if(movementHeight != 9)
                            this.transform.position += new Vector3(-1,movementHeight ,0);
                break;
                case 3:
                        movementHeight = validMovement(currentX,currentY,currentZ-1);
                        if(movementHeight != 9)
                            this.transform.position += new Vector3(0,movementHeight ,-1);
                break;
            }
        }
    }

    /// <summary>
    ///Tells ant how to move in given direction
    ///return 1 if ant is to move up a block
    ///return -1 if ant is to move down a block
    ///return 0 if ant is to stay level
    ///return 9 if cant legally move
    ///</summary>
    private int validMovement(int x, int y, int z)
    {
        if(WorldManager.Instance.GetBlock(x,y,z).Name.Equals("Air") && !WorldManager.Instance.GetBlock(x,y-1,z).Name.Equals("Air") )
            return 0;
        if(WorldManager.Instance.GetBlock(x,y+1,z).Name.Equals("Air") && !WorldManager.Instance.GetBlock(x,y,z).Name.Equals("Air") )
            return 1;
        if(WorldManager.Instance.GetBlock(x,y-1,z).Name.Equals("Air") && !WorldManager.Instance.GetBlock(x,y-2,z).Name.Equals("Air") 
            && WorldManager.Instance.GetBlock(x,y,z).Name.Equals("Air") )
            return -1;
        return 9;
    }
}
