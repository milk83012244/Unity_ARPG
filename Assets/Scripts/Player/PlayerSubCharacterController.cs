using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSubCharacterController : MonoBehaviour
{
    private PlayerCharacterSwitch playerCharacterSwitch;

    public GameObject subCharacterObj;
    public List<Vector3> positionList;
    public int distance = 20;
    public float speed = 0.1f;
    private void OnEnable()
    {
        playerCharacterSwitch = GetComponent<PlayerCharacterSwitch>();
    }
    private void Awake()
    {
        foreach (KeyValuePair<string, GameObject> name in playerCharacterSwitch.subControlCharacter)
        {
            subCharacterObj = playerCharacterSwitch.subControlCharacter[name.Key];
        }
    }

    private void FixedUpdate()
    {
        positionList.Add(transform.position);

        if (positionList.Count > distance)
        {
            positionList.RemoveAt(0);
            subCharacterObj.transform.position = positionList[0];
        }
    }
}
