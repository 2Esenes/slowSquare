using System;

public class FakeBannerEvents 
{
    public static Action<IronSourceAdInfo> OnBannerAdLoadedEvent;
    public static Action<IronSourceAdInfo> OnBannerAdLeftApplicationEvent;
    public static Action<IronSourceAdInfo> OnBannerAdScreenDismissedEvent;
    public static Action<IronSourceAdInfo> OnBannerAdScreenPresentedEvent;
    public static Action<IronSourceAdInfo> OnBannerAdClickedEvent;
    public static Action<IronSourceError> OnBannerAdLoadFailedEvent;

}
