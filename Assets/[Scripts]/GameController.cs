using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField, Header("Object Interaction")]
    private PlayerBehaviour player;

    [SerializeField, Header("Object Interaction")]
    private LayerMask hitLayers;

    [SerializeField, Header("User Interface")] private GameObject pauseUI;
    [SerializeField] private Text objectiveText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text keysText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text bulletDamageText;
    [SerializeField] private Text defenseText;
    [SerializeField] private Text interactText;
    [SerializeField] private Text interactPromptText;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        bool hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, 1.5f, hitLayers);

        if (hit)
        {
            if (hitInfo.collider.CompareTag("Door"))
            {
                Debug.Log("looking at a door");
                interactPromptText.text = "Press [E]";
            }
            else if (hitInfo.collider.CompareTag("Tome"))
            {
                Debug.Log("looking at a tome");
                //weaponComponent.TryShoot(hitInfo.collider.GetComponent<EnemyBehaviour>());
                interactPromptText.text = "Press [E]";
            }
        }
        else
        {
            interactText.text = "";
            interactPromptText.text = "";
        }

        // Mouse lock depending on if menu is shown
        if (pauseUI)
        {
            if (pauseUI.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
