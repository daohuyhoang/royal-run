using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float collisionCooldown = 1f;
    [SerializeField] float adjustChangeMoveSpeedAmount = -2f;
    
    LevelGenerator levelGenerator;
    float cooldownTimer = 0f;
    const string hitString = "Hit";

    void Start()
    {
        levelGenerator = FindFirstObjectByType<LevelGenerator>();
    }
    void Update()
    {
        cooldownTimer += Time.deltaTime;    
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (cooldownTimer < collisionCooldown) return;
        levelGenerator.ChangeChunkMoveSpeed(adjustChangeMoveSpeedAmount);
        animator.SetTrigger(hitString);
        cooldownTimer = 0f;
    }

}