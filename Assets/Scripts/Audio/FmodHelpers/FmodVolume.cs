using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public static class FmodVolume
{
    public static void SetVca(string vcaPath, float linear01)
    {
        linear01 = Mathf.Clamp01(linear01);
        VCA vca = RuntimeManager.GetVCA(vcaPath);
        vca.setVolume(linear01);

        // example usage (do not uncomment)
        // you would attach the slider values to inspector slider values and run these in start
        // FmodVolume.SetVca("vca:/Master", masterSliderValue);
        // FmodVolume.SetVca("vca:/SFX", sfxSliderValue);
    }
}