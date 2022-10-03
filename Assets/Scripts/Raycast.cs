using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    void DrawRay(Vector3 start, Vector3 dir, Color color)
    {
        Debug.DrawRay(start, dir, color);
    }
    
    public bool PlayerDetection(
        Vector3 raycastPosition, 
        float raycastDistance, 
        Vector3 raycastDirection, 
        LayerMask layerMask)
    {
        DrawRay(raycastPosition, raycastDirection * raycastDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, raycastDirection, raycastDistance, layerMask);
        if (hit)
        {
            return true;
        }
        return false;
    }

}
