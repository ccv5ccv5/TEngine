using System;
using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine.SceneManagement;

namespace GameMain
{
    public class ProcedureStartGame : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            StartGame().Forget();
        }

        private async UniTaskVoid StartGame()
        {
            await UniTask.Yield();
            UILoadMgr.HideAll();
            
            // 加载testscene场景
            var sceneModule = ModuleSystem.GetModule<SceneModule>();
            if (sceneModule != null)
            {
                var sceneHandle = sceneModule.LoadScene("Assets/AssetRaw/Scenes/MainScene.unity", LoadSceneMode.Single, false, 100, (handle) =>
                {
                    
                    Log.Info($"Scene {handle.SceneName} loaded successfully.");
                });
                
                // 等待场景加载完成
                while (!sceneHandle.IsDone)
                {
                    await UniTask.Yield();
                }
            }
            else
            {
                Log.Error("SceneModule is invalid.");
            }
        }
    }
}