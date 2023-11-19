using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundControl_0 : MonoBehaviour
{
    public GameObject backGroundColor;
    [Header("BackgroundNum 0 -> 3")]
    public int backgroundNum;
    public Sprite[] Layer_Sprites;
    private GameObject[] Layer_Object = new GameObject[5];
    private int max_backgroundNum = 3;
    void Start()
    {
        for (int i = 0; i < Layer_Object.Length; i++){
            Layer_Object[i] = GameObject.Find("Layer_" + i);
        }
        
    }

    void Update() {
        ChangeSprite();

        //for presentation without UIs
        if (backgroundNum == 0) backGroundColor.GetComponent<SpriteRenderer>().color = new Color(0.6666666f, 0.5843138f, 0.482353f);
        if (backgroundNum == 1) backGroundColor.GetComponent<SpriteRenderer>().color = new Color(0.5921569f, 0.4196079f, 0.4196079f);
        if (backgroundNum == 2) backGroundColor.GetComponent<SpriteRenderer>().color = new Color(0.3882353f, 0.3333333f, 0.3960785f);
        if (backgroundNum == 3) backGroundColor.GetComponent<SpriteRenderer>().color = new Color(0.4941177f, 0.4352942f, 0.5372549f);
    }

    void ChangeSprite(){
        Layer_Object[0].GetComponent<SpriteRenderer>().sprite = Layer_Sprites[backgroundNum*5];
        for (int i = 1; i < Layer_Object.Length; i++){
            Sprite changeSprite = Layer_Sprites[backgroundNum*5 + i];
            //Change Layer_1->7
            Layer_Object[i].GetComponent<SpriteRenderer>().sprite = changeSprite;
            //Change "Layer_(*)x" sprites in children of Layer_1->7
            Layer_Object[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = changeSprite;
            Layer_Object[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = changeSprite;
        }
    }

    public void NextBG(){
        backgroundNum = backgroundNum + 1;
        if (backgroundNum > max_backgroundNum) backgroundNum = 0;
        ChangeSprite();
    }
    public void BackBG(){
        backgroundNum = backgroundNum - 1;
        if (backgroundNum < 0) backgroundNum = max_backgroundNum;
        ChangeSprite();
    }
}
