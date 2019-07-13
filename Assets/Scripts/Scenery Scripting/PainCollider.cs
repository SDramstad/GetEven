using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainCollider : MonoBehaviour
{
    [SerializeField]
    private AudioClip damageSFX;
    [SerializeField]
    private int damagePerSec = 5;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<A_TakesDamage>())
        {
            CauseDamage(other);
        }
    }

    private void CauseDamage(Collider other)
    {
        //Debug.Log(Time.time);
        if (Time.time % 1 == 0)
        {
            other.GetComponent<A_TakesDamage>().TakeDamage(damagePerSec);
            audioSource.PlayOneShot(damageSFX);
        }
    }

}
