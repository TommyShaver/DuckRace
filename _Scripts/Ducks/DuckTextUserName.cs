using UnityEngine;
using TMPro;


public class DuckTextUserName : MonoBehaviour
{
    private TextMeshPro userNameTwitch;
    private int currentLayer = 51;
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
            userNameTwitch.sortingOrder = layer + currentLayer;
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

    public void DuckChangedLayer(int i)
    {
        //So when the ducks change levels the sort order travels with it.
        userNameTwitch.sortingOrder += i;
    }
}
