using System;
using System.Collections;
using UnityEngine;
using static FishDetectCatch;
using System.Threading;
using System.Threading.Tasks;

[RequireComponent(typeof(ScoreManager))]
public class SequenceManager : MonoBehaviour
{
    /// <summary>
    /// The different tyoes of notes that can appear in a sequence
    /// </summary>
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
    private float m_fishDifficulty;

    [Header("Sequences"), Header("Parent Sequence")]
    [SerializeField] private NoteType[] m_parentSequence;
    [SerializeField] private Vector2[] m_parentSequenceNotePositions;
    /// <summary>
    /// A value between 0 - 1 that the player's timing score is multiplied by
    /// </summary>
    [SerializeField] private float m_parentDifficulty;
    [Header("Sea Bass Sequence")]
    [SerializeField] private NoteType[] m_seaBassSequence;
    [SerializeField] private Vector2[] m_seaBassSequenceNotePositions;
    /// <summary>
    /// A value between 0 - 1 that the player's timing score is multiplied by
    /// </summary>
    [SerializeField] private float m_seaBassDifficulty;
    [Header("Angel Fish Sequence")]
    [SerializeField] private NoteType[] m_angelFishSequence;
    [SerializeField] private Vector2[] m_angelFishSequencePositions;
    /// <summary>
    /// A value between 0 - 1 that the player's timing score is multiplied by
    /// </summary>
    [SerializeField] private float m_angelFishDifficulty;
    [Header("Sun Fish Sequence")]
    [SerializeField] private NoteType[] m_sunFishSequence;
    [SerializeField] private Vector2[] m_sunFishSequencePositions;
    /// <summary>
    /// A value between 0 - 1 that the player's timing score is multiplied by
    /// </summary>
    [SerializeField] private float m_sunFishDifficulty;
    [Header("Cod Sequence")]
    [SerializeField] private NoteType[] m_codSequence;
    [SerializeField] private Vector2[] m_codSequencePositions;
    /// <summary>
    /// A value between 0 - 1 that the player's timing score is multiplied by
    /// </summary>
    [SerializeField] private float m_codDifficulty;
    [Header("Eel Sequence")]
    [SerializeField] private NoteType[] m_eelSequence;
    [SerializeField] private Vector2[] m_eelSequencePositions;
    /// <summary>
    /// A value between 0 - 1 that the player's timing score is multiplied by
    /// </summary>
    [SerializeField] private float m_eelDifficulty;
    [Header("Angler Fish Sequence")]
    [SerializeField] private NoteType[] m_anglerFishSequence;
    [SerializeField] private Vector2[] m_anglerFishSequencePositions;
    /// <summary>
    /// A value between 0 - 1 that the player's timing score is multiplied by
    /// </summary>
    [SerializeField] private float m_anglerFishDifficulty;
    [Header("Haddock Sequence")]
    [SerializeField] private NoteType[] m_haddockSequence;
    [SerializeField] private Vector2[] m_haddockSequencePositions;
    /// <summary>
    /// A value between 0 - 1 that the player's timing score is multiplied by
    /// </summary>
    [SerializeField] private float m_haddockDifficulty;
    [Header("Trout Sequence")]
    [SerializeField] private NoteType[] m_troutSequence;
    [SerializeField] private Vector2[] m_troutSequencePositions;
    /// <summary>
    /// A value between 0 - 1 that the player's timing score is multiplied by
    /// </summary>
    [SerializeField] private float m_troutDifficulty;

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
    public async void HandleOnStartSequence(SequenceType sequenceType, FishDetectCatch fishRef)
    {
        //play sequence given by sequence type 
        switch (sequenceType)    
        {
            case (SequenceType.parent):
                m_fishDifficulty = m_parentDifficulty;
                await PlaySequence(m_parentSequence, m_parentSequenceNotePositions, fishRef);
                break;
            case (SequenceType.seaBass):
                m_fishDifficulty = m_seaBassDifficulty;
                await PlaySequence(m_seaBassSequence, m_seaBassSequenceNotePositions, fishRef);
                break;
            case (SequenceType.angelFish):
                m_fishDifficulty = m_angelFishDifficulty;
                await PlaySequence(m_angelFishSequence, m_angelFishSequencePositions, fishRef);
                break;
            case (SequenceType.sunFish):
                m_fishDifficulty = m_sunFishDifficulty;
                await PlaySequence(m_sunFishSequence, m_sunFishSequencePositions, fishRef);
                break;
            case (SequenceType.cod):
                m_fishDifficulty = m_codDifficulty;
                await PlaySequence(m_codSequence, m_codSequencePositions, fishRef);
                break;
            case (SequenceType.eel):
                m_fishDifficulty = m_eelDifficulty;
                await PlaySequence(m_eelSequence, m_eelSequencePositions, fishRef);
                break;
            case (SequenceType.anglerFish):
                m_fishDifficulty = m_anglerFishDifficulty;
                await PlaySequence(m_anglerFishSequence, m_anglerFishSequencePositions, fishRef);
                break;
            case (SequenceType.haddock):
                m_fishDifficulty = m_haddockDifficulty;
                await PlaySequence(m_haddockSequence, m_haddockSequencePositions, fishRef);
                break;
            case (SequenceType.trout):
                m_fishDifficulty = m_troutDifficulty;
                await PlaySequence(m_troutSequence, m_troutSequencePositions, fishRef);
                break;
            default:
                Debug.Log("[!]ERROR: No valid sequence name passed to manager");
                break;

        }
    }

    private async Task PlaySequence(NoteType[] sequence, Vector2[] sequenceLocations, FishDetectCatch fishRef)
    {
        int length = sequence.Length;
        //count how many of each note type there are in the sequence, and spawn from pool accordingly
        GameObject[] sequenceList = MakeSequenceList(sequence, length);
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

        OnStartNewSequence?.Invoke(m_fishDifficulty);   

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
            Debug.Log("SEQUENCE KNOWS FISH WAS CAUGHT!!");
            OnReelFish += fishRef.ReelFishHandler;
            OnReelFish?.Invoke();
            OnReelFish -= fishRef.ReelFishHandler;
        }
        else
        {
            OnFishEscape += fishRef.FishEscapeHandler;
            OnFishEscape?.Invoke();
            OnFishEscape -= fishRef.FishEscapeHandler;
        }
        //when done reset pool actives
        for (int i = 0; i < m_clickOnceCount - 1; i++)
        {
            m_clickOnceActive[i] = false;
        }


    }

    /// <summary>
    /// Make an array of game objects corresponding to a given sequence
    /// </summary>
    /// <returns></returns>
    private GameObject[] MakeSequenceList(NoteType[] sequence, int length)
    {
        GameObject[] noteArray = new GameObject[sequence.Length];
        for (int i = 0; i < length; i++)
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
