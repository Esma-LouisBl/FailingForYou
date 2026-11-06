using FishNet.Serializing.Helping;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AmbianceManager : MonoBehaviour
{
    public LightManager ambiance;

    public float changeTps = 5f;

    void Start()
    {
        StartCoroutine(Changeambiance());
    }

    IEnumerator Changeambiance()
    {
        while (true)
        {
            ambiance.MorningSet();
            yield return new WaitForSeconds(changeTps);
            ambiance.AfternoonSet();
            yield return new WaitForSeconds(changeTps);
            ambiance.NightSet();
            yield return new WaitForSeconds(changeTps);
            ambiance.MorningRomSet();
            yield return new WaitForSeconds(changeTps);
            ambiance.AfternoonRomSet();
            yield return new WaitForSeconds(changeTps);
            ambiance.NightRomSet();
            yield return new WaitForSeconds(changeTps);
            ambiance.MorningHorSet();
            yield return new WaitForSeconds(changeTps);
        }
        
    }
}
