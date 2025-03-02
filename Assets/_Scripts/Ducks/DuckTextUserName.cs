using UnityEngine;
using TMPro;


public class DuckTextUserName : MonoBehaviour
{
    private TextMeshPro userNameTwitch;
    private int lockedLayer;
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
            lockedLayer = layer;
            userNameTwitch.sortingOrder = layer;
            userNameTwitch.text = name;
            Debug.Log("Player name loaded: " + name + " " + layer);
            iHaveAName = true;
        }
    }

    //Send info to DuckManager for tracking --------------------------------
    public string GetName()
    {
        return userNameTwitch.text;
    }
}
