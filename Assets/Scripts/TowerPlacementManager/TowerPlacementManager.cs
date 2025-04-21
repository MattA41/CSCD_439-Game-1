using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TowerPlacementManager : MonoBehaviour
{
    public GameObject towerPrefab;


    [Header("Tilemap Settings")]
    public Tilemap roadTilemap; // Assign red path tilemap here

    private GameObject previewTower;
    private SpriteRenderer rangeRenderer;
    private Transform rangeVisual;


    private bool isDragging = false;

    
    public PlayerManager playerManager;

    public int cost = 50;

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPos = roadTilemap.WorldToCell(mouseWorldPos); // still used for tile check

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

            // Place when mouse is released
            if (Input.GetMouseButtonUp(0))
            {
                PlaceTower(cost);
                bool isValid = IsValidPlacement(gridPos);
                rangeRenderer.color = isValid
                    ? new Color(0f, 1f, 0f, 0.3f) // green
                    : new Color(1f, 0f, 0f, 0.3f); // red

                if (Input.GetMouseButtonUp(0))
                {
                    if (isValid) PlaceTower();
                    else CancelPlacement();
                }

            }
        }
    }

    private void StartDragTower()
    {
        if (cost <= playerManager.coins)
        {
            isDragging = true;
            previewTower = Instantiate(towerPrefab);
            previewTower.GetComponent<Collider2D>().enabled = false;
            previewTower.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f); // semi-transparent

        }
        else
        {
            Debug.Log("NOT ENOUGH MONEY!");
        }
       
        isDragging = true;
        previewTower = Instantiate(towerPrefab);
        previewTower.GetComponent<Collider2D>().enabled = false;

        rangeVisual = previewTower.transform.Find("RangeVisual");
        rangeRenderer = rangeVisual.GetComponent<SpriteRenderer>();
        rangeVisual.gameObject.SetActive(true);
    }

    private bool IsValidPlacement(Vector3Int gridPos)
    {
        return roadTilemap.GetTile(gridPos) == null;
    }

    private void PlaceTower(int cost)
    {
        isDragging = false;
        previewTower.GetComponent<Collider2D>().enabled = true;
        rangeVisual.gameObject.SetActive(false);
        previewTower = null;
    }

    private void CancelPlacement()
    {
        isDragging = false;
        Destroy(previewTower);
        previewTower = null;

        playerManager.coins = playerManager.coins - cost; 

        
        
    }
}
