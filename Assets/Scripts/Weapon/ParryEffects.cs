using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryEffects : MonoBehaviour
{
    public ParticleSystem parry, block, gleam;



    public void PlayParry()
    {
        parry.Play();
    }

    public void PlayBlock()
    {
        block.Play();
    }

    public void PlayGleam()
    {
        gleam.Play();
    }
}
