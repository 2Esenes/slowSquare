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
        _serviceReferences.TimeController.Init();
        _serviceReferences.DoNotDestory.Init();
    }

    [System.Serializable]
    public sealed class ServiceReferences
    {
        [field: SerializeField] public AdManager AdManager { get; private set; }
        [field: SerializeField] public TimeController TimeController { get; private set; }
        [field: SerializeField] public DoNotDestory DoNotDestory { get; private set; }
    }
}