using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFootSfx : MonoBehaviour
{
    private PlayerSFX _playerSFX;
    public AudioClip newClip;

    // Start is called before the first frame update
    void Start()
    {
        _playerSFX = GameManager.instance.playerController.gameObject.GetComponent<PlayerSFX>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            _playerSFX.walkingClip = newClip;
        }
    }
  
}
