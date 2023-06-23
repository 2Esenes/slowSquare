using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCompositionRoot : MonoBehaviour
{
    [SerializeField] ServiceReferences _serviceReferences;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _serviceReferences.AdManager.Init();
    }

    [System.Serializable]
    public sealed class ServiceReferences
    {
        [field: SerializeField] public AdManager AdManager { get; private set; }
    }
}