using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using System.Collections;

public class DuckTextUserName : MonoBehaviour
{
    private TextMeshPro userNameTwitch;

    private void Awake()
    {
        userNameTwitch = GetComponent<TextMeshPro>();
        userNameTwitch.text = " ";
    }


    public void NameTag(string name)
    {
        userNameTwitch.text = name;
        Debug.Log("Player name loaded: " + name);
    }

}
