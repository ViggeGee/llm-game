using LLMUnity;
using System;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Aigenerator : MonoBehaviour
{
    // File paths for character data
    string namesFilePath = "Assets/Ai generator/names.txt";
    string needsFilePath = "Assets/Ai generator/needs.txt";
    string interestsFilePath = "Assets/Ai generator/interests.txt";
    string occupationsFilePath = "Assets/Ai generator/occupations.txt";

    // Arrays to store character data
    string[] names;
    string[] needs;
    string[] interests;
    string[] occupations;

    // Character attributes
    [SerializeField] private string characterName;
    [SerializeField] private string need;
    [SerializeField] private string interest;
    [SerializeField] private string occupation;
    [SerializeField] private int age;

    // UI and AI components
    [SerializeField] public TextMeshProUGUI computerText;
    [SerializeField] private LLMCharacter character;

    string prompt;
    // State management
    public bool finishedLoading = false;


    private bool isInitialized = false;

    private void Start()
    {
        GeneareNewAi();
    }

    public void GeneareNewAi()
    {
        //weird attempt to totally memory wipe the AI, not tested
        character.enabled = false;
        character.enabled = true;

        // Load character data from files
        names = LoadFromFile(namesFilePath);
        needs = LoadFromFile(needsFilePath);
        interests = LoadFromFile(interestsFilePath);
        occupations = LoadFromFile(occupationsFilePath);

        // Generate random character attributes
        age = UnityEngine.Random.Range(18, 100);
        characterName = names[UnityEngine.Random.Range(0, names.Length)];
        need = needs[UnityEngine.Random.Range(0, needs.Length)];
        interest = interests[UnityEngine.Random.Range(0, interests.Length)];
        occupation = occupations[UnityEngine.Random.Range(0, occupations.Length)];

        // Initialize the LLMCharacter if not already assigned
        if (character == null)
        {
            Debug.Log("WHOOOOPSYYYY");
        }

        // Set the character prompt
        prompt = "this is your character: Your name is " + characterName + ". You are " + age + " years old. You like " + interest + ". You are a " + occupation + " and you need " + need + ".  These are your traits: You are pretty easily convinced and persuaded. Now the player will call you on your phone, and the next message you recieve is from the player. Write I understand. if you understand";
        computerText.text = "Name: " + characterName + "\r\nAge: " + age + "\r\nOccupation: " + occupation + "\r\nHobby: " + interest + "\r\nNeed: " + need;

        // Warm up the model before starting the conversation
        _ = WarmupModel();
    }

    private async Task WarmupModel()
    {
        Debug.Log("Warming up the model...");
        await character.Warmup();
        Debug.Log("Model is ready!");
        isInitialized = true;
        LoadCharacter();
    }

    public async void LoadCharacter()
    {
        Debug.Log("LoadCharacter called in Aigenerator instance: " + GetInstanceID());

        if (!isInitialized)
        {
            Debug.LogWarning("Model is not initialized yet. Please wait...");
            return;
        }

        Debug.Log("Loading character...");
        finishedLoading = false;

        try
        {
            await character.Chat(prompt, WaitForCharacter);
            character.AIName = characterName;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error during character loading: " + ex.Message);
            finishedLoading = false;
        }
    }

    private void WaitForCharacter(string reply)
    {
        Debug.Log("AI response: " + reply);

        if (!string.IsNullOrEmpty(reply))
        {
            finishedLoading = true;
            Debug.Log("Character loading complete.");
        }
        else
        {
            Debug.LogWarning("Empty reply received from AI.");
        }
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

    public void Cleanup()
    {
        if (character != null)
        {
            character.CancelRequests(); // Cancel any pending requests
            character.ClearChat(); // Clear the chat history
            character.enabled = false; // Disable the LLMCharacter component
        }
        Debug.Log("LLM resources cleaned up.");
    }
    private void OnApplicationQuit()
    {
        Cleanup();
    }
}