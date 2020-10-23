using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFootSfx : MonoBehaviour
{
    private PlayerSFX _playerSFX;
    public AudioClip newClip;
    private AudioClip[] array;

    // Start is called before the first frame update
    void Start()
    {
        _playerSFX = GameManager.instance.playerController.gameObject.GetComponent<PlayerSFX>();
        string arrayName = newClip.name;
        arrayName = arrayName.ToLower().Trim().Replace(" ", "");
        if (arrayName.Contains("grass"))
        {
            array = _playerSFX.grassRoll;
        }
        else if (arrayName.Contains("stone"))
        {
            array = _playerSFX.PebbleRoll;
        }
        else if (arrayName.Contains("wood"))
        {
            array = _playerSFX.woodRoll;
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            _playerSFX.walkingClip = newClip;
            _playerSFX.rollArray = array;
        }
    }
  
}
