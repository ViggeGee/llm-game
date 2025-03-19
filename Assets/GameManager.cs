using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager _instance;

    // Public accessor for the singleton instance
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    [SerializeField] TextMeshProUGUI moneyText;

    [SerializeField] TextMeshProUGUI daysText;
    [SerializeField] TextMeshProUGUI rentText;
    [SerializeField] TextMeshProUGUI callsLeftText;

    [SerializeField] int day;
    [SerializeField] int rent;
    // Private backing field for callsLeft
    [SerializeField] int callsLeft;

    // Property for callsLeft
    public int CallsLeft
    {
        get { return callsLeft; }
        set
        {
            callsLeft = value;
            callsLeftText.text = "Calls Left: " + callsLeft; // Update UI

            // Check if callsLeft is 0 and run the method
            if (callsLeft == 0)
            {
                NewDay();
            }
        }
    }

    public void Pay(int amount)
    {
        float money = float.Parse(moneyText.text.Replace("$", "").Trim());

        money -= amount;

        moneyText.text = money.ToString() + "$";
    }

    public void GetPayed()
    {

    }

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        
    }

    private void NewDay()
    {
        callsLeft = 5;
        day++;
        Pay(rent);

        daysText.text = "DAY: " + day;
        rentText.text = "RENT: " + rent + "$";
        callsLeftText.text = "CALLS LEFT: " + callsLeft;

    }

    private void Start()
    {
        daysText.text = "DAY: " + day;
        rentText.text = "RENT: " + rent+"$";
        callsLeftText.text = "CALLS LEFT: " + callsLeft;
    }
}