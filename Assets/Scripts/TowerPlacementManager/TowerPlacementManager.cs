using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementManager : MonoBehaviour
{
    public GameObject towerPrefab;

    private GameObject previewTower;


    private bool isDragging = false;

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!isDragging)
        {
            // Only check for click on the TowerSelector when we're not already dragging
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

                if (hit.collider != null && hit.collider.CompareTag("TowerSelector"))
                {
                    StartDragTower();
                }
            }
        }
        else
        {
            // Drag in progress: follow mouse
            if (previewTower != null)
            {
                previewTower.transform.position = mouseWorldPos;
            }

            // Place when mouse is released
            if (Input.GetMouseButtonUp(0))
            {
                PlaceTower();
            }
        }
    }

    private void StartDragTower()
    {
        isDragging = true;
        previewTower = Instantiate(towerPrefab);

        // Make preview transparent
        previewTower.GetComponent<Collider2D>().enabled = false;
        // previewTower.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f); // semi-transparent

        // Sync the radius visual to the collider radius
        // SyncRangeVisualToCollider(previewTower);

        Transform rangeVisual = previewTower.transform.Find("RangeVisual");
        if (rangeVisual != null)
        {
            rangeVisual.gameObject.SetActive(true); // Show the range visual
        }
    }

    private void SyncRangeVisualToCollider(GameObject tower)
    {
        float radius = tower.GetComponent<CircleCollider2D>().radius;
        Transform rangeVisual = tower.transform.Find("RangeVisual");

        if (rangeVisual != null)
        {
            float spriteRadius = rangeVisual.GetComponent<SpriteRenderer>().bounds.extents.x; // Assuming the range visual is a circle sprite

            if (spriteRadius > 0f)
            {
                float scaleFactor = radius / spriteRadius;
                rangeVisual.localScale = Vector3.one * scaleFactor; // Scale the range visual to match the collider radius
            }
            else
            {
                Debug.LogWarning("Sprite bounds not valid — check if sprite is assigned!");
            }
        }
    }

    private void PlaceTower()
    {
        isDragging = false;

        // Finalize tower
        previewTower.GetComponent<SpriteRenderer>().color = Color.white;
        previewTower.GetComponent<Collider2D>().enabled = true;

        var rangeVisual = previewTower.transform.Find("RangeVisual");
        if (rangeVisual != null)
        {
            rangeVisual.gameObject.SetActive(false); // Hide the range visual
        }

        previewTower = null;
    }
}
