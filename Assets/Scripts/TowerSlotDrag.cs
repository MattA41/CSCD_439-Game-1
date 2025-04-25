using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSlotDrag : MonoBehaviour, IPointerDownHandler
{

    public GameObject towerPrefab;
    public int towerCost;
    public TowerPlacementManager placementManager;
	public void OnPointerDown(PointerEventData eventData)
	{
		placementManager.SetTowerToPlace(towerPrefab, towerCost);
	}
}
