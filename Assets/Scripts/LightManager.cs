using UnityEngine;
using UnityEngine.Rendering;

public class LightManager : MonoBehaviour
{
    [Header("Lights")]
    public Light atticDirectionalLight;
    public Light keyDirectionalLight;
    public Material materialShader;
    public Light spotReveal;
    public Light spotTV;
 

    [Header("Morning")]
    public Vector3 directionDirectionalMorning;
    public Color atticMorningColorLight;
    public Color keyMorningColorLight;

    public float atticMorningIntensity;
    public float keyMorningIntensity;
    public float spotMorningIntensity;

    public Color spotColorMorning;
 


    [Header("Afternoon")]
    public Vector3 directionDirectionalAfternoon;
    public Color atticAfternoonColorLight;
    public Color keyAfternoonColorLight;

    public float atticAfternoonIntensity;
    public float keyAfternoonIntensity;
    public float spotAfternoonIntensity;
 

    [Header("Night")]
    public Vector3 directionDirectionalNight;
    public Color atticNightColorLight;
    public Color keyNightColorLight;

    public float atticNightIntensity;
    public float keyNightIntensity;


    [Header("Romantic")]
    [Header("Romantic Morning")]
    public Color atticMorningRomantic;
    public Color keyMorningRomantic;

    public float atticMorningRomanticIntensity;
    public float keyMorningRomanticIntensity;

    [Header("Romantic Afternoon")]
    public Color atticAfternoonRomantic;
    [Header("Romantic Night")]
    public Color atticNightRomantic;

    [Header("Horrific")]
    [Header("Horrific Morning")]
    public Color atticMorningHorrific;
    [Header("Horrific Afternoon")]
    public Color atticAfternoonHorrific;
    [Header("Horrific Night")]
    public Color atticNightHorrific;

    [Header("Dramatic")]
    [Header("Horrific Morning")]
    [Header("Horrific Afternoon")]
    [Header("Horrific Night")]

    [Header("Fantastic")]
    [Header("Fantastic Morning")]
    [Header("Fantastic Afternoon")]
    [Header("Fantastic Night")]

    [Header("Historic")]
    [Header("Historic Morning")]
    [Header("Historic Afternoon")]
    [Header("Historic Night")]
    public Color atticNightHistoric;
    public void MorningSet()
    {
        atticDirectionalLight.color = atticMorningColorLight;
        keyDirectionalLight.color = keyMorningColorLight;

        atticDirectionalLight.intensity = atticMorningIntensity;
        keyDirectionalLight.intensity = keyMorningIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalMorning;

        materialShader.SetColor("_ColorSpot", spotColorMorning);
    }
    public void AfternoonSet()
    {
        atticDirectionalLight.color = atticAfternoonColorLight;
        keyDirectionalLight.color = keyAfternoonColorLight;

        atticDirectionalLight.intensity = atticAfternoonIntensity;
        keyDirectionalLight.intensity = keyAfternoonIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalAfternoon;
    }
    public void NightSet()
    {
        atticDirectionalLight.color = atticNightColorLight;
        keyDirectionalLight.color = keyNightColorLight;

        atticDirectionalLight.intensity = atticNightIntensity;
        keyDirectionalLight.intensity = keyNightIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalNight;
    }
    public void MorningRomSet()
    {
        atticDirectionalLight.color = atticMorningRomantic;
        keyDirectionalLight.color = keyMorningRomantic;

        atticDirectionalLight.intensity = atticMorningRomanticIntensity;
        keyDirectionalLight.intensity = keyMorningRomanticIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalMorning;
    }
    public void AfternoonRomSet()
    {
        atticDirectionalLight.transform.eulerAngles = directionDirectionalAfternoon;
    }
    public void NightRomSet()
    {
        atticDirectionalLight.transform.eulerAngles = directionDirectionalNight;
    }
    public void MorningHorSet()
    {
        atticDirectionalLight.transform.eulerAngles = directionDirectionalMorning;
    }
    public void AfternoonHorSet()
    {
        atticDirectionalLight.transform.eulerAngles = directionDirectionalAfternoon;
    }
    public void NightHorSet()
    {
        atticDirectionalLight.transform.eulerAngles = directionDirectionalNight;
    }
    public void MorningDraSet()
    {
        atticDirectionalLight.transform.eulerAngles = directionDirectionalMorning;
    }
    public void AfternoonDraSet()
    {
        atticDirectionalLight.transform.eulerAngles = directionDirectionalAfternoon;
    }
    public void NightDraSet()
    {
        atticDirectionalLight.transform.eulerAngles = directionDirectionalNight;
    }
    public void MorningFanSet()
    {
        atticDirectionalLight.transform.eulerAngles = directionDirectionalMorning;
    }
    public void AfternoonFanSet()
    {
        atticDirectionalLight.transform.eulerAngles = directionDirectionalAfternoon;
    }
    public void NightFanSet()
    {
        atticDirectionalLight.transform.eulerAngles = directionDirectionalNight;
    }
    public void MorningHisSet()
    {
        atticDirectionalLight.transform.eulerAngles = directionDirectionalMorning;
    }
    public void AfternoonHisSet()
    {
        atticDirectionalLight.transform.eulerAngles = directionDirectionalAfternoon;
    }
    public void NihgtHisSet()
    {
        atticDirectionalLight.transform.eulerAngles = directionDirectionalNight;
    }
}
