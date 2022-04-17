using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    private float respawnTimer = 0.0f;

    [SerializeField]
    private GameObject demon;

    [SerializeField]
    private float respawnCountdown = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!demon.activeSelf)
        {
            // respawn behaviour
            if (respawnTimer >= 0.0f)
            {
                respawnTimer -= Time.deltaTime;
            }
            else if (respawnTimer <= 0.0f)
            {
                gameObject.SetActive(true);
            }
        }
        else if (demon.activeSelf)
        {
            respawnTimer = respawnCountdown;
        }

    }
}
