using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TowerPlacementManager : MonoBehaviour
{
    [Header("Tilemap Settings")]
    public Tilemap roadTilemap;

    [Header("UI Settings")]
    public Image insuffFunds;

    [Header("Player Settings")]
    public PlayerManager playerManager;

    private GameObject towerPrefab;
    private int cost;

    private GameObject previewTower;
    private SpriteRenderer rangeRenderer;
    private Transform rangeVisual;

    private bool isDragging = false;
    private Coroutine fundsCoroutine;
    private bool isAnimatingFunds = false;




    // Update is called once per frame
    void Update()
    {
        if (!isDragging) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPos = roadTilemap.WorldToCell(mouseWorldPos);

        if (previewTower != null)
        {
            previewTower.transform.position = mouseWorldPos;

            bool isValid = IsValidPlacement(gridPos);
            rangeRenderer.color = isValid ? new Color(0f, 1f, 0f, 0.3f) : new Color(1f, 0f, 0f, 0.3f);

            if (Input.GetMouseButtonUp(0))
            {
                if (isValid && playerManager.coins >= cost)
                {
                    PlaceTower();
                    playerManager.coins -= cost;
                }
                else
                {
                    CancelPlacement();
                }
            }
        }

    }

     public void SetTowerToPlace(GameObject prefab, int towerCost)
    {
        towerPrefab = prefab;
        cost = towerCost;
        StartDragTower();
    }

    private void StartDragTower()
    {
        if (cost <= playerManager.coins)
        {
            isDragging = true;
            previewTower = Instantiate(towerPrefab);
            previewTower.GetComponent<Collider2D>().enabled = false;
            previewTower.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

            rangeVisual = previewTower.transform.Find("RangeVisual");
            rangeRenderer = rangeVisual.GetComponent<SpriteRenderer>();
            rangeVisual.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("NOT ENOUGH MONEY!");

            if (!isAnimatingFunds)
            {
                fundsCoroutine = StartCoroutine(ShowInsufficientFunds());
            }
        }


    }
    private IEnumerator ShowInsufficientFunds()
    {
        isAnimatingFunds = true;

        RectTransform rect = insuffFunds.GetComponent<RectTransform>();
        Vector2 startPos = rect.anchoredPosition;
        Vector2 upPos = new Vector2(startPos.x, 464);
        Vector2 downPos = startPos;

        float duration = 0.3f;

        // Animate up
        float t = 0f;
        while (t < 1f)
        {
            rect.anchoredPosition = Vector2.Lerp(startPos, upPos, t);
            t += Time.unscaledDeltaTime / duration;
            yield return null;
        }
        rect.anchoredPosition = upPos;

        yield return new WaitForSecondsRealtime(1.5f); // also unaffected by timeScale

        // Animate down
        t = 0f;
        while (t < 1f)
        {
            rect.anchoredPosition = Vector2.Lerp(upPos, downPos, t);
            t += Time.unscaledDeltaTime / duration;
            yield return null;
        }
        rect.anchoredPosition = downPos;

        isAnimatingFunds = false;
    }



    private bool IsValidPlacement(Vector3Int gridPos)
    {
        return roadTilemap.GetTile(gridPos) == null;
    }

    private void PlaceTower()
    {
        isDragging = false;
        previewTower.GetComponent<Collider2D>().enabled = true;
        previewTower.GetComponent<SpriteRenderer>().color = Color.white;
        rangeVisual.gameObject.SetActive(false);
        previewTower = null;
    }

    private void CancelPlacement()
    {
        isDragging = false;
        Destroy(previewTower);
        previewTower = null;
    }
}
