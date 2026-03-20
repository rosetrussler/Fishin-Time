using UnityEngine;
using UnityEngine.UI;

public class ClickHighlightScale : MonoBehaviour
{
    [SerializeField] float m_startSize;
    [SerializeField] float m_endSize;
    private RectTransform m_transform;

    private void Awake()
    {
        m_transform = this.GetComponent<RectTransform>();

    }

    private void Start()
    {
        
    }

    /// <summary>
    /// Updates the note highlights size according to the values(times) passed in
    /// </summary>
    /// <param name="timeUntilFinish"></param>
    /// <param name="noteRuntime"></param>
    public void NoteUpdateHandler(float timeUntilFinish, float noteRuntime)
    {
        //set the scale of the highlight relative to times passed in 
        float scale = m_startSize - (((noteRuntime - timeUntilFinish) / noteRuntime) * (m_startSize - m_endSize));
        m_transform.localScale =  new Vector3(scale, scale, 1);
    }

    /// <summary>
    /// Reset the note for a new sequence
    /// </summary>
    public void NewNoteHandler()
    {
        m_transform.localScale = new Vector3(m_startSize, m_startSize, 1);
    }
}
