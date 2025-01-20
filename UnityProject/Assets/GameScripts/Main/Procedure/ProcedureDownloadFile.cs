﻿using System;
using Cysharp.Threading.Tasks;
using TEngine;
using YooAsset;
using ProcedureOwner = TEngine.IFsm<TEngine.IProcedureManager>;
using Utility = TEngine.Utility;

namespace GameMain
{
    public class ProcedureDownloadFile : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        private ProcedureOwner _procedureOwner;

        private float _lastUpdateDownloadedSize;
        private float _totalSpeed;
        private int _speedSampleCount;

        private float CurrentSpeed
        {
            get
            {
                float interval = Math.Max(GameTime.deltaTime, 0.01f); // 防止deltaTime过小
                var sizeDiff = GameModule.Resource.Downloader.CurrentDownloadBytes - _lastUpdateDownloadedSize;
                _lastUpdateDownloadedSize = GameModule.Resource.Downloader.CurrentDownloadBytes;
                var speed = sizeDiff / interval;

                // 使用滑动窗口计算平均速度
                _totalSpeed += speed;
                _speedSampleCount++;
                return _totalSpeed / _speedSampleCount;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            _procedureOwner = procedureOwner;

            Log.Info("开始下载更新文件！");

            UILoadMgr.Show(UIDefine.UILoadUpdate, "开始下载更新文件...");

            BeginDownload().Forget();
        }

        private async UniTaskVoid BeginDownload()
        {
            var downloader = GameModule.Resource.Downloader;

            // 注册下载回调
            // downloader.OnDownloadErrorCallback = OnDownloadErrorCallback;
            // downloader.OnDownloadProgressCallback = OnDownloadProgressCallback;
            downloader.DownloadErrorCallback = OnDownloadErrorCallback;
            downloader.DownloadUpdateCallback = OnDownloadProgressCallback;
            downloader.BeginDownload();
            await downloader;

            // 检测下载结果
            if (downloader.Status != EOperationStatus.Succeed)
                return;

            ChangeState<ProcedureDownloadOver>(_procedureOwner);
        }

        private void OnDownloadErrorCallback(DownloadErrorData data)
        {
            // string fileName, string error;
            // data.PackageName = _packageName;
            // fileName = data.FileName;
            // data.ErrorInfo = failedDownloader.Error;
            UILoadTip.ShowMessageBox($"Failed to download file : {data.FileName}", MessageShowType.TwoButton,
                LoadStyle.StyleEnum.Style_Default,
                () => { ChangeState<ProcedureCreateDownloader>(_procedureOwner); }, UnityEngine.Application.Quit);
        }

        private void OnDownloadProgressCallback(DownloadUpdateData data)
        {
            string currentSizeMb = (data.CurrentDownloadBytes / 1048576f).ToString("f1");
            string totalSizeMb = (data.TotalDownloadBytes / 1048576f).ToString("f1");
            float progressPercentage = GameModule.Resource.Downloader.Progress * 100;
            string speed = Utility.File.GetLengthString((int)CurrentSpeed);

            string line1 = Utility.Text.Format("正在更新，已更新 {0}/{1} ({2:F2}%)", data.CurrentDownloadCount, data.TotalDownloadCount, progressPercentage);
            string line2 = Utility.Text.Format("已更新大小 {0}MB/{1}MB", currentSizeMb, totalSizeMb);
            string line3 = Utility.Text.Format("当前网速 {0}/s, 剩余时间 {1}", speed, GetRemainingTime(data.TotalDownloadBytes, data.CurrentDownloadBytes, CurrentSpeed));

            LoadUpdateLogic.Instance.DownProgressAction?.Invoke(GameModule.Resource.Downloader.Progress);
            UILoadMgr.Show(UIDefine.UILoadUpdate, $"{line1}\n{line2}\n{line3}");

            Log.Info($"{line1} {line2} {line3}");
        }

        private string GetRemainingTime(long totalBytes, long currentBytes, float speed)
        {
            int needTime = 0;
            if (speed > 0)
            {
                needTime = (int)((totalBytes - currentBytes) / speed);
            }
            TimeSpan ts = new TimeSpan(0, 0, needTime);
            return ts.ToString(@"mm\:ss");
        }



    }
}
