using TMPro;
using UnityEngine;

public class FishUI : MonoBehaviour
{
    public TextMeshProUGUI fishText;

    void Update()
    {
        fishText.text = "Fish: " + GameManager.Instance.fishCount;
    }
}
