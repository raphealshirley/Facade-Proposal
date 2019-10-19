using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTransparacy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private GameObject[] gameObjects;
    public void AdjustTransparancy()
    {

        Material newMat = Resources.Load("overlay", typeof(Material)) as Material;

        gameObjects = GameObject.FindGameObjectsWithTag("Facade");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].GetComponent<Renderer>().material = newMat;
        }
    }
}
