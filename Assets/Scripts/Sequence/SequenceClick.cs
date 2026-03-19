using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SequenceClick : MonoBehaviour
{
    private bool m_buttonClicked;
    private float m_score;
    private GameObject m_self;

    public event Action<float> OnScoreChanger;

    private void Awake()
    {
        //bind events 
        OnScoreChanger += FindFirstObjectByType<ScoreManager>().ScoreChangeHandler;
    }

    private void Start()
    {
        m_self = this.gameObject;
        m_self.SetActive(false);

        m_buttonClicked = false;
    }


    public void PublicStartNote(float time)
    {
        m_self.SetActive(true);
        StartCoroutine(StartNote(time));
    }

    private IEnumerator StartNote(float time)
    {                                               //make note indicator
        m_buttonClicked = false;
        m_score = 0.0f;
        float runtime = time;
        while (time > 0)
        {
            if (m_buttonClicked)
            {
                Debug.Log("Click button");
                m_score = 1.0f - time / runtime;
                OnScoreChanger?.Invoke(m_score);
                break;
            }
            time-= Time.deltaTime;
            yield return new WaitForEndOfFrame();   //make sure that everything runs in time
        }
        if (time > 0)
        {
            yield return new WaitForSecondsRealtime(time);
        }
        else
        {
            OnScoreChanger?.Invoke(m_score);
        }
        m_self.SetActive(false);
    }

    public void HandleOnButtonClicked()
    {
        m_buttonClicked = true;
    }
}
