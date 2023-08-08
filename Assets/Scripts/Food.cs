using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // The spawn area is restricted with a empty game object which has a box collider component.
    public BoxCollider2D gridArea;
    private bool spawnFlag;

    private void Start()
    {
        RandomizePosition();
    }
    public void RandomizePosition()
    {
        spawnFlag = false;
        // The food's position is changing with this algorithm. If the spawnPosition == any segment's position this method will work again on the 24th line.
        Vector3 spawnPosition = new Vector3();
        Bounds bounds = gridArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        spawnPosition = new Vector3(Mathf.Round(x), Mathf.Round(y), 0);

        for (int i = GameObject.Find("Snake").GetComponent<Snake>().segments.Count - 1; i > 0; i--)
        {
            if (spawnPosition == GameObject.Find("Snake").GetComponent<Snake>().segments[i].position)
            {
                spawnFlag = true;
                break;
            }
        }
        if (!spawnFlag)
        {
            transform.position = spawnPosition;
        }
        else
        {
            RandomizePosition();
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RandomizePosition();
        }
    }
}
