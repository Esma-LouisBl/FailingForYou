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
    public Color spotColorAfternoon;
 

    [Header("Night")]
    public Vector3 directionDirectionalNight;
    public Color atticNightColorLight;
    public Color keyNightColorLight;

    public float atticNightIntensity;
    public float keyNightIntensity;

    public float spotNightIntensity;
    public Color spotColorNight;


    [Header("Romantic")]
    [Header("Romantic Morning")]
    public Color atticMorningRomantic;
    public Color keyMorningRomantic;

    public float atticMorningRomanticIntensity;
    public float keyMorningRomanticIntensity;

    public float spotRomMorningIntensity;
    public Color spotRomMorningColor;

    [Header("Romantic Afternoon")]
    public Color atticAfternoonRomantic;
    public Color keyAfternoonRomantic;

    public float atticAfternoonRomanticIntensity;
    public float keyAfternoonRomanticIntensity;

    public float spotRomAfternoonIntensity;
    public Color spotRomAfternoonColor;

    [Header("Romantic Night")]
    public Color atticNightRomantic;
    public Color keyNightRomantic;

    public float atticNightRomanticIntensity;
    public float keyNightRomanticIntensity;

    public float spotRomNightIntensity;
    public Color spotRomNightColor;

    [Header("Horrific")]
    [Header("Horrific Morning")]
    public Color atticMorningHorrific;
    public Color keyMorningHorrific;

    public float atticMorningHorrificIntensity;
    public float keyMorningHorrificIntensity;

    public float spotHorMorningIntensity;
    public Color spotHorMorningColor;

    [Header("Horrific Afternoon")]
    public Color atticAfternoonHorrific;
    public Color keyAfternoonHorrific;

    public float atticAfternoonHorrificIntensity;
    public float keyAfternoonHorrificIntensity;

    public float spotHorAfternoonIntensity;
    public Color spotHorAfternoonColor;

    [Header("Horrific Night")]
    public Color atticNightHorrific;
    public Color keyNightHorrific;

    public float atticNightHorrificIntensity;
    public float keyNightHorrificIntensity;

    public float spotHorNightIntensity;
    public Color spotHorNightColor;

    [Header("Dramatic")]
    [Header("Dramatic Morning")]
    public Color atticMorningDramatic;
    public Color keyMorningDramatic;

    public float atticMorningDramaticIntensity;
    public float keyMorningDramaticIntensity;

    public float spotDraMorningIntensity;
    public Color spotDraMorningColor;

    [Header("Dramatic Afternoon")]
    public Color atticAfternoonDramatic;
    public Color keyAfternoonDramatic;

    public float atticAfternoonDramaticIntensity;
    public float keyAfternoonDramaticIntensity;

    public float spotDraAfternoonIntensity;
    public Color spotDraAfternoonColor;

    [Header("Dramatic Night")]
    public Color atticNightDramatic;
    public Color keyNightDramatic;

    public float atticNightDramaticIntensity;
    public float keyNightDramaticIntensity;

    public float spotDraNightIntensity;
    public Color spotDraNightColor;

    [Header("Fantastic")]
    [Header("Fantastic Morning")]
    public Color atticMorningFantastic;
    public Color keyMorningFantastic;

    public float atticMorningFantasticIntensity;
    public float keyMorningFantasticintensity;

    public float spotFanMorningIntensity;
    public Color spotFanMorningColor;

    [Header("Fantastic Afternoon")]
    public Color atticAfternoonFantastic;
    public Color keyAfternoonFantastic;

    public float atticAfternoonFantasticIntensity;
    public float keyAfternoonFantasticIntensity;

    public float spotFanAfternoonIntensity;
    public Color spotFanAfternoonColor;

    [Header("Fantastic Night")]
    public Color atticNightFantastic;
    public Color keyNightFantastic;

    public float atticNightFantasticIntensity;
    public float keyNightFantasticIntensity;

    public float spotFanNightIntensity;
    public Color spotFanNightColor;

    [Header("Historic")]
    [Header("Historic Morning")]
    public Color atticMorningHistoric;
    public Color keyMorningHistoric;
    [Header("Historic Afternoon")]
    public Color atticAfternoonHistoric;
    public Color keyAfternoonHistoric;
    [Header("Historic Night")]
    public Color atticNightHistoric;
    public Color keyNightHistoric;
    public void MorningSet()
    {
        atticDirectionalLight.color = atticMorningColorLight;
        keyDirectionalLight.color = keyMorningColorLight;

        atticDirectionalLight.intensity = atticMorningIntensity;
        keyDirectionalLight.intensity = keyMorningIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalMorning;

        materialShader.SetColor("_ColorSpot", spotColorMorning);
        materialShader.SetFloat("_Alpha", spotMorningIntensity);
    }
    public void AfternoonSet()
    {
        atticDirectionalLight.color = atticAfternoonColorLight;
        keyDirectionalLight.color = keyAfternoonColorLight;

        atticDirectionalLight.intensity = atticAfternoonIntensity;
        keyDirectionalLight.intensity = keyAfternoonIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalAfternoon;

        materialShader.SetColor("_ColorSpot", spotColorAfternoon);
        materialShader.SetFloat("_Alpha", spotAfternoonIntensity);
    }
    public void NightSet()
    {
        atticDirectionalLight.color = atticNightColorLight;
        keyDirectionalLight.color = keyNightColorLight;

        atticDirectionalLight.intensity = atticNightIntensity;
        keyDirectionalLight.intensity = keyNightIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalNight;

        materialShader.SetColor("_ColorSpot", spotColorNight);
        materialShader.SetFloat("_Alpha", spotNightIntensity);
    }
    public void MorningRomSet()
    {
        atticDirectionalLight.color = atticMorningRomantic;
        keyDirectionalLight.color = keyMorningRomantic;

        atticDirectionalLight.intensity = atticMorningRomanticIntensity;
        keyDirectionalLight.intensity = keyMorningRomanticIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalMorning;

        materialShader.SetColor("_ColorSpot", spotRomMorningColor);
        materialShader.SetFloat("_Alpha", spotRomMorningIntensity);
    }
    public void AfternoonRomSet()
    {
        atticDirectionalLight.color = atticAfternoonRomantic;
        keyDirectionalLight.color = keyAfternoonRomantic;

        atticDirectionalLight.intensity = atticAfternoonRomanticIntensity;
        keyDirectionalLight.intensity = keyAfternoonRomanticIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalAfternoon;

        materialShader.SetColor("_ColorSpot", spotRomAfternoonColor);
        materialShader.SetFloat("_Alpha", spotRomAfternoonIntensity);
    }
    public void NightRomSet()
    {
        atticDirectionalLight.color = atticNightRomantic;
        keyDirectionalLight.color = keyNightRomantic;

        atticDirectionalLight.intensity = atticNightRomanticIntensity;
        keyDirectionalLight.intensity = keyNightRomanticIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalNight;

        materialShader.SetColor("_ColorSpot", spotRomNightColor);
        materialShader.SetFloat("_Alpha", spotRomNightIntensity);

    }
    public void MorningHorSet()
    {
        atticDirectionalLight.color = atticMorningHorrific;
        keyDirectionalLight.color = keyMorningHorrific;

        atticDirectionalLight.intensity = atticMorningHorrificIntensity;
        keyDirectionalLight.intensity = keyMorningHorrificIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalMorning;

        materialShader.SetColor("_ColorSpot", spotHorMorningColor);
        materialShader.SetFloat("_Alpha", spotHorMorningIntensity);
    }
    public void AfternoonHorSet()
    {
        atticDirectionalLight.color = atticAfternoonHorrific;
        keyDirectionalLight.color = keyAfternoonHorrific;

        atticDirectionalLight.intensity = atticAfternoonHorrificIntensity;
        keyDirectionalLight.intensity = keyAfternoonHorrificIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalAfternoon;

        materialShader.SetColor("_ColorSpot", spotHorAfternoonColor);
        materialShader.SetFloat("_Alpha", spotHorAfternoonIntensity);
    }
    public void NightHorSet()
    {
        atticDirectionalLight.color = atticNightHorrific;
        keyDirectionalLight.color = keyNightHorrific;

        atticDirectionalLight.intensity = atticNightHorrificIntensity;
        keyDirectionalLight.intensity = keyNightHorrificIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalNight;

        materialShader.SetColor("_ColorSpot", spotHorNightColor);
        materialShader.SetFloat("_Alpha", spotHorNightIntensity);
    }
    public void MorningDraSet()
    {
        atticDirectionalLight.color = atticMorningDramatic;
        keyDirectionalLight.color = keyMorningDramatic;

        atticDirectionalLight.intensity = atticMorningDramaticIntensity;
        keyDirectionalLight.intensity = keyMorningDramaticIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalMorning;

        materialShader.SetColor("_ColorSpot", spotDraMorningColor);
        materialShader.SetFloat("_Alpha", spotDraMorningIntensity);
    }
    public void AfternoonDraSet()
    {
        atticDirectionalLight.color = atticAfternoonDramatic;
        keyDirectionalLight.color = keyAfternoonDramatic;

        atticDirectionalLight.intensity = atticAfternoonDramaticIntensity;
        keyDirectionalLight.intensity = keyAfternoonDramaticIntensity; 

        atticDirectionalLight.transform.eulerAngles = directionDirectionalAfternoon;

        materialShader.SetColor("_ColorSpot", spotDraAfternoonColor);
        materialShader.SetFloat("_Alpha", spotDraAfternoonIntensity);
    }
    public void NightDraSet()
    {
        atticDirectionalLight.color = atticNightDramatic;
        keyDirectionalLight.color = keyNightDramatic;

        atticDirectionalLight.intensity = atticNightDramaticIntensity;
        keyDirectionalLight.intensity = keyNightDramaticIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalNight;

        materialShader.SetColor("_ColorSpot", spotDraNightColor);
        materialShader.SetFloat("_Alpha", spotDraNightIntensity);
    }
    public void MorningFanSet()
    {
        atticDirectionalLight.color = atticMorningFantastic;
        keyDirectionalLight.color = keyMorningFantastic;

        atticDirectionalLight.intensity = atticMorningDramaticIntensity;
        keyDirectionalLight.intensity = keyMorningDramaticIntensity;

        atticDirectionalLight.transform.eulerAngles = directionDirectionalMorning;

        materialShader.SetColor("_ColorSpot", spotFanMorningColor);
        materialShader.SetFloat("_Alpha", spotFanMorningIntensity);
    }
    public void AfternoonFanSet()
    {
        atticDirectionalLight.color = atticAfternoonFantastic;

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
