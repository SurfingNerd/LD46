using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveGameManager : ManagerBase
{
    public static SaveGameManager instance = null;

    public PlayerState LocalCopyOfData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Save()
    {
        //if (!Directory.Exists("Saves"))
        //{
        //    Directory.CreateDirectory("Saves");
        //}

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream saveFile = File.Create(GetSaveGamePath());
        PlayerState data = GameManager.Instance.GetPlayerState();
        data.Version = Application.version;


        LocalCopyOfData = data;

        Debug.Log("Saved offline: " + LocalCopyOfData.ToString());

        formatter.Serialize(saveFile, data);

        saveFile.Close();
    }

    public string GetSaveGamePath()
    {
        return Application.persistentDataPath + "/save" + Application.version + ".binary";
    }

    public void Load()
    {
        if (File.Exists(GetSaveGamePath()))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Open(GetSaveGamePath(), FileMode.Open);

            try
            {
                LocalCopyOfData = (PlayerState)formatter.Deserialize(saveFile);
                GameManager.Instance.SetPlayerState(LocalCopyOfData);
                if (LocalCopyOfData.Version != Application.version)
                {
                    ClearSave();
                    Debug.Log("Clearing save due to version change. Previous ver: " + LocalCopyOfData.Version + " Current ver: " + Application.version);
                }
                Debug.Log("Loaded offline: " + LocalCopyOfData.ToString());

            }
            catch (Exception e)
            {
                Debug.LogError("Failed load, clearing save");
                ClearSave();
            }


            saveFile.Close();
        }
        else
        {
            Save();
        }
    }

    public void ClearSave()
    {
        if (File.Exists(GetSaveGamePath()))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream saveFile = File.Create(GetSaveGamePath());
            LocalCopyOfData = new PlayerState();

            formatter.Serialize(saveFile, LocalCopyOfData);

            saveFile.Close();
        }
        else
        {
            Save();
        }
    }

    public override void InitManager()
    {
        base.InitManager();

        Load();
    }

    // Convert an object to a byte array
    public static byte[] ObjectToByteArray(object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static object ByteArrayToObject(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }
    }

    public void SaveFromState(PlayerState state)
    {
        GameManager.Instance.SetPlayerState(state);
        Save();
    }
}
