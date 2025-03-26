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
    string inventoryFilePath = "Assets/Inventory/Inventory.txt";

    // Arrays to store character data
    string[] names;
    string[] needs;
    string[] interests;
    string[] occupations;
    string[] items;
    public string inventory;

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
        GenerateNewAi();
    }

    public void GenerateNewAi()
    {
        // Reset character state before creating new character
        if (character != null)
        {
            character.CancelRequests();
            character.ClearChat();
        }

        // Load character data
        names = LoadFromFile(namesFilePath);
        needs = LoadFromFile(needsFilePath);
        interests = LoadFromFile(interestsFilePath);
        occupations = LoadFromFile(occupationsFilePath);
        items = LoadFromFile(inventoryFilePath);

        // Generate random character attributes
        age = UnityEngine.Random.Range(18, 100);
        characterName = names[UnityEngine.Random.Range(0, names.Length)];
        need = needs[UnityEngine.Random.Range(0, needs.Length)];
        interest = interests[UnityEngine.Random.Range(0, interests.Length)];
        occupation = occupations[UnityEngine.Random.Range(0, occupations.Length)];
        inventory = string.Concat(items);

        // Update UI
        computerText.text = "Name: " + characterName + "\r\nAge: " + age + "\r\nOccupation: " + occupation + "\r\nHobby: " + interest;

        // Set character name
        character.AIName = characterName;

        // Create character prompt - identity only, not conversation reset
        prompt = "You are " + characterName + ", age " + age + ". " +
                 "You like " + interest + " and you work as a " + occupation + ". " +
                 "You need " + need + " and you are easily persuaded. " +
                 "The player is calling you on your phone. This is a new phone call. " +
                 "Please respond as " + characterName + ".";

        // Reset initialization flag
        isInitialized = false;

        // Warm up model
        _ = WarmupModel();
    }

    private async Task WarmupModel()
    {
        Debug.Log("Warming up model for: " + characterName);
        await character.Warmup();
        isInitialized = true;
        LoadCharacter();
    }

    public async void LoadCharacter()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Model not initialized yet");
            return;
        }

        finishedLoading = false;

        try
        {
            // Set the character prompt
            await character.Chat(prompt, WaitForCharacter);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error loading character: " + ex.Message);
            finishedLoading = false;
        }
    }

    private void WaitForCharacter(string reply)
    {
        Debug.Log("AI response: " + reply);

        if (!string.IsNullOrEmpty(reply))
        {
            finishedLoading = true;
            Debug.Log("Character loaded: " + characterName);
        }
        else
        {
            Debug.LogWarning("Empty reply from AI");
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
            character.CancelRequests();
            character.ClearChat();
            character.enabled = false;
        }
    }

    private void OnApplicationQuit()
    {
        Cleanup();
    }
}