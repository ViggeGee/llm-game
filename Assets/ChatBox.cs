using LLMUnity;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    public LLMCharacter llmCharacter;
    public TMP_InputField userInputField;
    public TMP_Text responseText;
    public TMP_Text moneyText;
    public AudioSource voice;

    [SerializeField] GameObject windows;
    [SerializeField] GameObject conversation;

    private void Start()
    {
        userInputField.onEndEdit.AddListener(OnSubmitMessage);
    }

    public void HandleDisconnect()
    {
        if (llmCharacter != null)
        {
            // End current conversation
            llmCharacter.CancelRequests();
            llmCharacter.ClearChat();

            // Recreate AI character in a separate step
            StartCoroutine(SwitchCharacter());
        }
    }

    private System.Collections.IEnumerator SwitchCharacter()
    {
        // Allow a frame for cleanup
        yield return null;

        // Generate new AI
        llmCharacter.gameObject.GetComponent<Aigenerator>().GenerateNewAi();

        // Reset UI
        windows.SetActive(true);
        responseText.text = "";
        conversation.SetActive(false);

        // Update game state
        GameManager.Instance.CallsLeft--;
    }

    void HandleReply(string reply)
    {
        string oldMsg = responseText.text;
        responseText.text = llmCharacter.AIName + " says: " + reply;
        string newMsg = responseText.text;

        if (oldMsg == newMsg)
        {
            voice.loop = false;

            // Check for payment
            MatchCollection matches = Regex.Matches(newMsg, @"pays\s*\$?(\d+)[\W$]*");
            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    int amount = int.Parse(match.Groups[1].Value);
                    float money = float.Parse(moneyText.text.Replace("$", "").Trim());
                    money += amount;
                    moneyText.text = money.ToString() + "$";
                    Debug.Log("Amount paid: " + amount + "$. Total income: " + amount + "$");
                    return;
                }
            }

            // Check for hang up
            string declinePattern = @"\*\s*(hang up|hangs up)\s*[\W]*";
            Match declineMatch = Regex.Match(reply, declinePattern, RegexOptions.IgnoreCase);
            if (declineMatch.Success)
            {
                responseText.text = "The action was declined.";
                HandleDisconnect();
                Debug.Log("Action was declined.");
            }
        }

        // Handle voice
        if (oldMsg != newMsg)
        {
            voice.pitch = Random.Range(1.3f, 1.5f);
            if (!voice.isPlaying)
                voice.Play();
        }
    }

    void OnSubmitMessage(string inputMessage)
    {
        if (string.IsNullOrEmpty(inputMessage)) return;

        responseText.text = "You said: " + inputMessage;

        // Send message directly, without appending identity reminders
        _ = llmCharacter.Chat(inputMessage, HandleReply);

        userInputField.text = "";
    }

    public void Cleanup()
    {
        if (llmCharacter != null)
        {
            llmCharacter.CancelRequests();
            llmCharacter.ClearChat();
            llmCharacter.enabled = false;
        }
    }

    private void OnApplicationQuit()
    {
        Cleanup();
    }
}