using UnityEngine;
using GlobalEnums;

public class Projectile : MonoBehaviour, IPoolable
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public Material Material
    {
        get => _spriteRenderer.material;
        set => _spriteRenderer.material = value;
    }

    public PoolingManager PoolingManagerSO { get; set; }
    public ThemeColor ProjectileColor { get; set; }
    public float Damage { set; get; }
}
