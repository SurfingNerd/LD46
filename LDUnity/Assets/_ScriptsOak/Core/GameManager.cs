using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ManagerBase
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<ManagerBase> CoreManagerTemplates = new List<ManagerBase>();
    public List<ManagerBase> GameManagerTemplates = new List<ManagerBase>();

    public List<Level> ListLevels = new List<Level>();

    List<Level> ListSpawnedLevels = new List<Level>();

    PlayerState Player = new PlayerState();

    int CurrentLevelIndex = 0;

    public int GetCurrentLevelIndex()
    {
        return CurrentLevelIndex;
    }

    public void LoadNextLevel()
    {

    }

    private void Start()
    {
        InitManager();
    }

    private void Update()
    {

    }

    public override void InitManager()
    {
        base.InitManager();

        for(int i = 0; i < ListLevels.Count; ++i)
        {
            ListLevels[i].gameObject.SetActive(false);
            //Level newLevel = Instantiate(ListLevels[i]);
            //newLevel.gameObject.SetActive(false);
            //ListSpawnedLevels.Add(newLevel);
        }

        ListLevels[0].StartLevel();

        for (int i = 0; i < CoreManagerTemplates.Count; ++i)
        {
            ManagerBase manager = Instantiate(CoreManagerTemplates[i]);
            manager.transform.SetParent(gameObject.transform);
            manager.InitManager();
        }

        for (int i = 0; i < GameManagerTemplates.Count; ++i)
        {
            ManagerBase manager = Instantiate(GameManagerTemplates[i]);
            manager.transform.SetParent(gameObject.transform);
            manager.InitManager();
        }

    }

    public PlayerState GetPlayerState()
    {
        return Player;
    }

    public void SetPlayerState(PlayerState state)
    {
        Player = state;
    }


}
