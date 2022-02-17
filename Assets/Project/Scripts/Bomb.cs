using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IDestroeable
{
    public event Action OnDie;
    public Action OnBlowUp;

    [SerializeField] int range;
    [SerializeField] LayerMask _unitsLayer;
    [SerializeField] ParticleSystem _blowUP;

    public void Resycle()
    {
        OnBlowUp -= BlowUpEffect;
        Destroy(gameObject);
        Destroy(this);
    }

    public void SetPlant(float _time)
    {
        if (OnBlowUp == null)
            OnBlowUp += BlowUpEffect;
        StartCoroutine(Boom(_time));
    }

    private IEnumerator TimerToBomb(float timeToBLOW)
    {
        yield return new WaitForSeconds(timeToBLOW);
        CallculateHitedColliders();
        OnBlowUp?.Invoke();
    }

    private IEnumerator Boom(float timeToBLOW)
    {
        yield return TimerToBomb(timeToBLOW);
        yield return new WaitForSeconds(1f);
        Resycle();
    }

    private void BlowUpEffect()
    {
        _blowUP.Play();
    }

    public void CallculateHitedColliders()
    {
        var area = Physics.OverlapSphere(transform.position, range, _unitsLayer);
        if(area.Length > 0)
        {
            foreach (var item in area)
            {
                IDamageable deadman = item.GetComponent<GameContent>();
                if (deadman != null)
                {
                    deadman.MakeYouDirtyOrCleanUP();
                }
            }
        }
        
    }
}
