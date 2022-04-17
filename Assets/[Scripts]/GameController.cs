using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField, Header("Object Interaction")]
    private PlayerBehaviour player;
    [SerializeField] WeaponComponent weapon;

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

    private GameObject doorObject;
    private GameObject keyObject;
    private UpgradeBehaviour upgradeObject;
    private TomeBehaviour tomeObject;

    private int keyCount = 0;
    private string objective = "Find the Tome of Evil";


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        UpdateGeneralHUD();
    }

    // Update is called once per frame
    void Update()
    {
        bool hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, 3f, hitLayers);

        if (hit)
        {
            if (hitInfo.collider.CompareTag("Door"))
            {
                if (keyCount > 0) { interactText.text = "DOOR : use key to unlock"; }
                else { interactText.text = "DOOR : find a key first!"; }
                interactPromptText.text = "Press [E]";
                doorObject = hitInfo.collider.gameObject;
            }
            else if (hitInfo.collider.CompareTag("Key"))
            {
                interactText.text = "KEY : use this to unlock doors";
                interactPromptText.text = "Press [E]";
                keyObject = hitInfo.collider.gameObject;
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
        keysText.text = keyCount.ToString();
        objectiveText.text = objective;
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
            if (keyCount > 0)
            {
                doorObject.SetActive(false);
                keyCount--;
            }
        }
        if (keyObject)
        {
            keyCount++;
            keyObject.SetActive(false);
        }
        if (upgradeObject)
        {
            switch (upgradeObject.upgrade)
            {
                case UpgradeType.DAMAGE:
                    weapon.AddBulletDamage(upgradeObject.value);
                    break;
                case UpgradeType.DEFENSE:
                    player.AddDefense(upgradeObject.value);
                    break;
                case UpgradeType.HEALTH:
                    player.AddHealth(upgradeObject.value);
                    break;
            }
            upgradeObject.gameObject.SetActive(false);
        }
        if (tomeObject)
        {

        }

        UpdateGeneralHUD();
    }

    public void Lose()
    {
        // Load Scene
        SceneManager.LoadScene("GameOver");
    }
}
