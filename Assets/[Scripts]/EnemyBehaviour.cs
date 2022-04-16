using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private float health = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShotAt(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log("DEATH");
        }
    }
}
