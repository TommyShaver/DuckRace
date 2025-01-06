using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using System.Collections;

public class DuckTextUserName : MonoBehaviour
{
    private TextMeshPro userNameTwitch;
    private bool iHaveAName = false;

    private void Awake()
    {
        userNameTwitch = GetComponent<TextMeshPro>();
        userNameTwitch.text = " ";
    }


    public void NameTag(string name, int layer)
    {
        if (!iHaveAName)
        {
            userNameTwitch.sortingOrder = layer + 1;
            userNameTwitch.text = name;
            Debug.Log("Player name loaded: " + name + " " + layer);
            iHaveAName = true;
        }
    }

}
