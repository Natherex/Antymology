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
    private int turnCost = 10;
    private int turnPenalty = 0;
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
            if(WorldManager.Instance.GetBlock(currentX,currentY-1,currentZ).Name.Equals("Acid"))
                turnPenalty = turnCost;
            else
                turnPenalty = 0;
            health -= (turnCost+turnPenalty);

    }
    /*
        Takes in an integer and performs the corresponding action if it is valid
        0 - move 1 tile in x axis
        1 - move 1 tile in z axis
        2 - move -1 tile in x axis
        3 - move -1 tile in z axis
        4 - eat mulch tile
        5 - dig up one tile
        7 - 
    */
    private void movement(int nextMove)
    {
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
                        }
                break;
                case 1:
                        movementHeight = validMovement(currentX,currentY,currentZ+1);
                        if(movementHeight != 9)
                        {
                            this.transform.position += new Vector3(0,movementHeight ,1);
                        }
                break;
                case 2:
                        movementHeight = validMovement(currentX-1,currentY,currentZ);
                        if(movementHeight != 9)
                        {
                            this.transform.position += new Vector3(-1,movementHeight ,0);
                        }
                break;
                case 3:
                        movementHeight = validMovement(currentX,currentY,currentZ-1);
                        if(movementHeight != 9)
                        {
                            this.transform.position += new Vector3(0,movementHeight ,-1);
                        }
                break;
                case 4:
                        if(WorldManager.Instance.GetBlock(currentX,currentY-1,currentZ).Name.Equals("Mulch") && health <50)
                        {
                            WorldManager.Instance.SetBlock(currentX,currentY-1,currentZ,new AirBlock());
                            this.transform.position += new Vector3(0,-1 ,0);
                            health +=50;
                        }
                break;
                case 5:
                        if(!WorldManager.Instance.GetBlock(currentX,currentY-1,currentZ).Name.Equals("Mulch") && !WorldManager.Instance.GetBlock(currentX,currentY-1,currentZ).Name.Equals("Container"))
                        {
                            WorldManager.Instance.SetBlock(currentX,currentY-1,currentZ,new AirBlock());
                            this.transform.position += new Vector3(0,-1 ,0);
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
