using System;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{

    //variables
    [SerializeField] private GameObject[] m_clickOncePool;
    private bool[] m_clickOnceActive;
    private int m_clickOnceCount;
    [SerializeField] Vector2 m_poolPosition;


    private void Start()
    {
        //set up pool variables
        m_clickOnceCount = m_clickOncePool.Length;
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
    public void HandleOnStartSequence(string sequenceName)
    {
        switch(sequenceName)
        {
            case ("parent"):
                ParentSequence();
                break;
            default:
                Debug.Log("[!]ERROR: No valid sequence name passed to manager");
                break;

        }
    }

    private void ParentSequence()
    {
        //start parent sequence
            //put all notes into position on screen, hide some and diable all but one, make different colours for how far in the sequence it is 
        //queue of notes, pass back score if score reaches low or high threshold end queue and pass or fail 
            //coroutines which trigger the next one? 
            //put in queue and executed by for loop with break condition 
        //at end of queue finish sequence 
    }

}
