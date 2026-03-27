using UnityEngine;
using TMPro;

public class DisplayFishNumber : MonoBehaviour
{
    private int m_score;
    private TextMeshProUGUI m_text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        FindFirstObjectByType<ScoreManager>().OnFishCaught += ChangeScore;
        m_text = GetComponent<TextMeshProUGUI>();
    }

    private void ChangeScore()
    {
        m_score += 1;
        m_text.text = "Fish Caught: " + m_score.ToString();
    }
}
