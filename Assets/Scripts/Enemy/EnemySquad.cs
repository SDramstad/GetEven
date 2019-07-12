using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquad : MonoBehaviour {

    /**
     * ENEMY SQUAD AI PACKAGE
     * 
     * This package may not be implemented.
     * 
     * This class aims to set up rules for alerting and managing the AI of a group of enemies.
     *  
     *  STATUSES
     *  Unaware : Soldiers just stand there.
     *  Aware : Soldiers are aware that a target has been spotted. Possibly send out someone to search.
     *  Alerted: Soldiers are all active and gunning for the target.
     * 
     * */

    public enum EnemyStatus
    {
        Unaware = 0,
        Aware = 1,
        Alerted = 2
    }

    public EnemyStatus status;

    void Start()
    {
        status = EnemyStatus.Unaware;
    }

    //Make sure to only put gameobjects with Enemy Ai in here. Technically, you could put anything that 
    // takes damage but that would result in bad behavior.
    public A_TakesDamage[] squadMembers; 

	public void MakeAware()
    {

    }

    public void MakeAlert()
    {
        status = EnemyStatus.Alerted;
        foreach (var enemy in squadMembers)
        {
            if (enemy.GetComponent<Soldier>() != null)
            {
                enemy.GetComponent<Soldier>().Alert();
            } 
            //else if (enemy.GetComponent<MolemanEnemy>() != null)
            //{
            //    enemy.GetComponent<MolemanEnemy>().Alert();
            //}
        }
    }
}
