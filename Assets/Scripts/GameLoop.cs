using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
{
    public GameObject[] heartIcons;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    private int currentHealth = 3;
    private int gold = 0;
    private int score = 0;
    private float timeLeft = 500f;

    void Update()
    {
        // Countdown timer
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if (timerText != null) timerText.text = "Time: " + Mathf.CeilToInt(timeLeft).ToString();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Melee attack with E or Left Click
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    Destroy(hit.gameObject);
                    score += 100;
                    if (scoreText != null) scoreText.text = "Score: " + score.ToString("D6");
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            currentHealth--;
            for (int i = 0; i < heartIcons.Length; i++)
            {
                heartIcons[i].SetActive(i < currentHealth);
            }

            if (currentHealth <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pit"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (other.CompareTag("Gold"))
        {
            Destroy(other.gameObject);
            gold++;
            score += 50;
            if (goldText != null) goldText.text = "Gold: " + gold.ToString("D3");
            if (scoreText != null) scoreText.text = "Score: " + score.ToString("D6");
        }
        else if (other.CompareTag("Portal"))
        {
            Debug.Log("Level Complete!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}