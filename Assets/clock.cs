using TMPro;
using UnityEngine;

public class clock : MonoBehaviour
{
    public TextMeshProUGUI timeText;  // Reference to TMP Text UI element

    // Update is called once per frame
    void Update()
    {
        // Get the current system time
        string currentTime = System.DateTime.Now.ToString("HH:mm");  // Format as HH:MM
        timeText.text = currentTime;  // Update the text to display the current time
    }
}
