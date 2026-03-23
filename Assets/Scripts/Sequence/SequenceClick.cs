using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SequenceClick : MonoBehaviour
{
    private bool m_buttonClicked;
    private float m_score;
    private GameObject m_self;

    public event Action<float> OnScoreChanger;
    private event Action<float, float> OnNoteUpdate;
    private event Action OnNoteChange;

    private void Awake()
    {
        //bind events 
        OnScoreChanger += FindFirstObjectByType<ScoreManager>().ScoreChangeHandler;
        ClickHighlightScale m_clickHighlightScale = this.GetComponentInChildren<ClickHighlightScale>();
        OnNoteUpdate += m_clickHighlightScale.NoteUpdateHandler;
        OnNoteChange += m_clickHighlightScale.NewNoteHandler;

    }

    private void Start()
    {
        m_self = this.gameObject;
        m_self.SetActive(false);
        m_self.GetComponent<Image>().enabled = true;

        m_buttonClicked = false;
    }


    /// <summary>
    /// Public function which starts the note's play coroutine
    /// </summary>
    /// <param name="time"></param>
    public void StartNote(float time)
    {
        m_buttonClicked = false;
        m_self.SetActive(true);
        m_self.GetComponent<Image>().enabled = true;
        StartCoroutine(PlayNote(time));
    }


    /// <summary>
    /// This function manages the note play sequence and scores the player on the accuracy of their timing, this value is returned from the coroutine via an event
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator PlayNote(float time)
    {                                               //make note indicator
        m_buttonClicked = false;
        m_score = 0.0f;
        float runtime = time;
        while (time > 0)
        {
            if (m_buttonClicked)
            {
                Debug.Log("Click button");
                m_score = (runtime - time) * (1/runtime);       //score = fraction of time passed
                OnScoreChanger?.Invoke(m_score);
                m_self.GetComponent<Image>().enabled = false;
                break;
            }
            time-= Time.deltaTime;
            OnNoteUpdate?.Invoke(time, runtime);
            yield return new WaitForEndOfFrame();   //make sure that everything runs in time
        }
        m_self.SetActive(false);
    }

    /// <summary>
    /// This tells the script when the button has been clicked
    /// </summary>
    public void HandleOnButtonClicked()
    {
        m_buttonClicked = true;
    }

    public void NewNote()
    {

    }
}
