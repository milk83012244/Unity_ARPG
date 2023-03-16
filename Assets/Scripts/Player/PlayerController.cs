using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public string currentControlCharacterNames;

    public string[] characterNames;
    public GameObject[] characterObjs;

    private PlayerInput input;
    private Rigidbody2D rig2D;
    private PlayerStateMachine stateMachine;

    /// <summary>
    /// ��e�����
    /// </summary>
    private Dictionary<string, GameObject> currentControlCharacter = new Dictionary<string, GameObject>();
    /// <summary>
    /// ����
    /// </summary>
    private Dictionary<int, string> partys = new Dictionary<int, string>();

    public float MoveSpeedX => Mathf.Abs(rig2D.velocity.x);
    public float MoveSpeedY => Mathf.Abs(rig2D.velocity.y);

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rig2D = GetComponent<Rigidbody2D>();
        stateMachine = GetComponent<PlayerStateMachine>();

        SwitchMainCharacter();

        partys.Clear();
    }
    private void Start()
    {
        input.EnableGamePlayInputs();
    }
    private void Update()
    {

    }
    /// <summary>
    /// �԰��Ҧ�����������
    /// </summary>
    public void SwitchCharacter()
    {

    }
    /// <summary>
    /// �԰��Ҧ�������԰�����
    /// </summary>
    public void BattleModeStartSwitchCharacter()
    {
        characterObjs[0].SetActive(false);
        currentControlCharacter.Clear();
        currentControlCharacter.Add(characterNames[1], characterObjs[1]);
        currentControlCharacter[characterNames[1]].SetActive(true);
        //�����e�����W��
        foreach (KeyValuePair<string,GameObject> name in currentControlCharacter)
        {
            currentControlCharacterNames = name.Key;
        }
        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //�������⪬�A��
    }
    /// <summary>
    /// �����^�D��
    /// </summary>
    public void SwitchMainCharacter()
    {
        if (currentControlCharacter.Count > 0)
        {
            currentControlCharacter.Clear();
        }

        for (int i = 0; i < characterObjs.Length; i++)
        {
            if (i == 0)
            {
                if (currentControlCharacter.Count == 0)
                {
                    currentControlCharacter.Add(characterNames[i], characterObjs[i]);
                    currentControlCharacter[characterNames[0]].SetActive(true);
                    //�����e�����W��
                    foreach (KeyValuePair<string, GameObject> name in currentControlCharacter)
                    {
                        currentControlCharacterNames = name.Key;
                    }
                }
            }
            else
            {
                characterObjs[i].SetActive(false);
            }
        }
        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //�������⪬�A��
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
}
