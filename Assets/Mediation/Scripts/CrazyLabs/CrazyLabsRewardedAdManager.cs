#if CRAZY_LABS
using System;
using System.Collections;
using System.Collections.Generic;
using Tabtale.TTPlugins;
using UnityEditor.Build;
using UnityEngine;

public class CrazyLabsRewardedAdManager : IRewardedAdManager
{
    private bool _isRewardedReady = false;
    public bool IsRewardedAdReady()
    {
        return TTPRewardedAds.IsReady();
    }

    public void LoadAds()
    {

    }

    public void RegisteOnAdAvailableEvent(Action<IronSourceAdInfo> method)
    {

    }

    public void RegisterIronSourceEvents()
    {
    }

    public void RegisterOnAdClickedEvent(Action<IronSourcePlacement, IronSourceAdInfo> method)
    {
    }

    public void RegisterOnAdClosedEvent(Action<IronSourceAdInfo> method)
    {
    }

    public void RegisterOnAdLoadFailedEvent(Action<IronSourceError> method)
    {
    }

    public void RegisterOnAdOpenedEvent(Action<IronSourceAdInfo> method)
    {
    }

    public void RegisterOnAdReadyEvent(Action<IronSourceAdInfo> method)
    {
    }

    public void RegisterOnAdShowFailedEvent(Action<IronSourceError, IronSourceAdInfo> method)
    {
    }

    public void RegisterOnAdUnavailableEvent(Action method)
    {
    }

    public void RegisterOnUserEarnedRewarededEvent(Action<IronSourcePlacement, IronSourceAdInfo> method)
    {
    }

    public void ShowAd()
    {
        TTPRewardedAds.Show("Reward", OnUserEarnedReward);
    }

    public void TerminateAd()
    {
    }

    public void UnRegisteOnAdAvailableEvent(Action<IronSourceAdInfo> method)
    {
    }

    public void UnRegisterOnAdClickedEvent(Action<IronSourcePlacement, IronSourceAdInfo> method)
    {
    }

    public void UnRegisterOnAdClosedEvent(Action<IronSourceAdInfo> method)
    {
    }

    public void UnRegisterOnAdLoadFailedEvent(Action<IronSourceError> method)
    {
    }

    public void UnRegisterOnAdOpenedEvent(Action<IronSourceAdInfo> method)
    {
    }

    public void UnRegisterOnAdReadyEvent(Action<IronSourceAdInfo> method)
    {
    }

    public void UnRegisterOnAdShowFailedEvent(Action<IronSourceError, IronSourceAdInfo> method)
    {
    }

    public void UnRegisterOnAdUnavailableEvent(Action method)
    {
    }

    public void UnRegisterOnUserEarnedRewarededEvent(Action<IronSourcePlacement, IronSourceAdInfo> method)
    {
    }

    private void OnUserEarnedReward(bool isEarned)
    {

        OnAdClosed();
    }

    private void OnAdClosed()
    {

    }
}
#endif