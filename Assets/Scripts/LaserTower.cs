using UnityEngine;
public class LaserTower : Tower
{
    [SerializeField, Range(1f, 100f)]
    float damagePerSecond = 10f;

    [SerializeField]
    private Transform turret = default, laserBeam = default;
    private TargetPoint target;
    Vector3 laserBeamScale;
    
    public override TowerType TowerType => TowerType.Laser;
    public static int Power => 1;

    void Awake () {
        laserBeamScale = laserBeam.localScale;
    }
    
    public override void GameUpdate () {
        if (TrackTarget(ref target) || AcquireTarget(out target))
        {
            Shoot();
        }
        else {
            laserBeam.localScale = Vector3.zero;
        }
    }
    
    void Shoot () {
        Vector3 point = target.Position;
        turret.LookAt(point);
        laserBeam.localRotation = turret.localRotation;
        
        float d = Vector3.Distance(turret.position, point);
        laserBeamScale.z = d;
        laserBeam.localScale = laserBeamScale;
        
        laserBeam.localPosition =
            turret.localPosition + 0.5f * d * laserBeam.forward;
        
        target.Enemy.ApplyDamage(damagePerSecond * Time.deltaTime);
    }

    public override bool CanBuild(Player player)
    {
        return player.power >= Power;
    }

    public override void Build(Player player)
    {
        player.power -= Power;
    }

    public override void Trash(Player player)
    {
        player.power += Power;
    }
}
