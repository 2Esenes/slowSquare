using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if GAME_MONOTIZE
public class GameMonotizeBannerAdManager : IBannerAdManager
{
    // Start is called before the first frame update
    public void DisplayBanner()
    {

    }

    public void HideBanner()
    {
        
    }

    public void LoadAds(IronSourceBannerSizeEnum bannerSize, BannerRect bannerRect, IronSourceBannerPosition bannerPosition)
    {

    }

    public void LoadFromAnotherInstance()
    {
  
    }

    public void RegisterIronsSourceBannerEvents()
    {
  
    }

    public void RegisterOnAdClickedEvent(Action<IronSourceAdInfo> method)
    {
    
    }

    public void RegisterOnAdKLeftApplicationEvent(Action<IronSourceAdInfo> method)
    {
 
    }

    public void RegisterOnAdLoadedEvent(Action<IronSourceAdInfo> method)
    {

    }

    public void RegisterOnAdLoadFailedEvent(Action<IronSourceError> method)
    {
  
    }

    public void RegisterOnAdScreenDismissedEvent(Action<IronSourceAdInfo> method)
    {
 
    }

    public void RegisterOnAdScreenPresentedEvent(Action<IronSourceAdInfo> method)
    {

    }

    public void TerminateAd()
    {
  
    }

    public void UnRegisterOnAdClickedEvent(Action<IronSourceAdInfo> method)
    {
   
    }

    public void UnRegisterOnAdKLeftApplicationEvent(Action<IronSourceAdInfo> method)
    {

    }

    public void UnRegisterOnAdLoadedEvent(Action<IronSourceAdInfo> method)
    {

    }

    public void UnRegisterOnAdLoadFailedEvent(Action<IronSourceError> method)
    {

    }

    public void UnRegisterOnAdScreenDismissedEvent(Action<IronSourceAdInfo> method)
    {

    }

    public void UnRegisterOnAdScreenPresentedEvent(Action<IronSourceAdInfo> method)
    {
   
    }
}
#endif