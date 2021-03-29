using Antymology.Helpers;
using Antymology.Terrain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenBehaviour : MonoBehaviour
{
    private int maxHealth = 100;
    private int health = 100;
    private int limit = 50;
    public int seed;
    private int turnCost = 2;
    private int turnPenalty = 0;
    // Start is called before the first frame update
    private Boolean toRight = true;
    void Start()
    {
        StartCoroutine(actions());
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnCollisionEnter(Collision collision)
    {

        AntBehaviour ant = collision.gameObject.GetComponent<AntBehaviour>();
        Debug.Log(health);
        if(ant.health > 20)
        {
            int removed = ant.health - 20;
            health = health + removed;
            ant.health = ant.health - removed;
            if(health>maxHealth)
            {
                removed = maxHealth-health;
                health = health - removed;
                ant.health = ant.health + removed;
            }
        }
    }
    void OnTriggerExit(Collider collision)
    {
        AntBehaviour ant = collision.gameObject.GetComponent<AntBehaviour>();
        if(ant.health > 20)
        {
            int removed = ant.health - 20;
            health = health + removed;
            ant.health = ant.health - removed;
            if(health>maxHealth)
            {
                removed = health - maxHealth;
                health = health - removed;
                ant.health = ant.health + removed;
            }
        }
    }
    IEnumerator actions()
    {
        var rand = new System.Random(seed);
        while(true)
        {
            yield return new WaitForSeconds(1); 
            if (health == 0)
            {
                WorldManager.Instance.queensHealth = 0;
                WorldManager.Instance.generation();
                Destroy(gameObject);
            }else if(health >= limit)
            {
                int currentX = (int)this.transform.position.x;
                int currentY = (int)this.transform.position.y;
                int currentZ = (int)this.transform.position.z;
                if(toRight && validMovement(currentX+1,currentY,currentZ) !=9 )
                {
                    movement(9);
                    movement(0);
                    ConfigurationManager.Instance.nestBlocksPlaced++;
                    health = health - (maxHealth/3);
                }else if(validMovement(currentX-1,currentY,currentZ) !=9 )
                {
                    toRight = false;
                    movement(9);
                    movement(2);
                    ConfigurationManager.Instance.nestBlocksPlaced++;
                    health = health - (maxHealth/3);
                }else if(validMovement(currentX,currentY,currentZ+1) !=9 )
                {
                    toRight = true;

                    movement(9);
                    movement(1);
                    ConfigurationManager.Instance.nestBlocksPlaced++;
                    health = health - (maxHealth/3);
                }else if(validMovement(currentX,currentY,currentZ-1) !=9 )
                {
                    toRight = true;
                    
                    movement(9);
                    movement(3);
                    ConfigurationManager.Instance.nestBlocksPlaced++;
                    health = health - (maxHealth/3);
                }else
                {
                    WorldManager.Instance.SetBlock(currentX,currentY,currentZ,new StoneBlock());
                    this.transform.position += new Vector3(0,1 ,0);
                    toRight = true;
                    movement(rand.Next(0,4));
                }
            }
            blockInteraction();
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

            WorldManager.Instance.queensX = currentX;
            WorldManager.Instance.queensY = currentY;
            WorldManager.Instance.queensZ = currentZ;
            WorldManager.Instance.queensHealth = health;

    }
    /*
        Takes in an integer and performs the corresponding action if it is valid
        0 - move 1 tile in x axis
        1 - move 1 tile in z axis
        2 - move -1 tile in x axis
        3 - move -1 tile in z axis
        4 - eat mulch tile
        5 - dig up one tile
        9 - place nest block
    */
    private void movement(int nextMove)
    {
            int currentX = (int)this.transform.position.x;
            int currentY = (int)this.transform.position.y;
            int currentZ = (int)this.transform.position.z;
            int movementHeight;

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
                case 9:
                        WorldManager.Instance.SetBlock(currentX,currentY,currentZ,new NestBlock());
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
