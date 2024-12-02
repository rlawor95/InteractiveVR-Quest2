
using System.IO;
using IMFINE.Utils;
using UnityEngine;

public class ApplicationDirectory : MonoSingleton<ApplicationDirectory>
{
#pragma warning disable CS0414
    public delegate void ApplicationDirectoryEvent();
    public event ApplicationDirectoryEvent Prepared;

    private bool _isPrepared = false;
    private DirectoryInfo _internalDataDirectoryInfo = null;
    private DirectoryInfo _externalDataDirectoryInfo = null;

    public bool isPrepared { get { return _isPrepared; } }
    public DirectoryInfo dataDirectoryInfo
    {
        get
        {
            if (_internalDataDirectoryInfo == null) SetStorageDirectory();
            return _internalDataDirectoryInfo;
        }
    }

    public DirectoryInfo externalDataDirectoryInfo
    {
        get
        {
            if (_externalDataDirectoryInfo == null) SetStorageDirectory(true);
            return _externalDataDirectoryInfo;
        }
    }

    private void CheckStorageDirectory(DirectoryInfo directoryInfo)
    {
        if (!directoryInfo.Exists)
        {
            TraceBox.Log("> " + GetType().Name + " CheckStorageDirectory / The storage directory does not exist.");
            Debug.Log("> " + GetType().Name + " CheckStorageDirectory / The storage directory does not exist.");
            return;
        }
        else
        {
            TraceBox.Log("> " + GetType().Name + " CheckStorageDirectory / OK!");
            Debug.Log("> " + GetType().Name + " CheckStorageDirectory / OK!");
            return;
        }
    }

    void Awake()
    {
        // Android : /storage/emulated/0/imfine/[productName]/Data
        // Editor : [ProjectFolderName]/Data
        // PC : [buildFolderName]/Data
        // iOS : /var/mobile/Applications/프로그램ID/Documents

        SetStorageDirectory();

        if (!_internalDataDirectoryInfo.Exists)
        {
            try
            {
                Directory.CreateDirectory(_internalDataDirectoryInfo.FullName);
                TraceBox.Log(_internalDataDirectoryInfo.Name + " Folder created");
                Debug.Log(_internalDataDirectoryInfo.Name + " Folder created / " + _internalDataDirectoryInfo.FullName);
            }
            catch (System.Exception e)
            {
                TraceBox.Log(e);
                Debug.Log(e);
            }
        }

        _isPrepared = true;
        Prepared?.Invoke();
    }

    private void SetStorageDirectory(bool isExternal = false)
    {
        if (!isExternal && _internalDataDirectoryInfo != null) return;
        if (isExternal && _externalDataDirectoryInfo != null) return;

#if UNITY_ANDROID && !UNITY_EDITOR

        DirectoryInfo storageDirectoryInfo;
        if (!isExternal)
        {
            storageDirectoryInfo = new DirectoryInfo(Application.persistentDataPath);
        }
        else
        {
            storageDirectoryInfo = new DirectoryInfo(GetAndroidExternalPath());
        }

        Debug.Log("> " + GetType().Name + " / Android Storage Path: " + storageDirectoryInfo.FullName);

        CheckStorageDirectory(storageDirectoryInfo);

        string imfinePath = Path.Combine(storageDirectoryInfo.FullName, "imfine");
        DirectoryInfo imfineDirectoryInfo = new DirectoryInfo(imfinePath);
        if (!imfineDirectoryInfo.Exists)
        {
            try
            {
                Directory.CreateDirectory(imfineDirectoryInfo.FullName);
                TraceBox.Log(imfineDirectoryInfo.Name + " Folder created");
                Debug.Log(imfineDirectoryInfo.Name + " Folder created / " + imfineDirectoryInfo.FullName);
            }
            catch (System.Exception e)
            {
                TraceBox.Log(e);
                Debug.Log(e);
            }
        }

        string productPath = Path.Combine(imfinePath, Application.productName.Replace(" ", ""));
        DirectoryInfo productDirectoryInfo = new DirectoryInfo(productPath);
        if (!productDirectoryInfo.Exists)
        {
            try
            {
                Directory.CreateDirectory(productDirectoryInfo.FullName);
                TraceBox.Log(productDirectoryInfo.Name + " Folder created");
                Debug.Log(productDirectoryInfo.Name + " Folder created / " + productDirectoryInfo.FullName);
            }
            catch (System.Exception e)
            {
                TraceBox.Log(e);
                Debug.Log(e);
            }
        }

        string dataPath = Path.Combine(productPath, "Data");
        DirectoryInfo dataDirectoryInfo = new DirectoryInfo(dataPath);
        if (isExternal)
        {
            if (!dataDirectoryInfo.Exists)
            {
                try
                {
                    Directory.CreateDirectory(dataDirectoryInfo.FullName);
                    TraceBox.Log(dataDirectoryInfo.Name + " Folder created");
                    Debug.Log(dataDirectoryInfo.Name + " Folder created / " + dataDirectoryInfo.FullName);
                }
                catch (System.Exception e)
                {
                    TraceBox.Log(e);
                    Debug.Log(e);
                }
            }
            _externalDataDirectoryInfo = dataDirectoryInfo;
        }
        else _internalDataDirectoryInfo = dataDirectoryInfo;

#elif UNITY_IOS && !UNITY_EDITOR
        DirectoryInfo storageDirectoryInfo = new DirectoryInfo(Application.persistentDataPath);

        Debug.Log("> " + GetType().Name + " / iOS Storage Path: " + storageDirectoryInfo.FullName);

        CheckStorageDirectory(storageDirectoryInfo);

        _internalDataDirectoryInfo = new DirectoryInfo(storageDirectoryInfo.FullName);

#else
        DirectoryInfo dataDirectoryInfo = new DirectoryInfo(Application.dataPath);
        _internalDataDirectoryInfo = new DirectoryInfo(dataDirectoryInfo.Parent.FullName + "/Data");
#endif

        Debug.Log("> " + GetType().Name + " / imfineDataDirectoryInfo.FullName: " + _internalDataDirectoryInfo.FullName);
        TraceBox.Log("> " + GetType().Name + " / imfineDataDirectoryInfo.FullName: " + _internalDataDirectoryInfo.FullName);
    }

    public static string GetAndroidExternalPath()
    {
        string[] potentialDirectories = new string[]
        {
            "/storage/emulated/0/Download",
            "/sdcard/Download",
            "/mnt/sdcard/Download",
            "/storage/sdcard0/Download"
        };

        if (Application.platform == RuntimePlatform.Android)
        {
            for (int i = 0; i < potentialDirectories.Length; i++)
            {
                if (Directory.Exists(potentialDirectories[i]))
                {
                    return potentialDirectories[i];
                }
            }
        }
        return "";
    }
}
