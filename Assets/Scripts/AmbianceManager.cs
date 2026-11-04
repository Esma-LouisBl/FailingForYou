using FishNet.Serializing.Helping;
using UnityEngine;
using System.Collections;

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
        ambiance.MorningSet();
        yield return new WaitForSeconds(changeTps);
        ambiance.AfternoonSet();
        yield return new WaitForSeconds(changeTps);
        ambiance.NightSet();
        yield return new WaitForSeconds(changeTps);
        ambiance.MorningRomSet();
    }
}
