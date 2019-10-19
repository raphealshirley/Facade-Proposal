using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenWorld : MonoBehaviour
{
    Vector3 worldPos;
    float x;
    float y;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 tch = touch.position;
                tch.z = 15;
                Vector3 vec = Camera.main.ScreenToWorldPoint(tch);
                GameObject.Find("MySphere").transform.position = vec;
                GameObject.Find("field").GetComponentInChildren<Text>().text = string.Format("real: {0} {1} {2}", vec.x, vec.y, vec.z);
                GameObject.Find("fieldMouse").GetComponentInChildren<Text>().text = string.Format("mouse: {0} {1} {2}", tch.x, tch.y, tch.z);
            }
        }
    }

    public void DrawTest()
    {


    }
}
/*
worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
GameObject.Find("field").GetComponentInChildren<Text>().text = string.Format("real: {0} {1} {2}", worldPos.x, worldPos.y, worldPos.z);
GameObject.Find("fieldMouse").GetComponentInChildren<Text>().text = string.Format("mouse: {0} {1} {2}", Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
*/

