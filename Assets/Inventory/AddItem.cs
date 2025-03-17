using TMPro;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AddItem : MonoBehaviour
{
    [SerializeField] float singlePrice;
    [SerializeField] float bulkPrice;
    [SerializeField] TextMeshProUGUI minProfit;
    [SerializeField] TextMeshProUGUI maxProfit;
    [SerializeField] TextMeshProUGUI money;
    [SerializeField] TextMeshProUGUI itemName;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void BuySingle()
    {
        float amount = float.Parse(money.text.Replace("$", "").Trim());
        amount -= singlePrice;

        if (amount > 0)
        {
            money.text = amount.ToString() + "$";

            float minProfitf = float.Parse(minProfit.text);
            float maxProfitf = float.Parse(maxProfit.text);

            Inventory.Instance.AddItem(itemName.text, 1, minProfitf, maxProfitf);
        }
        else
        {
            Debug.Log("U aint got no money!");
        }

    }
    public void BuyBulk()
    {
        float amount = float.Parse(money.text.Replace("$", "").Trim());
        amount -= bulkPrice;

        if (amount > 0)
        {
            money.text = amount.ToString() + "$";

            float minProfitf = float.Parse(minProfit.text);
            float maxProfitf = float.Parse(maxProfit.text);

            Inventory.Instance.AddItem(itemName.text, 10, minProfitf, maxProfitf);
        }
        else
        {
            Debug.Log("U aint got no money!");
        }
    }

}
