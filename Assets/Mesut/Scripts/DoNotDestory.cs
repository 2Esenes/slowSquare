using UnityEngine;

public sealed class DoNotDestory : MonoBehaviour
{
    private static DoNotDestory _instance;

    [SerializeField] GameObject[] _doNotDestroyObjects;

    bool _initialized;

    public void Init()
    {
        if (_instance != null)
        {
            for (int i = 0; i < _doNotDestroyObjects.Length; i++)
                Destroy(_doNotDestroyObjects[i]);
            Destroy(gameObject);
            return;
        }

        _instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        _initialized = true;

        for(int i = 0; i < _doNotDestroyObjects.Length; i++)
        {
            _doNotDestroyObjects[i].transform.SetParent(null);
            DontDestroyOnLoad(_doNotDestroyObjects[i]);
        }
    }
}
