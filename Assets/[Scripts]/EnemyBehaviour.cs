using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float health = 10;

    [SerializeField]
    private float damage = 10;

    private GameController game;
    private PlayerBehaviour player;

    private void Start()
    {
        game = FindObjectOfType<GameController>();
        player = FindObjectOfType<PlayerBehaviour>();
    }

    public void ShotAt(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private float GetDamage()
    {
        if (game.phase2)
        {
            return damage * 2f;
        }

        return damage;
    }
}
