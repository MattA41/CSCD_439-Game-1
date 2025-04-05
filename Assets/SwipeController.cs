using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [SerializeField] int maxPage;
    int currentpage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;
    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;

    private void Awake()
    {
        currentpage = 1;
        targetPos = levelPagesRect.localPosition;
    }

    public void Next()
    {
        if(currentpage < maxPage){
            currentpage++;
            targetPos+=pageStep;
            MovePage();
        }

    }


    public void Previous()
    {
        if (currentpage > 1)
        {
            currentpage--;
            targetPos-=pageStep;
            MovePage();
        }

    }
    
    void MovePage()
    {
        levelPagesRect.LeanMoveLocal(targetPos,tweenTime).setEase(tweenType);
    }
}
