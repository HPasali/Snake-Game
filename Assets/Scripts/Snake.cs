using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Snake : MonoBehaviour
{
    public int initialLength;
    private Vector3 rotation;
    private bool isMovementHorizontal;
    public GameObject snakeSegmentsPrefab, GameOverScreen;
    // The snake is shown with merged squares and these squares are holded in List.
    public List<Transform> segments = new List<Transform>();
    public TextMeshProUGUI scoreText;
    private int score;


    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        scoreText.text = "Score: " + score;

        if (isMovementHorizontal && Input.GetKeyDown(KeyCode.W))
        {
            rotation = Vector3.up;
            isMovementHorizontal = false;
        }
        else if(isMovementHorizontal && Input.GetKeyDown(KeyCode.S))
        {
            rotation = Vector3.down;
            isMovementHorizontal = false;
        }
        else if(!isMovementHorizontal && Input.GetKeyDown(KeyCode.A))
        {
            rotation = Vector3.left;
            isMovementHorizontal = true;
        }
        else if(!isMovementHorizontal && Input.GetKeyDown(KeyCode.D))
        {
            rotation = Vector3.right;
            isMovementHorizontal = true;
        }
    }

    private void FixedUpdate()
    {
        // The snake segments are move according to another segment which is in front of it.
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        transform.position = new Vector3(Mathf.Round(transform.position.x + rotation.x), Mathf.Round(transform.position.y + rotation.y), 0.0f);
    }

    public void Grow()
    {
        Transform snakeSegment = Instantiate(snakeSegmentsPrefab.transform);
        // It is take false due to prevent any error.
        snakeSegment.gameObject.SetActive(false);
        // The position is assigned and it added to list to move with other segments.
        snakeSegment.position = segments[segments.Count - 1].position;
        segments.Add(snakeSegment);
        snakeSegment.gameObject.SetActive(true);
        score += 5;
    }

    public void ResetState()
    {
        GameOverScreen.SetActive(false);
        Time.timeScale = 1.0f;
        score = 0;
        for (int i = segments.Count - 1; i > 0; i--) 
        {
            Destroy(segments[i].gameObject);
        }
        segments.Clear();
        segments.Add(transform);
        transform.position = Vector3.zero;
        // Initial rotation is right when game is started.
        rotation = Vector3.right;
        isMovementHorizontal = true;

        // This is the first version of snake length.
        for (int i = 0; i < initialLength; i++)
        {
            segments.Add(Instantiate(snakeSegmentsPrefab.transform));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            Grow();
        }
        // Obstacles are walls and the other snake segments.
        else if (collision.CompareTag("Obstacle"))
        {
            Time.timeScale = 0.0f;
            GameOverScreen.SetActive(true);
            
        }
    }
}
