using LLMUnity;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UI; // Include Unity UI for user input
using UnityEngine.Windows.Speech;

public class ChatBox : MonoBehaviour
{
    public LLMCharacter llmCharacter;
    public TMP_InputField userInputField; // UI Input Field for the user to type their message
    public TMP_Text responseText; // UI Text to display the response
    public TMP_Text moneyText; // UI Text to display the response
    public AudioSource voice;

    [SerializeField] GameObject windows;
    [SerializeField] GameObject conversation;
    private void Start()
    {
        // Optional: Add a listener to detect when the user presses enter or submits the input
        userInputField.onEndEdit.AddListener(OnSubmitMessage);
    }

    public void HandleDisconnect()
    {
        if (llmCharacter != null)
        {
            llmCharacter.CancelRequests(); // Cancel any pending requests
            llmCharacter.ClearChat(); // Clear the chat history
            llmCharacter.enabled = false; // Disable the LLMCharacter component
            Debug.Log("Chat history saved, pending requests canceled, chat history cleared, and character disabled.");
            llmCharacter.gameObject.GetComponent<Aigenerator>().GeneareNewAi();
        }
        // Reset the UI
        windows.SetActive(true); // Show the profiles screen
        responseText.text = "";
        conversation.SetActive(false); // Hide the conversation screen
        Debug.Log("UI reset to default state.");

        //Update gamemanager
        GameManager.Instance.CallsLeft--;

    }

    // Handle the reply from the LLM
    void HandleReply(string reply)
    {
        string oldMsg = responseText.text;
        // Display the LLM's response in the UI Text
        responseText.text = llmCharacter.AIName + " says: " + reply;

        string newMsg = responseText.text;

        if (oldMsg == newMsg)
        {
            voice.loop = false;

            MatchCollection matches = Regex.Matches(newMsg, @"pays\s*\$?(\d+)[\W$]*");


            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    int amount = int.Parse(match.Groups[1].Value); // Extract the number and convert to integer
                    float money = float.Parse(moneyText.text.Replace("$", "").Trim());

                    money += amount;

                    moneyText.text = money.ToString() + "$";

                    Debug.Log("Amount paid: " + amount + "$. Total income: " + amount + "$");

                    HandleDisconnect();

                    return;
                }
            }

            // Regular Expression to check for the word "Decline" or "Declines" starting with *
            string declinePattern = @"\*\s*(hang up|hangs up)\s*[\W]*";
            Match declineMatch = Regex.Match(reply, declinePattern, RegexOptions.IgnoreCase);

            if (declineMatch.Success)
            {
                // Handle the decline logic here
                responseText.text = "The action was declined.";

                HandleDisconnect();

                Debug.Log("Action was declined.");
            }
        }

        if (oldMsg != newMsg)
        {
            voice.pitch = Random.Range(1.3f, 1.5f);
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

    public void Cleanup()
    {
        if (llmCharacter != null)
        {
            llmCharacter.CancelRequests(); // Cancel any pending requests
            llmCharacter.ClearChat(); // Clear the chat history
            llmCharacter.enabled = false; // Disable the LLMCharacter component
        }
        Debug.Log("LLM resources cleaned up.");
    }

    private void OnApplicationQuit()
    {

        Cleanup();

    }
}
