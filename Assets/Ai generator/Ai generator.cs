using LLMUnity;
using System.ComponentModel;
using System.IO;
using TMPro;
using UnityEngine;

public class Aigenerator : MonoBehaviour
{
    string namesFilePath = "Assets/Ai generator/names.txt";
    string needsFilePath = "Assets/Ai generator/needs.txt";
    string interestsFilePath = "Assets/Ai generator/interests.txt";
    string occupationsFilePath = "Assets/Ai generator/occupations.txt";

    string[] names;
    string[] needs;
    string[] interests;
    string[] occupations;

    [SerializeField] private string characterName;
    [SerializeField] private string need;
    [SerializeField] private string interest;
    [SerializeField] private string occupation;
    [SerializeField] private int age;
    public bool finishedLoading = false;

    [SerializeField] string prompt;
    [SerializeField] public TextMeshProUGUI computerText;

    [SerializeField] LLMCharacter character;

    private void Start()
    {
        // Load data from files
        names = LoadFromFile(namesFilePath);
        needs = LoadFromFile(needsFilePath);
        interests = LoadFromFile(interestsFilePath);
        occupations = LoadFromFile(occupationsFilePath);
        age = Random.Range(18, 100);

        if (character == null)
        {
            character = gameObject.AddComponent<LLMCharacter>();
            GameObject[] LLM = GameObject.FindGameObjectsWithTag("LLM");
            character.llm = LLM[0].GetComponent<LLM>();
        }

        characterName = names[Random.Range(0, names.Length)];
        need = needs[Random.Range(0, needs.Length)];
        interest = interests[Random.Range(0, interests.Length)];
        occupation = occupations[Random.Range(0, occupations.Length)];

        prompt = "this is your character: Your name is " + characterName + ". You are " + age + " years old. You like " + interest + ". You are a " + occupation + " and you need " + need + ".  These are your traits: You are pretty easily convinced and persuaded. Now the player will call you on your phone, and the next message you recieve is from the player. Write I understand. if you understand";
        computerText.text = "Name: " + characterName + "\r\nAge: " + age + "\r\nOccupation: " + occupation + "\r\nHobby: " + interest + "\r\nNeed: " + need;
    }

    public void LoadCharacter()
    {
        _ = character.Chat(prompt, WaitForCharacter);
        //character.prompt += prompt;
        character.AIName = characterName;
    }

    private string[] LoadFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            return File.ReadAllLines(filePath);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
            return new string[0];
        }
    }

    private void WaitForCharacter(string reply)
    {
        string oldReply = reply;

        Debug.Log(reply);

        string newReply = reply;

        if (oldReply == newReply && reply != "")
        {
            finishedLoading = true;
        }
    }
}
