using UnityEditor.Animations;
using UnityEngine;

public abstract class Weapon : Item
{
    [System.Serializable]
    public struct Stats
    {
        public string name, description;

        [Header("Visuals")]
        public ParticleSystem hitEffect;
        public Projectile projectilePrefab;
        public GameObject prefab;
        // public Aura auraPrefab;
        public Rect spawnVariance;

        [Header("Values")]
        public float lifespan; // If 0, will last forever
        
        public float damage, damageVariance, area, speed, cooldown, projectileInterval, knockback;
        public int number, piercing, maxInstances;
        
        public float windDownTimer;
        public float timeTakenUp;
        public float timeTakenDown;

        public static Stats operator +(Stats stats1, Stats stats2)
        {
            Stats result = new Stats();

            result.name = stats2.name ?? stats1.name;
            result.description = stats2.description ?? stats1.description;

            //result.projectilePrefab = stats2.projectilePrefab ?? stats1.projectilePrefab;
            //result.auraPrefab = stats2.auraPrefab ?? stats1.auraPrefab;

            result.hitEffect = stats2.hitEffect ?? stats1.hitEffect;
            result.spawnVariance = stats2.spawnVariance;
            result.lifespan = stats2.lifespan + stats1.lifespan;
            result.damage = stats2.damage + stats1.damage;
            result.damageVariance = stats2.damageVariance + stats1.damageVariance;
            result.area = stats2.area + stats1.area;
            result.speed = stats2.speed + stats1.speed;
            result.cooldown = stats2.cooldown + stats1.cooldown;
            result.number = stats2.number + stats1.number;
            result.piercing =  stats2.piercing + stats1.piercing;
            result.projectileInterval = stats2.projectileInterval + stats1.projectileInterval;
            result.knockback = stats2.knockback + stats1.knockback;
            result.timeTakenUp = 0f;
            result.timeTakenDown = 0f;

            return result;
        }

        public float GetDamage()
        {
            return damage + Random.Range(0, damageVariance);
        }
    }

    protected Stats currentStats;

    public WeaponData data;

    protected float currentCooldown;

    protected PlayerMovement movement;

    public virtual void Initialize(WeaponData data)
    {
        base.Intialise(data);
        this.data = data;
        currentStats = data.baseStats;
        movement = GetComponentInParent<PlayerMovement>();
        currentCooldown = currentStats.cooldown;
    }

    protected virtual void Awake()
    {
        if(data) currentStats = data.baseStats;
    }

    protected virtual void Start()
    {
        if (data)
        {
            Initialize(data);
        }
    }

    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0f)
        {
            Attack(currentStats.number);
        }
    }


    public override bool DoLevelUp()
    {
        base.DoLevelUp();
        if (!CanLevelUp())
        {
            Debug.LogWarning(string.Format("Cannot level up {0} to Level {1}, max level of {2} already reached.", name, currentLevel, data.maxLevel));
            return false;
        }

        currentStats += data.GetLevelData(++currentLevel);
        return true;
    }

    public virtual bool CanAttack()
    {
        return currentCooldown <= 0;
    }

    protected virtual bool Attack(int attackCount = 1)
    {
        if (CanAttack())
        {
            currentCooldown += currentStats.cooldown;
            return true;
        }
        return false;
    }

    public virtual float GetDamage()
    {
        return currentStats.GetDamage() * owner.CurrentMight;
    }

    public virtual Stats GetStats() { return currentStats; }


}
