using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimDriver : MonoBehaviour
{
    [Header("Animator")]
    public int fireLayerIndex = 1;
    public float crossFade = 0.03f;

    [Header("State names (Fire layer)")]
    public string windUpState = "WindUp";
    public string shootState = "Shoot";
    public string windDownState = "WindDown";
    public string emptyState = "Empty";

    [Header("Shoot Clip (for timing)")]
    public AnimationClip shootClip;

    // Frame timing you provided:
    const float muzzleFrame = 19f;
    const float totalFrames = 37f;
    const float muzzleNorm = muzzleFrame / totalFrames; // ~0.5135135

    public float FireDelaySeconds =>
        shootClip ? shootClip.length * muzzleNorm : 0f;

    Animator anim;
    int windUpHash, shootHash, windDownHash, emptyHash;

    void Awake()
    {
        anim = GetComponent<Animator>();

        string layerName = anim.GetLayerName(fireLayerIndex);

        windUpHash   = Animator.StringToHash($"{layerName}.{windUpState}");
        shootHash    = Animator.StringToHash($"{layerName}.{shootState}");
        windDownHash = Animator.StringToHash($"{layerName}.{windDownState}");
        emptyHash    = Animator.StringToHash($"{layerName}.{emptyState}");
    }

    public void PlayWindUp()   => anim.CrossFadeInFixedTime(windUpHash, crossFade, fireLayerIndex);
    public void PlayShoot()    => anim.CrossFadeInFixedTime(shootHash, crossFade, fireLayerIndex);
    public void PlayWindDown() => anim.CrossFadeInFixedTime(windDownHash, crossFade, fireLayerIndex);
    public void PlayEmpty()    => anim.CrossFadeInFixedTime(emptyHash, crossFade, fireLayerIndex);
}
