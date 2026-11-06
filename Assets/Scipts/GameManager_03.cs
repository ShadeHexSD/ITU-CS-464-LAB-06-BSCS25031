using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_03 : MonoBehaviour
{
    public static GameManager_03 Instance { get; private set; }

    [SerializeField] int targetScore = 10;
    [SerializeField] int lives = 3;
    [SerializeField] float respawnDelay = 0.6f;
    [SerializeField] float spawnProtection = 1.5f;
    [SerializeField] Health player;
    [SerializeField] Transform respawnPoint;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] Transform mazeEnd;
    [SerializeField] float winDistance = 1.5f;

    bool reachedExit = false;

    int score;
    bool finished;
    bool respawning;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        if (player == null)
            player = FindAnyObjectByType<Health>();

        if (winPanel != null)
            winPanel.SetActive(false);

        if (losePanel != null)
            losePanel.SetActive(false);

        if (player == null || respawnPoint == null)
            Debug.LogError("Assign Player and Respawn Point on GameManager.", this);

        UpdateUI();
    }

    void Update()
    {
        if (mazeEnd == null || player == null || reachedExit)
            { return; }

        Vector3 playerPosition = player.transform.position;
        Vector3 endPosition = mazeEnd.position;


        playerPosition.y = 0;
        endPosition.y = 0;

        if(Vector3.Distance(endPosition, playerPosition) < winDistance)
        {
            reachedExit = true;
            Win();
        }
    }

    public void AddScore(int amount)
    {
        if (finished || amount <= 0)
            return;

        score += amount;
        UpdateUI();

    }

    public void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score} / {targetScore}";

        if (livesText != null && player != null)
            livesText.text = $"Lives: {lives}   Health: {Mathf.Max(player.Current, 0)} / {player.Max}";
    }

    public void PlayerDied()
    {
        if (finished || respawning)
            return;

        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            Lose();
            return;
        }

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        respawning = true;
        yield return new WaitForSeconds(respawnDelay);

        if (player == null || respawnPoint == null)
        {
            Debug.LogError("Cannot respawn: Player or Respawn Point is missing.", this);
            respawning = false;
            yield break;
        }

        Rigidbody body = player.GetComponent<Rigidbody>();

        if (body != null)
        {
            body.position = respawnPoint.position;
#if UNITY_6000_0_OR_NEWER
            body.linearVelocity = Vector3.zero;
#else
            body.velocity = Vector3.zero;
#endif
            body.angularVelocity = Vector3.zero;
        }
        else
        {
            player.transform.position = respawnPoint.position;
        }

        player.Restore(spawnProtection);
        respawning = false;
    }

    void Win()
    {
        if (finished)
            return;

        finished = true;
        if (winPanel != null)
            winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void Lose()
    {
        if (finished)
            return;

        finished = true;
        if (losePanel != null)
            losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
