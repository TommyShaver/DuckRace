using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Linq;
using System;

public class ScoreBoradManager : MonoBehaviour
{
    public TextMeshProUGUI[] scoreBoradNames;
    public TextMeshProUGUI[] scoreBoradScores;
    private int[] score = new int[8];
    private RectTransform scoreBoradTransform;

    private void Start()
    {
        CleanUp();
        scoreBoradTransform.DOAnchorPosX(-500, 0);
    }

    //Movement -----------------------------------------------------------
    public void MoveOnScreen()
    {
        scoreBoradTransform.DOAnchorPosX(100, .5f).SetEase(Ease.InOutBack);
    }

    public void MoveOffScreen()
    {
        scoreBoradTransform.DOAnchorPosX(-500, .5f).SetEase(Ease.InOutBack);
    }


    //Logic --------------------------------------------------------------
    public void GiveName(string getName)
    {
        bool nameExist = false;
        if (scoreBoradNames.All(item => !string.IsNullOrEmpty(item.text)))
        {
            Debug.Log("This game is full");
            return;
        }
        for (int i = 0; i < scoreBoradNames.Length; i++)
        {
            if (scoreBoradNames[i].text == getName)
            {
                Debug.Log("Name Already in list." + i);
                nameExist = true;
                break;
            }
        }
        if (!nameExist)
        {
            for (int i = 0; i < scoreBoradNames.Length; i++)
            {
                if (scoreBoradNames[i].text == string.Empty)
                {
                    scoreBoradNames[i].text = getName;
                    scoreBoradScores[i].text = "0";
                    break;
                }
            }
        }
        nameExist = false;
    }

    public void ScoreUpdate(string winnersName)
    {
        for (int i = 0; i < scoreBoradNames.Length; i++)
        {
            if (scoreBoradNames[i].text == winnersName)
            {
                score[i]++;
                scoreBoradScores[i].text = score[i].ToString();
                scoreBoradNames[i].text = winnersName;
                break;
            }
        }
    }


    public void CleanUp()
    {
        for (int i = 0; i < scoreBoradNames.Length; i++)
        {
            scoreBoradNames[i].text = string.Empty;
            scoreBoradScores[i].text = string.Empty;
            score[i] = 0;
        }
    }

   

}
