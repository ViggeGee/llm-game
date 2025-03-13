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
        postItNote.text = aiGenerator.computerText.text;
        aiGenerator.LoadCharacter();


        conversation.GetComponent<ChatBox>().llmCharacter = this.agent;

        profiles.SetActive(false);
        conversation.SetActive(false);
        callingScreen.SetActive(true);

        StartCoroutine(WaitForAIGeneration());
    
    }

    private IEnumerator WaitForAIGeneration()
    {
        while (!aiGenerator.finishedLoading)
        {
            yield return null;
        }

        if (aiGenerator.finishedLoading)
        {
            callingScreen.SetActive(false);
            conversation.SetActive(true);
        }
    }
}
