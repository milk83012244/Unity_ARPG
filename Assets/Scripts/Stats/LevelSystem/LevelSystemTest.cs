using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystemTest : MonoBehaviour
{
    [SerializeField] private LevelSystemExpDeBugWindow deBugWindow;

    LevelSystem levelSystem;
    LevelSystemAnimated levelSystemAnimated;

    private void Awake()
    {
        levelSystem = new LevelSystem();

        deBugWindow.SetLevelSystem(levelSystem);

        levelSystemAnimated = new LevelSystemAnimated(levelSystem);
        deBugWindow.SetLevelSystemAnimated(levelSystemAnimated);
    }
    private void Update()
    {
        levelSystemAnimated.Update();
    }
}
