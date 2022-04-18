using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField, Header("Object Interaction")]
    private PlayerBehaviour player;
    [SerializeField] WeaponHolder weapon;

    [SerializeField, Header("Object Interaction")]
    private LayerMask hitLayers;

    [SerializeField, Header("User Interface")] private GameObject pauseUI;
    [SerializeField] private Text objectiveText;
    [SerializeField] private Text timeTitleText;
    [SerializeField] private Text distanceText;
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
    private GameObject tomeObject;
    private GameObject escapeObject;

    [SerializeField]
    private GameObject escapeLocation;
    [SerializeField]
    private GameObject tomeLocation;

    private int keyCount = 0;
    private string objective = "Find the Tome of Evil";

    public bool phase2 = false;
    private float timer = 100;

    private SoundManager sound;
    private MusicManager music;


    // Start is called before the first frame update
    void Start()
    {
        sound = FindObjectOfType<SoundManager>();
        music = FindObjectOfType<MusicManager>();
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
                upgradeObject = hitInfo.collider.GetComponent<UpgradeBehaviour>();
                interactText.text = "UPGRADE : " + UpgradeText();
                interactPromptText.text = "Press [E]";
            }
            else if (hitInfo.collider.CompareTag("Tome"))
            {
                interactText.text = "TOME OF EVIL : collect objective";
                interactPromptText.text = "Press [E]";
                tomeObject = hitInfo.collider.gameObject;
            }
            else if (hitInfo.collider.CompareTag("Escape"))
            {
                interactText.text = "EXIT : Escape here with the Tome of Evil!";
                interactPromptText.text = "Press [E]";
                escapeObject = hitInfo.collider.gameObject;
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
            escapeObject = null;
        }

        // Timer for phase 2
        if (phase2)
        {
            timer -= Time.deltaTime;
            timeText.text = ((int)timer).ToString();

            if (timer < 0)
            {
                Lose();
            }
        }

        // Mouse lock depending on if menu is shown
        if (pauseUI)
        {
            if (pauseUI.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0.0f;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1.0f;
            }
        }

        if (phase2)
        {
            float distance = Vector3.Distance(escapeLocation.transform.position, player.transform.position);
            distanceText.text = ((int)distance).ToString() + " m";
        }
        else
        {
            float distance = Vector3.Distance(tomeLocation.transform.position, player.transform.position);
            distanceText.text = ((int)distance).ToString() + " m";
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

    private string UpgradeText()
    {
        string words = "";

        if (upgradeObject)
        {
            switch (upgradeObject.upgrade)
            {
                case UpgradeType.DAMAGE:
                    words = "Increase Bullet Damage by " + upgradeObject.value;
                    break;
                case UpgradeType.DEFENSE:
                    words = "Increase Defense by " + upgradeObject.value;
                    break;
                case UpgradeType.HEALTH:
                    words = "Increase Health by " + upgradeObject.value;
                    break;
            }
        }
        return words;
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
            phase2 = true;
            music.PlayMusic(MusicManager.TrackID.PHASE2);
            tomeObject.SetActive(false);
            timeTitleText.text = "TIME LEFT";
            objective = "Find the Exit!!!";
        }
        if (escapeObject)
        {
            Win();
        }

        UpdateGeneralHUD();
    }

    public void Lose()
    {
        // Load Scene
        sound.PlaySound(SoundManager.TrackID.LOSE);
        SceneManager.LoadScene("GameOver");
    }

    public void Win()
    {
        if (phase2)
        {
            // Load Scene
            sound.PlaySound(SoundManager.TrackID.WIN);
            SceneManager.LoadScene("GameOverWin");
        }
    }
}
