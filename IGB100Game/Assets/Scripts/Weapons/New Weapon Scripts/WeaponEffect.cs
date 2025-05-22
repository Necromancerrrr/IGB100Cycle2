using UnityEngine;

public abstract class WeaponEffect : MonoBehaviour
{
    [HideInInspector] public PlayerStats owner;
    [HideInInspector] public Weapon weapon;

    public float GetDamage()
    {
        return weapon.GetDamage();
    }
    public float ScaleUpTransition(float timeTakenU, float scaleMax, float duration)
    {
        float t = timeTakenU / duration;
        transform.localScale = new Vector3(Mathf.Lerp(0, scaleMax, t), Mathf.Lerp(0, scaleMax, t), 1);
        //scaleSpeed += scaleRate;
        timeTakenU += Time.deltaTime;
        return timeTakenU;
    }

    public float ScaleDownTransition(float timeTakenD, float scaleMax, float duration)
    {
        float t = timeTakenD / duration;
        transform.localScale = new Vector3(Mathf.Lerp(scaleMax, 0, t), Mathf.Lerp(scaleMax, 0, t), 1);
        //scaleSpeed += scaleRate;
        timeTakenD += Time.deltaTime;
        return timeTakenD;
    }
}
