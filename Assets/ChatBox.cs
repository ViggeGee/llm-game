using LLMUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Include Unity UI for user input
using UnityEngine.Windows.Speech;

public class ChatBox : MonoBehaviour
{
    public LLMCharacter llmCharacter;
    public InputField userInputField; // UI Input Field for the user to type their message
    public TMP_Text responseText; // UI Text to display the response
    public AudioSource voice;

    private void Start()
    {
        // Optional: Add a listener to detect when the user presses enter or submits the input
        userInputField.onEndEdit.AddListener(OnSubmitMessage);
    }

    // Handle the reply from the LLM
    void HandleReply(string reply)
    {
        string oldMsg = responseText.text;
        // Display the LLM's response in the UI Text
        responseText.text = "LLM says: " + reply;
        Debug.Log("LLM says: " + reply);

        string newMsg = responseText.text;

        if (oldMsg == newMsg)
        {
            voice.loop = false;
        }
        if (oldMsg != newMsg)
        {
            voice.pitch = Random.Range(0.6f, 0.8f);
            if (!voice.isPlaying)
                voice.Play();
        }

    }

    // Called when the user submits a message (presses Enter or submits the input)
    void OnSubmitMessage(string inputMessage)
    {
        if (string.IsNullOrEmpty(inputMessage)) return; // Ignore empty messages

        // Optionally: You can display the user’s message in the UI
        responseText.text = "You said: " + inputMessage;

        // Send the user input to the LLM
        _ = llmCharacter.Chat(inputMessage, HandleReply);

        // Optionally: Clear the input field after sending the message
        userInputField.text = "";
    }
}
