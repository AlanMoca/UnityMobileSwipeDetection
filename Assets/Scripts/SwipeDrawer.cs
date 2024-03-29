﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float zOffset = 11;                             //distance of the camera and draw the lineRendered with the touch

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe; 
    }

    private void SwipeDetector_OnSwipe( SwipeData data )        //Set the positions converted from screen point to worldPoint (The positions that we can give to our line renderer)
    {
        Vector3[] positions = new Vector3[2];
        positions[0] = Camera.main.ScreenToWorldPoint( new Vector3( data.StartPosition.x, data.StartPosition.y, zOffset ) );
        positions[1] = Camera.main.ScreenToWorldPoint( new Vector3( data.EndPosition.x, data.EndPosition.y, zOffset ) );
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions( positions );
    }
}
