using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] GameObject PcCamera;
    [SerializeField] GameObject mainCamera;

    bool interactComputer = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactComputer)
        {
            PcCamera.SetActive(true);
            mainCamera.SetActive(false);
            interactText.text = "";
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Computer"))
        {
            interactText.text = "Press 'E' to start PC";
            interactComputer = true;
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        interactText.text = "";
        interactComputer = false;
    }
}
