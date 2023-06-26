using System;
using Unity.VisualScripting;
using UnityEngine;

public class MainCompositionRoot : MonoBehaviour
{
    [SerializeField] ServiceReferences _serviceReferences;
    [SerializeField] OtherReferences _otherReferences;

    DateTime _startime;

    private void Awake()
    {
        Init();
        _startime = DateTime.Now;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log((DateTime.Now - _startime).TotalMinutes);
            Debug.Log((DateTime.Now - _startime).TotalSeconds);
        }

        if(Input.GetKeyDown(KeyCode.None))
        {
            Debug.Log("Any");
        }
    }

    private void Init()
    {
        _serviceReferences.AdManager.Init();
        _serviceReferences.TimeController.Init();
        _serviceReferences.DoNotDestory.Init();
        _serviceReferences.ShockWaveController.Init();

        // === Other References Init ===
        for (int i = 0; i < _otherReferences.GreenPlayerFirstLvls.Length; i++)
        {
            _otherReferences.GreenPlayerFirstLvls[i].Init(_serviceReferences.AdManager);
        }
        // =============================
    }

    [System.Serializable]
    public sealed class ServiceReferences
    {
        [field: SerializeField] public AdManager AdManager { get; private set; }
        [field: SerializeField] public TimeController TimeController { get; private set; }
        [field: SerializeField] public DoNotDestory DoNotDestory { get; private set; }
        [field: SerializeField] public ShockWaveController ShockWaveController { get; private set; }
    }

    [System.Serializable]
    public sealed class OtherReferences
    {
        [field: SerializeField] public GreenPlayeFirstLvl[] GreenPlayerFirstLvls { get; private set; }
    }
}