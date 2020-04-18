using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionDataDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI TextVersionCode;

    [SerializeField]
    TextMeshProUGUI TextVersionName;

    // Start is called before the first frame update
    void Start()
    {
        TextVersionCode.text = "v" + Application.version;
        TextVersionName.text = "Beta";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //int versioncode =  context().getPackageManager().getPackageInfo(context().getPackageName(), 0).versionCode;
    public static int GetVersionCode()
    {
        AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageMngr = context.Call<AndroidJavaObject>("getPackageManager");
        string packageName = context.Call<string>("getPackageName");
        AndroidJavaObject packageInfo = packageMngr.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
        return packageInfo.Get<int>("versionCode");
    }

    //int versionName =  context().getPackageManager().getPackageInfo(context().getPackageName(), 0).versionName;
    public static string GetVersionName()
    {
        AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageMngr = context.Call<AndroidJavaObject>("getPackageManager");
        string packageName = context.Call<string>("getPackageName");
        AndroidJavaObject packageInfo = packageMngr.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
        return packageInfo.Get<string>("versionName");
    }
}
