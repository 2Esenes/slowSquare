using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif


#if UNITY_5_3 || UNITY_5_4_OR_NEWER
using UnityEngine.SceneManagement;
#endif
#if ENABLE_IOS_ON_DEMAND_RESOURCES
using UnityEngine.iOS;
#endif
using System.Collections;
using UnityEngine.Networking;

namespace TTPluginsAssetBundles
{
    public abstract class AssetBundleLoadOperation : IEnumerator
    {
        public object Current
        {
            get
            {
                return null;
            }
        }

        public bool MoveNext()
        {
            return !IsDone();
        }

        public void Reset()
        {
        }

        abstract public bool Update();

        abstract public bool IsDone();
    }

    public abstract class AssetBundleDownloadOperation : AssetBundleLoadOperation
    {
		public bool done { get; protected set; }
        public string assetBundleName { get; private set; }
        public LoadedAssetBundle assetBundle { get; protected set; }
        public string error { get; protected set; }

        protected abstract bool downloadIsDone { get; }
        protected abstract void FinishDownload();

        public override bool Update()
        {
            if (!done && downloadIsDone)
            {
                FinishDownload();
                done = true;
            }

            return !done;
        }

        public override bool IsDone()
        {
            return done;
        }

        public abstract string GetSourceURL();

        public AssetBundleDownloadOperation(string assetBundleName)
        {
            this.assetBundleName = assetBundleName;
        }
    }

#if ENABLE_IOS_ON_DEMAND_RESOURCES
    // Read asset bundle asynchronously from iOS / tvOS asset catalog that is downloaded
    // using on demand resources functionality.
    public class AssetBundleDownloadFromODROperation : AssetBundleDownloadOperation
    {
        OnDemandResourcesRequest request;

        public AssetBundleDownloadFromODROperation(string assetBundleName)
            : base(assetBundleName)
        {
            // Work around Xcode crash when opening Resources tab when a 
            // resource name contains slash character
            request = OnDemandResources.PreloadAsync(new string[] { assetBundleName.Replace('/', '>') });
        }

		protected override bool downloadIsDone {
			get {
				if (request == null) {
					return true;
				}	

				if (request.isDone && string.IsNullOrEmpty (request.error)) {
					return true;
				}

				return false;
			}
		}

		public override bool Update()
		{
			if (done) {
				return false;
			}

			if (request == null || request.isDone) {
				if (request == null || string.IsNullOrEmpty (request.error)) {
					FinishDownload ();
					done = true;
					return false;
				}

				request.Dispose();
				request = OnDemandResources.PreloadAsync (new string[] { assetBundleName.Replace ('/', '>') });
			}

			return true;
		}

        public override string GetSourceURL()
        {
            return "odr://" + assetBundleName;
        }

        protected override void FinishDownload()
        {
            error = request.error;
            if (error != null)
                return;

            var path = "res://" + assetBundleName;

            var bundle = AssetBundle.LoadFromFile(path);
            if (bundle)
            {
                assetBundle = new LoadedAssetBundle(bundle);
                // At the time of unload request is already set to null, so capture it to local variable.
                var localRequest = request;
                // Dispose of request only when bundle is unloaded to keep the ODR pin alive.
                assetBundle.unload += () =>
                {
                    localRequest.Dispose();
                };
            }
            else
            {
                error = "Failed to load " + path;
                request.Dispose();
            }

            request = null;
        }
    }
#endif

#if ENABLE_IOS_APP_SLICING
    // Read asset bundle synchronously from an iOS / tvOS asset catalog
    public class AssetBundleOpenFromAssetCatalogOperation : AssetBundleDownloadOperation
    {
        public AssetBundleOpenFromAssetCatalogOperation(string assetBundleName)
            : base(assetBundleName)
        {
            var path = "res://" + assetBundleName;
            var bundle = AssetBundle.LoadFromFile(path);
			if (bundle)
				assetBundle = new LoadedAssetBundle (bundle);
			else
                error = "Failed to load " + path; 
        }

        protected override bool downloadIsDone { get { return true; } }

        protected override void FinishDownload() {}

        public override string GetSourceURL()
        {
            return "res://" + assetBundleName;
        }
    }
#endif

    public class AssetBundleDownloadFromWebOperation : AssetBundleDownloadOperation
    {
        private UnityWebRequest _www;
        private readonly string _url;

        public AssetBundleDownloadFromWebOperation(string assetBundleName, UnityWebRequest www)
            : base(assetBundleName)
        {
            if (www == null)
                throw new System.ArgumentNullException("www");
            _url = www.url;
            _www = www;
        }

        protected override bool downloadIsDone { get { return (_www == null) || _www.isDone; } }

        protected override void FinishDownload()
        {
            error = _www.error;
            if (!string.IsNullOrEmpty(error))
                return;
            
            _www.downloadHandler = new DownloadHandlerAssetBundle(_url, 0);
            _www.SendWebRequest();
            while (!_www.isDone) ;
            if (!System.String.IsNullOrEmpty(_www.error))
            {
                return;
            }
            
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(_www);
            if (bundle)
                assetBundle = new LoadedAssetBundle(bundle);
            else
                error = assetBundleName + " is not a valid asset bundle.";
            _www.Dispose();
            _www = null;
        }

        public override string GetSourceURL()
        {
            return _url;
        }
    }

#if UNITY_EDITOR
    public class AssetBundleLoadLevelSimulationOperation : AssetBundleLoadOperation
    {
        AsyncOperation m_Operation;

        public AssetBundleLoadLevelSimulationOperation(string assetBundleName, string levelName, bool isAdditive)
        {
            string[] levelPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, levelName);
            if (levelPaths.Length == 0)
            {
                ///@TODO: The error needs to differentiate that an asset bundle name doesn't exist
                //        from that there right scene does not exist in the asset bundle...

                Debug.LogError("There is no scene with name \"" + levelName + "\" in " + assetBundleName);
                return;
            }

            m_Operation = EditorSceneManager.LoadSceneAsyncInPlayMode(levelPaths[0],
                new LoadSceneParameters(isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single));
        }

        public override bool Update()
        {
            return false;
        }

        public override bool IsDone()
        {
            return m_Operation == null || m_Operation.isDone;
        }
    }
#endif

    public abstract class AssetBundleLoadAssetOperation : AssetBundleLoadOperation
    {
        public abstract T GetAsset<T>() where T: UnityEngine.Object;
    }

    public class AssetBundleLoadAssetOperationSimulation : AssetBundleLoadAssetOperation
    {
        Object                          m_SimulatedObject;

        public AssetBundleLoadAssetOperationSimulation(Object simulatedObject)
        {
            m_SimulatedObject = simulatedObject;
        }

        public override T GetAsset<T>()
        {
            return m_SimulatedObject as T;
        }

        public override bool Update()
        {
            return false;
        }

        public override bool IsDone()
        {
            return true;
        }
    }

    public class AssetBundleLoadAssetOperationFull : AssetBundleLoadAssetOperation
    {
        protected string                m_AssetBundleName;
        protected string                m_AssetName;
        protected string                m_DownloadingError;
        protected System.Type           m_Type;
        protected AssetBundleRequest    m_Request = null;

        public AssetBundleLoadAssetOperationFull(string bundleName, string assetName, System.Type type)
        {
            m_AssetBundleName = bundleName;
            m_AssetName = assetName;
            m_Type = type;
        }

        public override T GetAsset<T>()
        {
            if (m_Request != null && m_Request.isDone)
                return m_Request.asset as T;
            else
                return null;
        }

        // Returns true if more Update calls are required.
        public override bool Update()
        {
            if (m_Request != null)
                return false;

            LoadedAssetBundle bundle = AssetBundleManager.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);
            if (bundle != null)
            {
                ///@TODO: When asset bundle download fails this throws an exception...
                m_Request = bundle.m_AssetBundle.LoadAssetAsync(m_AssetName, m_Type);
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool IsDone()
        {
            // Return if meeting downloading error.
            // m_DownloadingError might come from the dependency downloading.
            if (m_Request == null && m_DownloadingError != null)
            {
                Debug.LogError(m_DownloadingError);
                return true;
            }

            return m_Request != null && m_Request.isDone;
        }
    }

    public class AssetBundleLoadManifestOperation : AssetBundleLoadAssetOperationFull
    {
        public AssetBundleLoadManifestOperation(string bundleName, string assetName, System.Type type)
            : base(bundleName, assetName, type)
        {
        }

        public override bool Update()
        {
            base.Update();

            if (m_Request != null && m_Request.isDone)
            {
                AssetBundleManager.AssetBundleManifestObject = GetAsset<AssetBundleManifest>();
                return false;
            }
            else
                return true;
        }
    }
}
