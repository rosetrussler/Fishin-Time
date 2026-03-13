using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SequenceClick : MonoBehaviour
{
    private bool m_buttonClicked;

    private void Start()
    {
        m_buttonClicked = false;
    }

    IEnumerator StartNote(float time)
    {
        m_buttonClicked = false;
        float score = 0.0f;
        float runtime = time;
        while (time > 0)
        {
            if (m_buttonClicked)
            {
                score = 1.0f - time / runtime;
                break;
            }
            time-= Time.deltaTime;
        }
        if (time > 0)
        {
            yield return new WaitForSecondsRealtime(time);
        }
        yield return score;
    }

    public void HandleOnButtonClicked()
    {
        m_buttonClicked = true;
    }
}
