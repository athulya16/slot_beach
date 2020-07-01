using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Reel : MonoBehaviour
{
    public GameObject[] symbolArray;
    public GameObject panel;
    public int[] reel;

    public float iconX;
    public float iconY;
    public float iconHeight;

    int peakSymbolIndex = -1;
    bool spin;
    bool reverseSpin = false;

    float reverseDuration = 0.08f;
    int speed = 900;
    float duration = 3f;

    public GameObject[] staticSymbolArray = new GameObject[3];
    GameObject[] reelSymbolsInSpin = new GameObject[4];
    int[] staticSymbolIndexArray = new int[4];

    public ReelManager reelManagerRef;
    public GameObject staticSymbolPanel;
    int symbolsInSpin;
    public int numOfRows = 3;
    bool isFirstSpin = true;
    int reelId;

    float animateSpeed = 0.5f;
    int animationLoop = 0;
    Vector2 finalPos;
    float reverseDistance;
  
    IEnumerator Spin()
    {
        duration = duration + (reelId*0.2f);
        spin = true;
        yield return new WaitForSeconds(duration + reverseDuration);
        reverseSpin = true;

        float dur = reverseDistance / (speed);
        yield return new WaitForSeconds(dur);
        reverseSpin = false;
        spin = false;
        isFirstSpin = false;
        OnReelStopped();
    }

    public void ConstructReel(int id)
    {
        reverseDistance = speed * reverseDuration;
        reelId = id;
        for (int i = 0; i < reelSymbolsInSpin.Length; i++)
        {
            Destroy(reelSymbolsInSpin[i]);
        }
        Array.Clear(reelSymbolsInSpin, 0, reelSymbolsInSpin.Length);
        RectTransform panelRect = this.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, iconHeight);
        staticSymbolPanel.transform.position = new Vector2(transform.position.x, panel.transform.position.y);
        SetReel(reelId);
        GenerateRandomSymbolIndex();
        int start = numOfRows;
        for (int i = 0, j = numOfRows; i < reel.Length && start < (symbolsInSpin + reelId); i++, j++)
        {
            CreateSymbol(j, reel[i]);
            start++;
            if (i + 1 == reel.Length)
            {
                i = -1;
            }
            if (start == symbolsInSpin+reelId)
            {
                int count = symbolsInSpin ;
                transform.position = new Vector2(transform.position.x, transform.position.y + (count * iconHeight));
            }
        }
        if(isFirstSpin)
        {
            SetStaticSymbols();
        }
    }

    public void StartSpin(int id)
    {
        reelId = id;
        float extraDistance = 0 ;
        transform.position = new Vector2(transform.position.x, transform.position.y + extraDistance);
        RectTransform panelRect = this.GetComponent<RectTransform>();
        finalPos = new Vector2(transform.position.x, transform.position.y - panelRect.sizeDelta.y);
        duration = 3f;
        StartCoroutine(Spin());
    }

    private void SetReel(int reelId)
    {
        symbolsInSpin = (int)((speed * 3f) / iconHeight);
        reelSymbolsInSpin = new GameObject[symbolsInSpin + reelId+1]; 
        RectTransform panelRect = this.GetComponent<RectTransform>();
        transform.position = new Vector2(transform.position.x, panel.transform.position.y + iconHeight - ((symbolsInSpin -reelId-1) * iconHeight) / 2f);
        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, (symbolsInSpin + reelId) * iconHeight);
    }

    void CreateSymbol(int rowIndex, int symbolId)
    {
        rowIndex  = rowIndex + 1;
        reelSymbolsInSpin[rowIndex] = GetSymbolObj(symbolId);
        reelSymbolsInSpin[rowIndex].SetActive(true);
        reelSymbolsInSpin[rowIndex].transform.SetParent(this.transform, false);
        float posX = panel.transform.position.x;
        float posY = panel.transform.position.y - (rowIndex - (reelId +2)) * iconHeight;
        reelSymbolsInSpin[rowIndex].transform.position = new Vector2(posX, posY);
    }

    private void SetStaticSymbols()
    {
        for (int i = 0; i < staticSymbolArray.Length; i++)
        {
            Destroy(staticSymbolArray[i]);
        }
        Array.Clear(staticSymbolArray, 0, staticSymbolArray.Length);
        if (isFirstSpin)
        {
            int index = UnityEngine.Random.Range(0, reel.Length - 1);
            int start = 0;
            
            for (int i = index, j = 0; i < reel.Length && start < numOfRows; i++, j++)
            {
                staticSymbolArray[j] = GetSymbolObj(reel[i]);
                staticSymbolArray[j].SetActive(true);
                staticSymbolArray[j].transform.SetParent(staticSymbolPanel.transform, false);
                float xPos = staticSymbolPanel.transform.position.x;
                float yPos = panel.transform.position.y + iconY - start * iconHeight;
                staticSymbolArray[j].transform.position = new Vector2(xPos, yPos);
                start++;
                if (i + 1 == reel.Length)
                {
                    i = -1;
                }
            }
            isFirstSpin = false;
        }
        else
        {
            int start = 0;
           for( int i = (peakSymbolIndex+1), j = 0; i < reel.Length && start < numOfRows; i++, j++)
            {
                staticSymbolArray[start] = GetSymbolObj(reel[i]);
                staticSymbolArray[start].SetActive(true);
                staticSymbolArray[start].transform.SetParent(staticSymbolPanel.transform, false);
                float xPos = staticSymbolPanel.transform.position.x ;
                float yPos = staticSymbolPanel.transform.position.y+ iconHeight - start * iconHeight;
                staticSymbolArray[start].transform.position = new Vector2(xPos, yPos);
                start++;
                if (i + 1 == reel.Length)
                {
                    i = -1;
                }
            }
        }
    }

    void SetPeakSymbol()
    {
        CreateSymbol(-1, reel[peakSymbolIndex]);
    }

    private GameObject GetSymbolObj(int index)
    {
        GameObject obj = null;
        for (int i = 0; i < symbolArray.Length; i++)
        {
            string name = symbolArray[i].name;
            int id = Int32.Parse(name.Substring(name.Length - 1));
            if (index == id)
            {
                obj = Instantiate(symbolArray[id], symbolArray[id].transform.position,Quaternion.identity) as GameObject;
            }
        }
        return obj;
    }

    private void Update()
    {
        if (spin)
        {
            if(reverseSpin)
            {
                float step1 = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + reverseDistance), step1);
            }
            else
            {
                float step = speed * Time.deltaTime;
                RectTransform panelRect = this.GetComponent<RectTransform>();
                transform.position = Vector3.MoveTowards(transform.position, new Vector2(transform.position.x, finalPos.y - reverseDistance), step);
                staticSymbolPanel.transform.position = Vector3.MoveTowards(staticSymbolPanel.transform.position, new Vector2(transform.position.x, staticSymbolPanel.transform.position.y- panelRect.sizeDelta.y), step);
            }
        }
    }
   
    private void GenerateRandomSymbolIndex()
    {
         peakSymbolIndex = UnityEngine.Random.Range(0, reel.Length - 1);
         SetPeakSymbol();
         int start = 0;
         for (int i = (peakSymbolIndex + 1), j = 0; i < reel.Length && start < numOfRows; i++, j++)
         {
            staticSymbolIndexArray[start] = reel[i];
            CreateSymbol(j, reel[i]);
            start++;
            if (i + 1 == reel.Length) 
            {
                i = -1;
            }
        }
    }

    private void OnReelStopped()
    {
        if(!isFirstSpin)
        {
            SetStaticSymbols();
            staticSymbolPanel.transform.position = new Vector2(transform.position.x,panel.transform.position.y);
        }
        for (int i = 0; i < reelSymbolsInSpin.Length; i++)
        {
            Destroy(reelSymbolsInSpin[i]);
        }
        Array.Clear(reelSymbolsInSpin, 0, reelSymbolsInSpin.Length);

        reelManagerRef.GetComponent<ReelManager>().SpinCompleted();
    }

    public int[] GetPeakSymbol()
    {
       return staticSymbolIndexArray;
    }

    public void Animate(int row)
    {
        staticSymbolArray[row].SetActive(false);
        StartCoroutine(PlayAnimation(row));
    }

    IEnumerator PlayAnimation(int row)
    {
        yield return new WaitForSeconds(animateSpeed);
        staticSymbolArray[row].SetActive(true);
        yield return new WaitForSeconds(animateSpeed);
        animationLoop++;
        if(animationLoop < 2)
        {
            Animate(row);
        }
        else
        {
            animationLoop = 0;
        }
    }
}
