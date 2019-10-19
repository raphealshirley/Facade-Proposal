using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nabood : MonoBehaviour
{
    private GameObject[] gameObjects;
    public void DestroyAllObjects()
    {
        gameObjects = GameObject.FindGameObjectsWithTag("Facade");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }
}