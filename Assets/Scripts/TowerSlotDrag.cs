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
    private Image background;         // The image behind the icon
    private Color originalColor;       // The original color of the icon
    private RectTransform iconTransform; // Optional: reference to icon if you want only that to shake
    public TowerPlacementManager placementManager;

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    private void Start()
    {
        if (costText != null)
        {
            costText.text = towerCost.ToString();
        }

        background = GetComponent<Image>();
        iconTransform = GetComponent<RectTransform>();
        originalColor = background.color;

        if (iconTransform != null) originalPosition = iconTransform.localPosition;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (placementManager.playerManager.coins >= towerCost)
        {
            placementManager.SetTowerToPlace(towerPrefab, towerCost);
        }
        else
        {
            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
                ResetVisuals();
            }

            shakeCoroutine = StartCoroutine(ShowErrorFeedback());
            placementManager.StartCoroutine(placementManager.ShowInsufficientFunds());
        }
    }

    IEnumerator ShowErrorFeedback()
    {
        // Flash red background
        background.color = Color.red;

        // Shake icon
        float shakeTime = 0.2f;
        float strength = 5f;
        float timer = 0f;

        Vector2 originalAnchoredPos = iconTransform.anchoredPosition;
        while (timer < shakeTime)
        {
            timer += Time.deltaTime;
            float offsetX = Mathf.Sin(timer * 50) * strength;
            iconTransform.anchoredPosition = originalAnchoredPos + new Vector2(offsetX, 0f);
            yield return null;
        }

        iconTransform.anchoredPosition = originalAnchoredPos;
        background.color = originalColor;
        shakeCoroutine = null;
    }

    private void ResetVisuals()
    {
        if (iconTransform != null) iconTransform.anchoredPosition = originalPosition;
        if (background != null) background.color = originalColor;
    }
}
