using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SimpleImage : Graphic
{
    VertexHelper vh;
    protected override void OnPopulateMesh(VertexHelper vh)
    {

        this.vh = vh;
        Vector2 corner1 = Vector2.zero;
        Vector2 corner2 = Vector2.zero;

        corner1.x = 0.4f;
        corner1.y = 0.4f;
        corner2.x = 0.8f;
        corner2.y = 0.8f;

        corner1.x -= rectTransform.pivot.x;
        corner1.y -= rectTransform.pivot.y;
        corner2.x -= rectTransform.pivot.x;
        corner2.y -= rectTransform.pivot.y;

        corner1.x *= rectTransform.rect.width;
        corner1.y *= rectTransform.rect.height;
        corner2.x *= rectTransform.rect.width;
        corner2.y *= rectTransform.rect.height;

        this.vh.Clear();

        UIVertex vert = UIVertex.simpleVert;

        vert.position = new Vector2(corner1.x, corner1.y);
        vert.color = color;
        this.vh.AddVert(vert);

        vert.position = new Vector2(corner1.x, corner2.y );
        vert.color = color;
        this.vh.AddVert(vert);

        vert.position = new Vector2(corner2.x, corner2.y);
        vert.color = color;
        this.vh.AddVert(vert);

        vert.position = new Vector2(corner2.x , corner1.y);
        vert.color = color;
        this.vh.AddVert(vert);

        this.vh.AddTriangle(0, 1, 2);
        this.vh.AddTriangle(2, 3, 0);
    }

   
    public void CustomShapeSetup()
    {

        SetVerticesDirty();

        Vector2 corner1 = Vector2.zero;
        Vector2 corner2 = Vector2.zero;

        corner1.x = 0f;
        corner1.y = 0f;
        corner2.x = 1f;
        corner2.y = 1f;

        corner1.x -= rectTransform.pivot.x;
        corner1.y -= rectTransform.pivot.y;
        corner2.x -= rectTransform.pivot.x;
        corner2.y -= rectTransform.pivot.y;

        corner1.x *= rectTransform.rect.width;
        corner1.y *= rectTransform.rect.height;
        corner2.x *= rectTransform.rect.width;
        corner2.y *= rectTransform.rect.height;

        this.vh.Clear();

        UIVertex vert = UIVertex.simpleVert;

        vert.position = new Vector2(corner1.x, corner1.y);
        vert.color = color;
        this.vh.AddVert(vert);

        vert.position = new Vector2(corner1.x, corner2.y);
        vert.color = color;
        this.vh.AddVert(vert);

        vert.position = new Vector2(corner2.x, corner2.y);
        vert.color = color;
        this.vh.AddVert(vert);

        vert.position = new Vector2(corner2.x, corner1.y);
        vert.color = color;
        this.vh.AddVert(vert);

        this.vh.AddTriangle(0, 1, 2);
        this.vh.AddTriangle(2, 3, 0);

        SetVerticesDirty();
    }


}