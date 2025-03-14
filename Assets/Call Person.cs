using LLMUnity;
using System.Collections;
using TMPro;
using UnityEngine;

public class CallPerson : MonoBehaviour
{
    LLMCharacter agent;
    [SerializeField] GameObject profiles;
    [SerializeField] GameObject conversation;
    [SerializeField] GameObject callingScreen;
    [SerializeField] TextMeshProUGUI postItNote;
    Aigenerator aiGenerator;

    bool isCalling = false;

    public void OnCall(GameObject character)
    {
        agent = character.GetComponent<LLMCharacter>();
        agent.enabled = true;

        aiGenerator = character.GetComponent<Aigenerator>();

        Debug.Log("Using Aigenerator instance: " + aiGenerator.GetInstanceID());

        postItNote.text = aiGenerator.computerText.text;

        conversation.GetComponent<ChatBox>().llmCharacter = this.agent;

        profiles.SetActive(false);
        conversation.SetActive(false);
        callingScreen.SetActive(true);

        StartCoroutine(WaitForAIGeneration());
    }

    private IEnumerator WaitForAIGeneration()
    {
        Debug.Log("Waiting for AI generation in Aigenerator instance: " + aiGenerator.GetInstanceID());

        while (!aiGenerator.finishedLoading) // Thread-safe read
        {
            yield return null;
        }

        Debug.Log("AI generation complete in Aigenerator instance: " + aiGenerator.GetInstanceID());

        callingScreen.SetActive(false);
        conversation.SetActive(true);
        aiGenerator.finishedLoading = false; // Thread-safe modification
    }
}