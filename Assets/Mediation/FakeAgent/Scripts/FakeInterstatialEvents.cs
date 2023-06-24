using System;

public class FakeInterstatialEvents 
{
    public static Action<IronSourceAdInfo> OnInterstatialAdOpened;
    public static Action<IronSourceAdInfo> OnInterstatialAdClosed;
    public static Action<IronSourceError> OnInterstatialAdFailedToLoad;
    public static Action<IronSourceError, IronSourceAdInfo> OnInterstatialAdShownFailed;
    public static Action<IronSourceAdInfo> OnInterstatialAdSuccesed;
    public static Action<IronSourceAdInfo> OnInterstailAdReady;
    public static Action<IronSourceAdInfo> OnInterstatialAdClicked;
}
