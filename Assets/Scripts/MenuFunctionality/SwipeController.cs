using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SwipeController : MonoBehaviour
{
    [SerializeField] int maxPage;
    public int currentpage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;
    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;

    private void Awake()
    {
        currentpage = 0;
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
        if (currentpage > 0)
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
  

    //Add maps to the Window Scroller

    public void SelectPage()
    {
        int[] maps = new int[] { 0, 1, 2 }; // Add as many maps as needed

        if (maps[currentpage] == 0)
        {
            SceneManager.LoadScene("Scenes/ActualMap1");
        }
        if (maps[currentpage] == 1)
        {
            SceneManager.LoadScene("Scenes/ActualMap2");
        }
        if (maps[currentpage] == 2)
        {
            SceneManager.LoadScene("Scenes/ActualMap3");
        }
        else
        {
            Debug.Log("Not a valid map");
        }
    }
}
