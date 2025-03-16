using UnityEngine;

public class Windows : MonoBehaviour
{
    [SerializeField] GameObject profiles;
    [SerializeField] GameObject windows;
    [SerializeField] GameObject shop;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject PcCamera;
 

    public void OpenProfiles()
    {
        windows.SetActive(false);
        profiles.SetActive(true);   
    }
    public void OpenShop()
    {
        windows.SetActive(false);
        shop.SetActive(true);   
    }
    public void CloseProfiles()
    {
        windows.SetActive(true);
        profiles?.SetActive(false);
    }

    public void CloseShop()
    {
        windows.SetActive(true);
        shop.SetActive(false);
    }

    public void TurnOffPc()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PcCamera.SetActive(false);
        playerCamera.SetActive(true);
    }
}
