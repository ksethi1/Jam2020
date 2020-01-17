﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float damage;
    [SerializeField] float delay;

    bool canAttack=true;

    ITakeDamage victim;
    bool close;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        victim = collision.GetComponent<ITakeDamage>();
        if(victim!=null)
        {
            close = true;
            StopAllCoroutines();
            StartCoroutine(AttemptHit(victim));
        }
    }

    public void DisableAttack()
    {
        canAttack = false;
    }

    IEnumerator AttemptHit(ITakeDamage victim)
    {
        while(close)
        {
            yield return new WaitForSeconds(delay);
            if(close && canAttack)
                victim.TakeDamage(damage);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ITakeDamage victim = collision.GetComponent<ITakeDamage>();
        if (victim == this.victim)
        {
            close = false;
        }
    }
}
