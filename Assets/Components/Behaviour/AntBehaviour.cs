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
    private int movementCost = 10;
    private int movementPenalty = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(actions());
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator actions()
    {
        var rand = new System.Random(seed);
        while(true)
        {
            yield return new WaitForSeconds(1); 
            if (health == 0)
            {
                Destroy(gameObject);
            }

            blockInteraction();
            movement(rand.Next(0,4));
        }
    }
    private void blockInteraction()
    {
            int currentX = (int)this.transform.position.x;
            int currentY = (int)this.transform.position.y;
            int currentZ = (int)this.transform.position.z;
            if(WorldManager.Instance.GetBlock(currentX,currentY,currentZ).Name.Equals("Acid"))
            {
                movementPenalty = movementCost;
            }else
                movementPenalty = 0;

            if(WorldManager.Instance.GetBlock(currentX,currentY-1,currentZ).Name.Equals("Mulch") && health <50)
            {
                WorldManager.Instance.SetBlock(currentX,currentY-1,currentZ,new AirBlock());
                this.transform.position += new Vector3(0,-1 ,0);
                health +=50;
            }
    }
    private void movement(int nextMove)
    {
            var rand = new System.Random(seed);
            int currentX = (int)this.transform.position.x;
            int currentY = (int)this.transform.position.y;
            int currentZ = (int)this.transform.position.z;
            int movementHeight;
            Debug.Log(nextMove);
            switch(nextMove)
            {
                case 0:
                        movementHeight = validMovement(currentX+1,currentY,currentZ);
                        if(movementHeight != 9)
                        {
                            this.transform.position += new Vector3(1,movementHeight ,0);
                            health -=movementCost+movementPenalty;
                        }
                break;
                case 1:
                        movementHeight = validMovement(currentX,currentY,currentZ+1);
                        if(movementHeight != 9)
                        {
                            this.transform.position += new Vector3(0,movementHeight ,1);
                            health -=movementCost+movementPenalty;
                        }
                break;
                case 2:
                        movementHeight = validMovement(currentX-1,currentY,currentZ);
                        if(movementHeight != 9)
                        {
                            this.transform.position += new Vector3(-1,movementHeight ,0);
                            health -=movementCost+movementPenalty;
                        }
                break;
                case 3:
                        movementHeight = validMovement(currentX,currentY,currentZ-1);
                        if(movementHeight != 9)
                        {
                            this.transform.position += new Vector3(0,movementHeight ,-1);
                            health -=movementCost+movementPenalty;
                        }
                break;
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
