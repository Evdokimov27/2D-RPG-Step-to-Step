using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground_0 : MonoBehaviour
{
    public bool Camera_Move;
    public float Camera_MoveSpeed = 1.5f;
    [Header("Layer Setting")]
    public float[] Layer_Speed = new float[5];
    public GameObject[] Layer_Objects = new GameObject[5];

    public Transform _camera;
    private float[] startPos = new float[5];
    private float boundSizeX;
    private float sizeX;
    private GameObject Layer_0;
    void Start()
    {

    }

    void Update(){
        if(_camera == null)_camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        //Moving camera
        if (Camera_Move){
        _camera.position += Vector3.right * Time.deltaTime * Camera_MoveSpeed;
        }
        for (int i=0;i<Layer_Objects.Length;i++){
            float temp = _camera.position.x * (1-Layer_Speed[i]);
            float distance = _camera.position.x  * Layer_Speed[i];
            Layer_Objects[i].transform.position = new Vector2 (startPos[i] + distance,0);
            if (temp > startPos[i] + boundSizeX*sizeX){
                startPos[i] += boundSizeX*sizeX;
            }else if(temp < startPos[i] - boundSizeX*sizeX){
                startPos[i] -= boundSizeX*sizeX;
            }
            
        }
    }
}
