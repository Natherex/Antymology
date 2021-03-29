using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class neuralManager : MonoBehaviour
{
    private int[] layers = new int[] { 3, 10, 10, 1 };
    private NeuralNetwork current;
    private NeuralNetwork previous;

    public int getMove(float xToQueen, float yToQueen, float health )
    {
        float move = 0;
        float[] inputs = new float[]{xToQueen,yToQueen,health};
        move = current.FeedForward(inputs)[0];
        
        if(move < 0.16667)
            return 0;
        if(move < 0.33333)
            return 1;
        if(move < 0.5)
            return 2;
        if(move < 0.66667)
            return 3;
        if(move < 0.83333)
            return 4;
        else
            return 5;    
    }

    // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
