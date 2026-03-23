using System;
using System.Collections;
using UnityEngine;
using static FishDetectCatch;
using System.Threading;
using System.Threading.Tasks;

[RequireComponent(typeof(ScoreManager))]
public class SequenceManager : MonoBehaviour
{
    enum NoteType
    {
        rest,
        clickOnce,
    }


    //variables
    [Header("General Variables")]
    [SerializeField] Vector2 m_poolPosition;
    [SerializeField] private int m_noteTime;

    [Header("Click Once Variables")]
    [SerializeField] private GameObject[] m_clickOncePool;
    private bool[] m_clickOnceActive;
    private int m_clickOnceCount;
    private int m_clickOncePointerToNext;

    [Header("Sequences"), Header("Parent Sequence")]
    [SerializeField] private NoteType[] m_parentSequence;
    [SerializeField] private Vector2[] m_parentSequenceNotePositions;

    private bool m_fishCaught = false;

    //events
    public event Action<float> OnStartNewSequence;
    public event Action OnReelFish;
    public event Action OnFishEscape;


    private void Awake()
    {
        FindFirstObjectByType<ScoreManager>().OnFishCaught += FishCaughtHandler;
    }

    private void Start()
    {
        //set up pool variables
        m_clickOnceCount = m_clickOncePool.Length;
        m_clickOncePointerToNext = 0;
        m_clickOnceActive = new bool[m_clickOnceCount - 1];
        for(int i = 0;  i < m_clickOnceCount - 1; i++)
        {
            m_clickOnceActive[i] = false;
        }
    }

    /// <summary>
    /// Starts the click sequence according to the passed in string
    /// </summary>
    /// <param name="sequenceName"></param>
    public void HandleOnStartSequence(SequenceType sequenceType)
    {
        switch(sequenceType)
        {
            case (SequenceType.parent):
                PlaySequence(m_parentSequence, m_parentSequenceNotePositions);
                break;
            default:
                Debug.Log("[!]ERROR: No valid sequence name passed to manager");
                break;

        }
    }

    private async Task PlaySequence(NoteType[] sequence, Vector2[] sequenceLocations)
    {
        //count how many of each note type there are in the sequence, and spawn from pool accordingly
        GameObject[] sequenceList = MakeSequenceList(sequence);
        int sequenceLength = sequence.Length;

        // move notes to correct positions 
        for (int i = 0; i < sequenceLength; i++)
        {
            if (sequenceList[i] != null)
            {
                sequenceList[i].GetComponent<RectTransform>().position = sequenceLocations[i];
                sequenceList[i].GetComponent<SequenceClick>().NewNote();
            }
        }

        //tell score manager to start new sequence with current fish difficulty
        OnStartNewSequence?.Invoke(0.0f);   //TO DO: pass in actual fish difficulty

        //start sequence 
        for (int i = 0; i < sequenceLength; i++)
        {
            if (sequence[i] == NoteType.clickOnce)
            {
                try
                {
                   sequenceList[i].GetComponent<SequenceClick>().StartNote(m_noteTime);                  
                }
                catch (Exception e)
                {
                    Debug.Log("[!]ERROR: No click once note available for this note in sequence");
                }

            }
            //assume rest if note isn't in this list 

            //wait correct amount of time before next pass
            await Task.Delay(m_noteTime * 1000);
        }

        //if fish has been caught, reel fish if not fish escapes
        if (m_fishCaught == true)
        {
            OnReelFish?.Invoke();
        }
        else
        {
            OnFishEscape?.Invoke();
        }
        //when done reset pool active
        for (int i = 0; i < m_clickOnceCount - 1; i++)
        {
            m_clickOnceActive[i] = false;
        }

    }

    /// <summary>
    /// Make an array of game objects corresponding to a given sequence
    /// </summary>
    /// <returns></returns>
    private GameObject[] MakeSequenceList(NoteType[] sequence)
    {
        GameObject[] noteArray = new GameObject[sequence.Length];
        for (int i = 0; i < m_parentSequence.Length; i++)
        {
            switch (sequence[i])
            {
                case (NoteType.rest): //rest note, do nothing
                    noteArray[i] = null;
                    break;
                case (NoteType.clickOnce):  //get next click once note from pool, if available
                    try
                    {
                        noteArray[i] = m_clickOncePool[GetNextClickOnceIndex()];
                    }
                    catch (Exception e)
 {
                        //spawn new click once note if pool is out, but log error
                        noteArray[i] = null;
                    }
                    break;
                default:
                    Debug.Log("[!]ERROR: No valid note type in sequence");
                    break;
            }
        }
        return noteArray;
    }


    /// <summary>
    /// Get index of next available click once note in pool, and mark it as active. If no more notes available, return -1 and log error
    /// </summary>
    /// <returns></returns>
    private int GetNextClickOnceIndex()
    {
        if (m_clickOnceActive[m_clickOncePointerToNext] == true)
        {
            Debug.Log("[!]ERROR: No more click once notes available in pool");
            return -1;
        }
        else
        {
            m_clickOnceActive[m_clickOncePointerToNext] = true;
            int indexToReturn = m_clickOncePointerToNext;
            m_clickOncePointerToNext = m_clickOncePointerToNext + 1;
            if (m_clickOncePointerToNext >= m_clickOnceCount - 1)   //if reach end of pool, loop back to start
            {
                m_clickOncePointerToNext = 0;
            }
            return indexToReturn;
        }
    }

    private void FishCaughtHandler()
    {
        m_fishCaught = true;
    }
}
