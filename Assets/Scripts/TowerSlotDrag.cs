using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSlotDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public GameObject towerPrefab;

    private GameObject previewTower;


    public void OnBeginDrag(PointerEventData eventData)
    {
        previewTower = Instantiate(towerPrefab);
        previewTower.GetComponent<Collider2D>().enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (previewTower != null)
        {
            previewTower.transform.position = mousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(previewTower); // For now, just destroy
    }
}
