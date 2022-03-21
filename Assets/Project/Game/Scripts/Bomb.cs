using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IDestroyable
{
    public event Action OnDie;

    [SerializeField] private int range;
    [SerializeField] private LayerMask unitsLayer;
    [SerializeField] private ParticleSystem blowUP;

    public void MustDestroy()
    {
        OnDie -= BlowUpEffect;
        Destroy(gameObject);
        Destroy(this);
    }

    public void SetPlant(float _time)
    {
        if (OnDie == null)
            OnDie += BlowUpEffect;
        StartCoroutine(Boom(_time));
    }

    private IEnumerator TimerToBomb(float timeToBLOW)
    {
        yield return new WaitForSeconds(timeToBLOW);
        CallculateHitedColliders();
        OnDie?.Invoke();
    }

    private IEnumerator Boom(float timeToBLOW)
    {
        yield return TimerToBomb(timeToBLOW);
        yield return new WaitForSeconds(1f);
        MustDestroy();
    }

    private void BlowUpEffect()
    {
        blowUP.Play();
    }

    public void CallculateHitedColliders()
    {
        var area = Physics.OverlapSphere(transform.position, range, unitsLayer);
        if(area.Length > 0)
        {
            foreach (var item in area)
            {
                IDamageable deadman = item.GetComponent<Enemy>();
                if (deadman != null)
                {
                    deadman.MakeYouDirtyOrCleanUP();
                }
            }
        }
        
    }
}
