using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameControler : MonoBehaviour
{
    public ReelManager reelManager;
    public Reel[] reel ;
    public int numOfReels;
    public int numOfRows;
    public Button spinButton;

    public CoinManager coinManager;
    public GameEvaluator evaluator;
    int[,] symbolArray;
    public int betAmount;
    int win = 0;

    public Text betLabel;
    public Text winlabel;
    public Text userBalanceLabel;

    private void Start()
    {
        coinManager = GetComponent<CoinManager>();
        coinManager.SetBetAmount(betAmount);
        betLabel.text = betAmount.ToString();
        userBalanceLabel.text = coinManager.GetPlayerBalance().ToString();
        symbolArray = new int[numOfReels, numOfRows];
        reelManager.ConstructReel();
        GetAndSetSymbolArray();
        evaluator.Evaluate(symbolArray);
    }

    void ResetGame()
    {
        reelManager.ResetReelManager();
        evaluator.ResetEvaluator();
        win = 0;
        winlabel.text = win.ToString();
    }

    public void OnSpinButtonClicked()
    {
        ResetGame();
        if (coinManager.GetPlayerBalance() - betAmount > 0)
        {
            coinManager.updateNetUserBalance(-betAmount);
            userBalanceLabel.text = coinManager.GetPlayerBalance().ToString();
            spinButton.interactable = false;
            reelManager.ConstructReel();
            GetAndSetSymbolArray();
            evaluator.Evaluate(symbolArray);
            reelManager.StartSpin();
        }
    }

    public void  SpinCompleted()
    {
        win = evaluator.GetTotalWin();
        winlabel.text = win.ToString();
        coinManager.updateNetUserBalance(win);
        userBalanceLabel.text = coinManager.GetPlayerBalance().ToString();
        if(win > 0)
        {
            reelManager.SetPayLines(evaluator.GetWinPayLine());
        }
        StartCoroutine(SetDelayForWinDisplay());
    }

    IEnumerator SetDelayForWinDisplay()
    {
        yield return new WaitForSeconds(evaluator.GetWinPayLine().Count * 2f);
        spinButton.interactable = true;
    }
    void GetAndSetSymbolArray()
    {
        
        for(int i = 0; i < numOfReels; i++)
        {
            for (int j = 0; j <numOfRows;j++)
            {
                symbolArray[i,j] = reel[i].GetPeakSymbol()[j];
            }
        }
    }
}
