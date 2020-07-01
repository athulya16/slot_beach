using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReelManager : MonoBehaviour
{
    public Reel[] reel;
    public int numOfReels;
    public int numOfRows;

    int reelsStopped = 0;
    List<int> currentLine;
    int numOfPaylines = 0;
    int currentLineIndex = 0;
    List<List<int>> payLineList;

    public GameControler gameControlerRef;

    public void ResetReelManager()
    {
        reelsStopped = 0;
        numOfPaylines = 0;
        currentLineIndex = 0;
    }

    public void ConstructReel()
    {
        for(int i = 0; i < numOfReels; i++)
        {
            reel[i].ConstructReel(i);
        }
    }

    public void StartSpin()
    {
        for (int i = 0; i < numOfReels; i++)
        {
            reel[i].StartSpin(i);
        }
    }

    public void SpinCompleted()
    {
        reelsStopped++;
        if(reelsStopped >= numOfReels)
        {
            gameControlerRef.GetComponent<GameControler>().SpinCompleted();
        }
    }

    public void SetPayLines(List<List<int>> paylines)
    {
        payLineList = paylines;
        numOfPaylines = paylines.Count;
        AnimateLines();
    }
    public void AnimateLines()
    {
        if(currentLineIndex < numOfPaylines)
        {
            currentLine = payLineList[currentLineIndex];
            StartCoroutine(AnimateLine());
        }
    }

     IEnumerator AnimateLine()
    {
        for (int j = 0; j < currentLine.Count; j++)
        {
            int reelId = currentLine[j] % numOfReels;
            int row = (int)Mathf.Floor((currentLine[j] / numOfReels));
            reel[reelId].Animate(row);
        }
        yield return new WaitForSeconds(2f);
        currentLineIndex++;
        AnimateLines();
    }
}
