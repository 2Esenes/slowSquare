using System;
using System.Collections.Generic;

namespace Tabtale.TTPlugins.UnityIAPWrapper
{
    public interface TTPIAppleExtensions
    {
        void RestoreTransactions(Action<bool> callback);
        void RegisterPurchaseDeferredListener(Action<TTPIProduct> callback);
        Dictionary<string, string> GetIntroductoryPriceDictionary();
    }

    public interface TTPIGooglePlayStoreExtensions
    {
        void RestoreTransactions(Action<bool> success);
    }

    public interface TTPIExtensionProvider
    {
        T GetExtension<T>();
    }
}

