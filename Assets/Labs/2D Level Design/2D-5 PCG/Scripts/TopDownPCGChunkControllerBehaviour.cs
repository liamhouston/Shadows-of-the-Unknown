using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPCGChunkControllerBehaviour : MonoBehaviour
{
    // controls for where the chunk can be placed by the generator
    public bool leftOkay = false; // whether its allowed to be on the left half
    public bool rightOkay = false; // above, but for right half
    public bool verticalFlipOkay = false; // whether its allowed to be vertically flipped
    public bool horizontalFlipOkay = false; // above, but horizontal

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool getLeftOkay(){
        return leftOkay;
    }

    public bool getRightOkay(){
        return rightOkay;
    }

    public bool getVerticalFlipOkay(){
        return verticalFlipOkay;
    }

    public bool getHorizontalFlipOkay(){
        return horizontalFlipOkay;
    }
}
