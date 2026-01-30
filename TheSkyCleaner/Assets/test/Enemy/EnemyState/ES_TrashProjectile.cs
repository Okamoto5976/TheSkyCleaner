using UnityEngine;

/// <summary>
/// シンプルな直線弾丸。寿命で自動返却 or 破棄。
/// Rigidbody を使わない場合のフォールバックとして利用。
/// </summary>
public class ES_TrashProjectile : MonoBehaviour
{
    private Vector3 _velocity;
    private ObjectPoolManager _pool;
    private float _life;
    private float _timer;
    private bool _active;

    public void Init(Vector3 velocity, ObjectPoolManager pool, float life)
    {
        _velocity = velocity;
        _pool = pool;
        _life = life;
        _timer = 0f;
        _active = true;
        enabled = true;
    }

    private void Update()
    {
        if (!_active) return;

        transform.position += _velocity * Time.deltaTime;
        _timer += Time.deltaTime;

        if (_timer >= _life)
        {
            Dispose();
        }
    }

    private void OnDisable()
    {
        _active = false;
    }

    private void Dispose()
    {
        _active = false;
        _pool.ReturnToPool(gameObject);
    }
}
