using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SequenceClick : MonoBehaviour
{
    private bool m_buttonClicked;
    private float m_score;
    private GameObject m_self;

    private void Start()
    {
        m_self = this.gameObject;
        m_self.SetActive(false);

        m_buttonClicked = false;
    }


    public float PublicStartNote(float time)
    {
        m_self.SetActive(true);
        StartCoroutine(StartNote(time));
        return m_score;
    }

    private IEnumerator StartNote(float time)
    {
        m_buttonClicked = false;
        float m_score = 0.0f;
        float runtime = time;
        while (time > 0)
        {
            if (m_buttonClicked)
            {
                m_score = 1.0f - time / runtime;
                break;
            }
            time-= Time.deltaTime;
        }
        if (time > 0)
        {
            yield return new WaitForSecondsRealtime(time);
        }
        yield return null;
    }

    public void HandleOnButtonClicked()
    {
        m_buttonClicked = true;
    }
}
