using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 玩家控制器 負責玩家的移動控制
/// </summary>
public class PlayerController : MonoBehaviour
{
    private PlayerInput input;
    private Rigidbody2D rig2D;
    private PlayerStateMachine stateMachine;
    private PlayerCharacterSwitch characterSwitch;
    private CharacterStatsDataMono characterStats;

    public float MoveSpeedX => Mathf.Abs(rig2D.velocity.x);
    public float MoveSpeedY => Mathf.Abs(rig2D.velocity.y);

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rig2D = GetComponent<Rigidbody2D>();
        stateMachine = GetComponent<PlayerStateMachine>();
        characterSwitch = GetComponent<PlayerCharacterSwitch>();
        characterStats = GetComponent<CharacterStatsDataMono>();

    }
    private void Start()
    {
        input.EnableGamePlayInputs();
    }
    private void Update()
    {

    }
    public void Move(float speed)
    {
        SetVelocityX(speed * input.AxisX);
        SetVelocityY(speed * input.AxisY);
    }
    public void MoveXY(float speedx, float speedy)
    {
        SetVelocityXY(speedx * input.AxisX, speedy * input.AxisY);
    }

    public void SetVelocity(Vector3 velocity)
    {
        rig2D.velocity = velocity;
    }
    public void SetVelocityX(float velocityX)
    {
        rig2D.velocity = new Vector3(velocityX, rig2D.velocity.y);
    }
    public void SetVelocityY(float velocityY)
    {
        rig2D.velocity = new Vector3(rig2D.velocity.x, velocityY);
    }
    public void SetVelocityXY(float speedX, float speedY)
    {
        float XY = Mathf.Sqrt(speedX * speedX + speedY * speedY);
        rig2D.velocity = new Vector3(speedX * Mathf.Sqrt(0.5f), speedY * Mathf.Sqrt(0.5f));
    }
    public void DodgeMoveX(Vector2 dodgeDir, float speed)
    {
        rig2D.velocity = dodgeDir * speed * 1;
    }
    public void DodgeMoveY(Vector2 dodgeDir, float speed)
    {
        rig2D.velocity = dodgeDir * speed * 1;
    }
}
