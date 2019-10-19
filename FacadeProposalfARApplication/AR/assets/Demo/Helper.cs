using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static GameObject CreatePlane(float width, float height)
    {
        GameObject go = new GameObject("Quad");
        MeshFilter mf = go.AddComponent(typeof(MeshFilter)) as MeshFilter;
        MeshRenderer mr = go.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

        Mesh m = new Mesh();
        m.vertices = new Vector3[]
        {
            new Vector3(0,0,3),
            new Vector3(width,height,3),
            new Vector3(0,height,3),
            new Vector3(width,height,3)

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

        return go;
    }
}
