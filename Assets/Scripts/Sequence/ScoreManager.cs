using System;
using UnityEngine;

[RequireComponent(typeof(SequenceManager))]
public class ScoreManager : MonoBehaviour
{
    private float m_playerScore;
    private float m_currentFishDifficulty;  //has to be in range 0 <= x < 1
    private bool m_fishCaught;

    public event Action OnFishCaught;

    //hold the score of the player 
    //manage random fish score
    //call event when the player catches or looses fish

    private void Awake()
    {
        //subscribe to events
        SequenceManager sequenceManager = GetComponent<SequenceManager>();
        sequenceManager.OnStartNewSequence += StartNewSequenceHandler;
    }

    private void StartNewSequenceHandler(float fishDifficulty)
    {
        m_fishCaught = false;
        m_playerScore = 0;
        m_currentFishDifficulty = fishDifficulty;
    }

    public void ScoreChangeHandler(float playerScore)
    {
        m_playerScore += playerScore;
        Debug.Log("Player score: " + m_playerScore);
        if (m_playerScore >= 1.0f && m_fishCaught == false)
        {
            Debug.Log("FISH CAUGHT");
            m_fishCaught = true;
            //call fish caught event to reset bobber state
            OnFishCaught?.Invoke();
        }
    }
}
