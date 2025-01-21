using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using TEngine;

public class MainHome : MonoBehaviour
{
    public GameObject CanvasDesktop;
    private AssetHandle _windowHandle;

    private void Start()
    {        
        // 加载主页面
        // _windowHandle = YooAssets.LoadAssetAsync<GameObject>("UIHome");
        // yield return _windowHandle;
        // _windowHandle.InstantiateSync(CanvasDesktop.transform);

        // TODO: 加载主页面UI
        // GameModule.UI.OpenWindow(UIHomeWindow.UIName);
        // GameModule.UI.ShowUI<UIHomeWindow>();
    }

    private void OnDestroy()
    {
        // 释放资源句柄
        // if (_windowHandle != null)
        // {
        //     _windowHandle.Release();
        //     _windowHandle = null;
        // }

        // // 切换场景的时候释放资源
        // if (YooAssets.Initialized)
        // {
        //     var package = YooAssets.GetPackage("DefaultPackage");
        //     var operation = package.UnloadUnusedAssetsAsync();
        //     operation.WaitForAsyncComplete();
        // }
    }
}