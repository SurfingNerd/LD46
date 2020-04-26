using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ManagerBase
{
    public enum GameDifficulty
    {
        Easy,
        Normal,
        Hard
    }


    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<ManagerBase> CoreManagerTemplates = new List<ManagerBase>();
    public List<ManagerBase> GameManagerTemplates = new List<ManagerBase>();

    public List<Level> ListLevelTemplates = new List<Level>();
    public List<BodySurgery> ListSurgeryBodyTemplates = new List<BodySurgery>();


    List<Level> ListSpawnedLevels = new List<Level>();
    Level CurrentLevel;

    PlayerState Player = new PlayerState();

    int CurrentLevelIndex = 0;

    bool bIsChangingLevels = false;

    public GameDifficulty Difficulty = GameDifficulty.Normal;

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

        //for(int i = 0; i < ListLevelTemplates.Count; ++i)
        //{
        //    Level newLevel = Instantiate(ListLevelTemplates[0]);
        //    newLevel.gameObject.SetActive(false);
        //    ListSpawnedLevels.Add(newLevel);
        //}
        CurrentLevel = Instantiate(ListLevelTemplates[CurrentLevelIndex]);
        CurrentLevel.StartLevel();

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

    public Level GetCurrentLevel()
    {
        return CurrentLevel;
    }

    public void CompleteLevel()
    {
        CurrentLevel.EndLevel();

        SurgeryManager.Instance.StartSurgery();
        OutsideManager.Instance.gameObject.SetActive(false);

        //SetNextLevel();
    }

    public void NotifySurgeryEnded()
    {
        if(CurrentLevelIndex == 4)
        {
            HUD.Instance.ShowGameWinScreen();
            AudioManager.Instance.PlaySoundOneShot(AudioManager.Instance.ClipFinale);
        }
        else
        {
            HUD.Instance.GetComponent<Canvas>().worldCamera = OutsideManager.Instance.CamRef;
            SurgeryManager.Instance.gameObject.SetActive(false);
            OutsideManager.Instance.gameObject.SetActive(true);
            SetNextLevel();
            AudioManager.Instance.SwitchMusic(AudioManager.Instance.ClipMusicWander);
        }
    }

    void SetNextLevel()
    {
        bIsChangingLevels = true;
        Destroy(CurrentLevel.gameObject);   
        CurrentLevelIndex++;

        CurrentLevel = Instantiate(ListLevelTemplates[CurrentLevelIndex]);
        CurrentLevel.StartLevel();
        bIsChangingLevels = false;
    }

    public void RestartLevel()
    {
        bIsChangingLevels = true;
        Destroy(CurrentLevel.gameObject);

        CurrentLevel = Instantiate(ListLevelTemplates[CurrentLevelIndex]);
        CurrentLevel.StartLevel();
        CharacterPlayer.instance.SetCurrentCorpse(null);
        CharacterPlayer.instance.SetCaught(false);
        HUD.Instance.SetGetCaught(false);
        bIsChangingLevels = false;
    }

    public bool IsChangingLevels()
    {
        return bIsChangingLevels;
    }
}
