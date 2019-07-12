using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_TakesDamage : MonoBehaviour, ITakeDamage {

    [SerializeField]
    internal ParticleSystem painGFX;

    //public ParticleSystem painGFX;

    public abstract void TakeDamage(int damage);
    

}
