using LLMUnity;
using TMPro;
using UnityEngine;

public class CallPerson : MonoBehaviour
{
    LLMCharacter agent;
    [SerializeField] GameObject profiles;
    [SerializeField] GameObject conversation;
    [SerializeField] TextMeshProUGUI postItNote;
    Aigenerator aiGenerator;
    public void OnCall(GameObject character)
    {
     
        

        agent = character.GetComponent<LLMCharacter>();
        agent.enabled = true;

        aiGenerator = character.GetComponent<Aigenerator>();
        postItNote.text = aiGenerator.computerText.text;
        aiGenerator.LoadCharacter();


        conversation.GetComponent<ChatBox>().llmCharacter = this.agent;

        profiles.SetActive(false);
        conversation.SetActive(true);
    
    }
}
