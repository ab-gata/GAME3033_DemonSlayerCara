using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private DoorBehaviour doorObject;
    private KeyBehaviour keyObject;
    private UpgradeBehaviour upgradeObject;
    private TomeBehaviour tomeObject;

    private int keyCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        bool hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, 3f, hitLayers);

        if (hit)
        {
            if (hitInfo.collider.CompareTag("Door"))
            {
                interactPromptText.text = "Press [E]";
                doorObject = hitInfo.collider.GetComponent<DoorBehaviour>();
            }
            else if (hitInfo.collider.CompareTag("Key"))
            {
                interactPromptText.text = "Press [E]";
                keyObject = hitInfo.collider.GetComponent<KeyBehaviour>();
            }
            else if (hitInfo.collider.CompareTag("Upgrade"))
            {
                interactPromptText.text = "Press [E]";
                upgradeObject = hitInfo.collider.GetComponent<UpgradeBehaviour>();
                interactPromptText.text = "";
            }
            else if (hitInfo.collider.CompareTag("Tome"))
            {
                interactPromptText.text = "Press [E]";
                tomeObject = hitInfo.collider.GetComponent<TomeBehaviour>();
            }
        }
        else
        {
            interactText.text = "";
            interactPromptText.text = "";

            doorObject = null;
            keyObject = null;
            upgradeObject = null;
            tomeObject = null;
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

    private void UpdateGeneralHUD()
    {
        
    }

    // Called from weapon
    public void UpdateBulletDamageHUD(int damage)
    {
        bulletDamageText.text = "Bullet Damage : " + damage;
    }

    // Called from player
    public void UpdateStatsHUD(int health, int defence)
    {
        healthText.text = "Health : " + health + " / 100";
        defenseText.text = "Defense : " + defence;
    }

    public void Interact()
    {
        if (doorObject)
        {

        }
        if (keyObject)
        {

        }
        if (upgradeObject)
        {
            switch (upgradeObject.upgrade)
            {
                case UpgradeType.DAMAGE:
                    player.AddDefense(upgradeObject.value);
                    break;
                case UpgradeType.DEFENSE:
                    player.AddDefense(upgradeObject.value);
                    break;
                case UpgradeType.HEALTH:
                    break;
            }
        }
        if (tomeObject)
        {

        }
    }

    public void Lose()
    {
        // Load Scene
        SceneManager.LoadScene("GameOver");
    }
}
