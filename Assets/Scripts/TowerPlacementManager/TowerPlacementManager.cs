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
        previewTower.GetComponent<Collider2D>().enabled = false;
        previewTower.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f); // semi-transparent

        // Enable range indicator 
        var rangeIndicator = previewTower.transform.Find("RangeIndicator");
        if (rangeIndicator != null)
        {
            rangeIndicator.gameObject.SetActive(true);
        }
    }

    private void PlaceTower()
    {
        isDragging = false;
        
        // Finalize tower
        previewTower.GetComponent<SpriteRenderer>().color = Color.white;
        previewTower.GetComponent<Collider2D>().enabled = true;

        var rangeIndicator = previewTower.transform.Find("RangeIndicator");
        if (rangeIndicator != null)
        {
            rangeIndicator.gameObject.SetActive(false);
        }

        previewTower = null;
    }
}
