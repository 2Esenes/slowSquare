using UnityEngine;

public sealed class Level : MonoBehaviour
{
    [SerializeField] EnemyBasicController[] _enemies;
    [SerializeField] LevelController _levelController;

    int _enemyCount;

    private void Awake()
    {
        _enemies = GetComponentsInChildren<EnemyBasicController>();
        _enemyCount = _enemies.Length;

        for (int i = 0; i < _enemyCount; i++)
            _enemies[i].RegisterOnDie(OnEnemyDie);
    }

    private void OnEnemyDie(EnemyBasicController ebc)
    {
        ebc.UnRegisterOnDie(OnEnemyDie);
        _enemyCount--;

        if (_enemyCount <= 0)
            _levelController.OpenSkillCards();
    }
}
