using System.Collections;
using UnityEngine;
using TMPro;

public class CatchAnimation : MonoBehaviour
{
    private TextMeshProUGUI m_text;

    private void Awake()
    {
        FindFirstObjectByType<ScoreManager>().OnFishCaught += HandleCatch;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_text = GetComponent<TextMeshProUGUI>();
        m_text.gameObject.SetActive(false);
    }

    private void HandleCatch()
    {
        m_text.gameObject.SetActive(true);
        StartCoroutine(DisplayForXSeconds());
    }

    private IEnumerator DisplayForXSeconds()
    {
        Debug.Log("DISPLAY TEXT!!");
        yield return new WaitForSecondsRealtime(2.0f);
        m_text.gameObject.SetActive(false);
        yield return null;
    }

}
