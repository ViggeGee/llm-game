using LLMUnity;
using UnityEngine;
using System.Threading.Tasks;

public class InventoryRAG : MonoBehaviour
{
    [SerializeField] private LLM llm; // Reference to your LLM
    [SerializeField] private Aigenerator aiGenerator; // Reference to your Aigenerator

    private RAG rag;
    private bool isInitialized = false;

    private async void Start()
    {
        // Make sure LLM is initialized first
        await LLM.WaitUntilModelSetup();

        // Initialize RAG system
        rag = gameObject.AddComponent<RAG>();
        rag.Init(SearchMethods.DBSearch, ChunkingMethods.SentenceSplitter, llm);

        // Load inventory data into RAG
        await UpdateInventoryInRAG();

        isInitialized = true;
        Debug.Log("Inventory RAG system initialized");
    }

    // Call this whenever inventory changes
    public async Task UpdateInventoryInRAG()
    {
        if (rag == null) return;

        // Clear existing inventory data
        rag.Clear();

        // Add current inventory to RAG
        string inventoryData = aiGenerator.inventory;
        await rag.Add(inventoryData, "inventory");

        Debug.Log("Updated inventory in RAG: " + inventoryData);
    }

    // Function to enhance a prompt with inventory data
    public async Task<string> EnhancePromptWithInventory(string userMessage)
    {
        if (!isInitialized) return userMessage;

        // Search for relevant inventory information
        (string[] results, float[] distances) = await rag.Search(userMessage, 1, "inventory");

        if (results.Length == 0) return userMessage;

        // Create an enhanced prompt that includes inventory information
        string enhancedPrompt = "Before responding, please note the following player inventory information:\n";
        enhancedPrompt += results[0] + "\n\n";
        enhancedPrompt += "User message: " + userMessage;

        return enhancedPrompt;
    }
}