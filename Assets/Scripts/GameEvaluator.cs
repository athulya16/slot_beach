using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvaluator : MonoBehaviour
{
    int[] payLines = new int[] { 0, 1, 2, 3, 4};
    int[] winValues = new int[] { 5, 4, 3, 2, 1, 1, 1, 1 };
    int totalWin = 0;
    List<List<int>> winLine = new List<List<int>>();

    public void ResetEvaluator()
    {
        totalWin = 0;
        winLine.Clear();
    }

    public void Evaluate(int[,] symbolArray)
    {
        for(int i = 0; i < payLines.Length; i++)
        {
            int symbolId;
            switch(payLines[i])
            {
                case 0:
                    symbolId = symbolArray[0, 0];
                    if((symbolId == symbolArray[1,0]) && (symbolId == symbolArray[2,0]))
                    {
                        totalWin += winValues[symbolId];
                        List<int> subList = new List<int>() { 0, 1, 2 };
                        winLine.Add(subList);
                    }
                    //[0,1,2]
                    break;
                case 1:
                    symbolId = symbolArray[0, 1];
                    if ((symbolId == symbolArray[1,1]) && (symbolId == symbolArray[2, 1]))
                    {
                        totalWin += winValues[symbolId];
                        List<int> subList = new List<int>() { 3, 4, 5 };
                        winLine.Add(subList);
                    }
                    //[3,4,5]
                    break;
                case 2:
                    symbolId = symbolArray[0, 2];
                    if ((symbolId == symbolArray[1, 2]) && (symbolId == symbolArray[2, 2]))
                    {
                        totalWin += winValues[symbolId];
                        List<int> subList = new List<int>() { 6, 7, 8 };
                        winLine.Add(subList);
                    }
                    //[6,7,8]
                    break;
                case 3:
                    symbolId = symbolArray[0, 0];
                    if ((symbolId == symbolArray[1, 1]) && (symbolId == symbolArray[2, 0]))
                    {
                        totalWin += winValues[symbolId];
                        List<int> subList = new List<int>() { 0, 4, 2 };
                        winLine.Add(subList);
                    }
                    //[0,4,2]
                    break;
                case 4:
                    symbolId = symbolArray[0, 2];
                    if ((symbolId == symbolArray[1, 1]) && (symbolId == symbolArray[2, 2]))
                    {
                        totalWin += winValues[symbolId];
                        List<int> subList = new List<int>() { 6, 4, 8 };
                        winLine.Add(subList);
                    }
                    //[6,4,8]
                    break;
            }
        }
    }

    public int GetTotalWin()
    {
        return totalWin;
    }

    public List<List<int>> GetWinPayLine()
    {
        return winLine;
    }
}
