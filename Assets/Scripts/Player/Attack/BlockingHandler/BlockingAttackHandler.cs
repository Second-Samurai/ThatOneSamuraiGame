using Player.Animation;
using UnityEngine;

namespace ThatOneSamuraiGame
{

    public class BlockingAttackHandler : MonoBehaviour
    {
        [Header("Block Variables")]
        public bool bIsBlocking = false;
        public float blockTimer = 0f;
        public float blockCooldown;
        public bool bCanBlock = true;
        public bool bInputtingBlock = false;
        
        [Header("Parry Variables")]
        public bool bIsParrying;
        public float parryTimer = 0f; 
        bool _bDontCheckParry = false;
        public ParryEffects parryEffects;
        
        private PlayerSFX playerSFX;
        private IKPuppet _IKPuppet;
        
        private PlayerAnimationComponent m_PlayerAnimationComponent;
        
        private void Start()
        {
            playerSFX = gameObject.GetComponent<PlayerSFX>();
            _IKPuppet = GetComponent<IKPuppet>();
        }
        
        private void Update()
        {
            CheckBlockCooldown();
            if (_bDontCheckParry && !bInputtingBlock && bIsBlocking) EndBlock(); 
        }

        public void StartBlock()
        {
            if (!bIsBlocking && blockTimer == 0f && bCanBlock)
            {
                playerSFX.Armour();
                bIsBlocking = true;
                _bDontCheckParry = false;
                parryEffects.PlayGleam();
                _IKPuppet.EnableIK();
                bInputtingBlock = true;
                bIsParrying = true;
                parryTimer = 0f;
            }
            else
            {
                Debug.LogError("bIsblocking: " + bIsBlocking + " blockTimer: " + blockTimer + " bcanBlock: " + bCanBlock);
            }
        }
        
        public void EndBlock()
        {
            bIsBlocking = false;
            bIsParrying = false;
            parryTimer = 0f;
            _IKPuppet.DisableIK();
            SetBlockCooldown();
            
        }
        
        private void CheckBlockCooldown()
        {
            if (blockTimer != 0f)
            {
                if (blockTimer > 0f)
                {
                    blockTimer -= Time.deltaTime;
                }
                if (blockTimer < 0f)
                    blockTimer = 0f;
            }
        }
        
        // Seems out of place as there is a start block method
        public void TriggerBlock(GameObject attacker)
        {
            //rotate to face attacker
            parryEffects.PlayBlock();
            bIsBlocking = false;
            m_PlayerAnimationComponent.TriggerGuardBreak();
            _IKPuppet.DisableIK();
        }
        
        public void SetBlockCooldown()
        {
            blockTimer = blockCooldown;
        }
        
        public void DisableBlock()
        {
            bCanBlock = false;
            _IKPuppet.DisableIK();
        }

        public void EnableBlock()
        {
            bCanBlock = true;
        }
        
    }

}