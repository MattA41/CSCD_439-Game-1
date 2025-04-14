using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDrawer : MonoBehaviour
{

    public float radius = 3f;
    public int segments = 60;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        DrawCircle();
    }

    private void DrawCircle()
    {
        lineRenderer.positionCount = segments + 1;
        lineRenderer.loop = true;

        Vector3[] points = new Vector3[segments + 1];

        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            points[i] = new Vector3(x, y, 0);
            angle += 2 * Mathf.PI / segments;
        }

        lineRenderer.SetPositions(points);
    }

     public void SetRadius(float newRadius)
    {
        radius = newRadius;
        DrawCircle();
    }
}
