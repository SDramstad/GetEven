using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_ThreatCharacter : A_TakesDamage
{
    public enum Faction
    {
        State,
        Player,
        Crime,
        Mutant
    }

    [SerializeField]
    protected Faction faction;

    public override void TakeDamage(int damage)
    {
        Debug.Log("Take damage called in A_ThreatCharacter.");
        throw new NotImplementedException();
    }

    public Faction GetFaction()
    {
        return faction;
    }
}
