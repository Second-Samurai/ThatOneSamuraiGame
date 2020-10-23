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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player")) 
        {
            _playerSFX.walkingClip = newClip;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
