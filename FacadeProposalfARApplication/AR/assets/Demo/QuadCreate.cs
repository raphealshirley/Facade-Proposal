using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuadCreate
{
    public static bool realWorld = true;
    public static GameObject CreateQuad(float x1_screen, float y1_screen, float x2_screen, float y2_screen, float x3_screen, float y3_screen, float x4_screen, float y4_screen, float z_screen)
    {
        GameObject go = new GameObject("Quad");
        MeshFilter mf = go.AddComponent(typeof(MeshFilter)) as MeshFilter;
        MeshRenderer mr = go.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

        //order of the message: top left top right bottom left bottom right

        Vector3 bottomLeft_screen = new Vector3(x3_screen,y3_screen,z_screen);
        Vector3 bottomRight_screen = new Vector3(x4_screen, y4_screen, z_screen);
        Vector3 topLeft_screen = new Vector3(x1_screen, y1_screen, z_screen);
        Vector3 topRight_screen = new Vector3(x2_screen, y2_screen, z_screen);

        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(bottomLeft_screen);
        Vector3 bottomRight = Camera.main.ScreenToWorldPoint(bottomRight_screen);
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(topLeft_screen);
        Vector3 topRight = Camera.main.ScreenToWorldPoint(topRight_screen);

        Mesh m = new Mesh();
        m.vertices = new Vector3[]
        {
            new Vector3(bottomLeft.x,bottomLeft.y,bottomLeft.z),
            new Vector3(bottomRight.x,bottomRight.y,bottomRight.z),
            new Vector3(topLeft.x,topLeft.y,topLeft.z),
            new Vector3(topRight.x,topRight.y,topRight.z)

        };

        m.uv = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(1,1)

        };
        //m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        m.triangles = new int[] { 0, 2, 1, 2, 3, 1 };


        mf.mesh = m;
        m.RecalculateBounds();
        m.RecalculateNormals();

        go.tag = "Facade";

        if (realWorld == false)
        {
            go.transform.parent = Camera.main.transform;
        }


        return go;
    }
}
