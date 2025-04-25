using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerSlotDrag : MonoBehaviour, IPointerDownHandler
{

    public GameObject towerPrefab;
    public Text costText;
    public int towerCost;
    public TowerPlacementManager placementManager;

    private void Start()
    {
        if (costText != null)
        {
            costText.text = towerCost.ToString();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        placementManager.SetTowerToPlace(towerPrefab, towerCost);
    }
}
