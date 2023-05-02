using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    //Internal camera reference
    public Camera camera;

    //Player reference
    public GameObject player;

    //Shake
    private bool shaking; 
    public float maxShakeDuration = 1f;
    public int numberOfShakes = 4;
    public float shakeSpeed=3;
    public Vector3 maxShakeRange;

    //Rotation
    private bool rotation;
    public float maxAngle = 5;
    public int numberOfRotations = 4;
    public float maxRotationDuration = 1f;

    //Zoom
    private bool zooming; 
    private float defaultSize = 10f;
    public float minSize=2f;//How close can we possibly get
    public float zoomInDuration=1f;
    public float zoomOutDuration=1f;

    //Private information
    private bool zoomingIn = true;
    private float zoomTimer = 0f;
    private float shakeTimer = 0f;
    private float rotationTimer = 0f;
    private int currentShakeNumber = 0;
    private int currentRotationNumber = 0;
    private Vector3 defaultPosition;
    private float defaultRotation;
    private float targetRotation;
    private float prevZ; 
    private Vector3 currentShakeDirection;

    // Start is called before the first frame update
    void Start()
    {
        defaultSize = camera.orthographicSize;
        defaultPosition = transform.position;
        defaultRotation= transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        //Zooming Code
        if(zooming){
            Vector3 playerPos = player.transform.position;
            playerPos.z=defaultPosition.z;
            if(zoomingIn){
                zoomTimer+=Time.deltaTime;

                if(zoomTimer<zoomInDuration){
                    //Just linear, feel free to get more fancy with it if you like
                    camera.orthographicSize = defaultSize-(defaultSize-minSize)*zoomTimer/zoomInDuration;
                    transform.position = Vector3.Lerp(defaultPosition,playerPos,zoomTimer/zoomInDuration);
                }
                else{
                    zoomTimer = 0f;
                    zoomingIn = false;
                    camera.orthographicSize = minSize;
                    transform.position = playerPos;
                }
            }
            else{
                zoomTimer+=Time.deltaTime;
                if(zoomTimer<zoomOutDuration){
                    //Just linear, feel free to get more fancy with it if you like
                    camera.orthographicSize = minSize+(defaultSize-minSize)*zoomTimer/zoomOutDuration;
                    transform.position = Vector3.Lerp(playerPos,defaultPosition,zoomTimer/zoomOutDuration);
                }
                else{
                    zoomTimer = 0f;
                    zooming = false;
                    camera.orthographicSize = defaultSize;
                    transform.position = defaultPosition;
                }
            }
        }

        //Shaking Code
        if(shaking){
            Debug.Log("Shaking");
            
            shakeTimer+=Time.deltaTime;

            if(shakeTimer<maxShakeDuration/numberOfShakes){
                transform.position+=(currentShakeDirection)*Time.deltaTime*shakeSpeed;
            }
            else{
                currentShakeNumber+=1;
                if(currentShakeNumber<numberOfShakes-1){
                    if(currentShakeNumber%2==1){
                        currentShakeDirection*= -1;
                    }
                    else{
                       currentShakeDirection = Vector3.right*Random.Range(0,maxShakeRange.x)+Vector3.up*Random.Range(0,maxShakeRange.y)+Vector3.forward*Random.Range(0,maxShakeRange.z); 
                    }

                    shakeTimer = 0;
                }
                else if(currentShakeNumber==numberOfShakes-1){
                    currentShakeDirection = defaultPosition-transform.position;
                    shakeTimer = 0;
                }
                else{
                    shaking = false;
                    currentShakeDirection = Vector3.zero;
                    shakeTimer=0;
                    currentShakeNumber = 0;
                    transform.position = defaultPosition;
                }
            }
        }

        //Rotation Code
        if(rotation){
            rotationTimer+=Time.deltaTime;

            if(rotationTimer<maxRotationDuration/numberOfRotations){
                Vector3 newRotation = transform.eulerAngles;
                newRotation.z += (targetRotation-defaultRotation)*(rotationTimer/(maxRotationDuration/numberOfRotations));
                transform.eulerAngles = newRotation;
            }
            else{
                if(currentRotationNumber<numberOfRotations-1){
                    currentRotationNumber+=1;
                    prevZ = transform.eulerAngles.z;
                    if(targetRotation-defaultRotation<0){
                        targetRotation = defaultRotation+Random.Range(0,maxAngle);
                    }
                    else{
                        targetRotation = defaultRotation+Random.Range(-1*maxAngle,0);
                    }
                    
                    rotationTimer = 0f;
                }
                else if(currentRotationNumber==numberOfRotations-1){
                    currentRotationNumber+=1;
                    prevZ = transform.eulerAngles.z;
                    targetRotation = defaultRotation;
                    rotationTimer = 0f;
                }
                else{
                    rotation = false;
                    rotationTimer = 0f;
                    currentRotationNumber = 0;
                    transform.eulerAngles = new Vector3(0,0,defaultRotation);
                    prevZ = transform.eulerAngles.z;

                }
            }
            

        }
    }

    public void ZoomOnPlayer(){
        zooming= true;
        zoomingIn=true;
        zoomTimer = 0f;
    }

    public void ShakeCamera(){
        shaking = true;
        currentShakeDirection = Vector3.right*Random.Range(0,maxShakeRange.x)+Vector3.up*Random.Range(0,maxShakeRange.y)+Vector3.forward*Random.Range(0,maxShakeRange.z);
        shakeTimer=0;
        currentShakeNumber = 0;
    }

    public void RotateCamera(){
        rotation = true;
        rotationTimer = 0f;
        currentRotationNumber = 0;
        prevZ = transform.eulerAngles.z;
        targetRotation = defaultRotation+Random.Range(-1*maxAngle,maxAngle);

    }
}
