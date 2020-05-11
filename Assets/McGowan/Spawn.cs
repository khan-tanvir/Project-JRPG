using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject item;
    private Transform Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SpawnDroppedItem()
    {
        Vector2 playerPos = new Vector2(Player.position.x + Random.Range(-1, 1), Player.position.y + Random.Range(-1, 1));
        Instantiate(item, playerPos, Quaternion.identity);
    }
}
