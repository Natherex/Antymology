using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    void OnGUI()
    {
        GUI.Box ( new Rect (150,50,150,30), "Nest Blocks: " + ConfigurationManager.Instance.nestBlocksPlaced);
    }

}