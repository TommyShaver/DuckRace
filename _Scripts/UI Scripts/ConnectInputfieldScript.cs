using TMPro;
using UnityEngine;

public class ConnectInputfieldScript : MonoBehaviour
{
    [SerializeField] TMP_InputField currentInputfeild;
    [SerializeField] TextMeshProUGUI currentPlacerText;

    public string GetText()
    {
        string updateName = currentInputfeild.text;
        currentPlacerText.text = currentInputfeild.text;
        return updateName;
    }
}
