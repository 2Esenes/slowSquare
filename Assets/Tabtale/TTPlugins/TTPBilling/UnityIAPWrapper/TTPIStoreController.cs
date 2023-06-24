using System;
using System.Collections.Generic;

namespace Tabtale.TTPlugins.UnityIAPWrapper
{
    public interface TTPIStoreController
    {
        TTPIProductCollection products { get; }
        void ConfirmPendingPurchase(TTPIProduct product);
        void FetchAdditionalProducts(HashSet<TTPIProductDefinition> products, Action successCallback, Action<TTPIInitializationFailureReason> failCallback);
        void InitiatePurchase(string productId);
        void InitiatePurchase(TTPIProduct product);
    }
}

