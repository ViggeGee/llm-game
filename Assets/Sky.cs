using UnityEngine;

public class Sky : MonoBehaviour
{
    public Light directionalLight; // Reference to the directional light
    public Material skyboxMaterial; // Reference to the skybox material with the cubemap

    [Header("Light Settings")]
    public float maxIntensity = 10f; // Intensity at noon
    public float minIntensity = 0f;  // Intensity at midnight

    [Header("Cubemap Exposure Settings")]
    public float maxExposure = 0.85f; // Exposure at noon
    public float minExposure = 0.25f; // Exposure at midnight

    private void Update()
    {
        // Get the current time of the computer
        System.DateTime currentTime = System.DateTime.Now;

        // Convert the time to a 0-24 range
        float hour = currentTime.Hour;
        float minute = currentTime.Minute;
        float second = currentTime.Second;

        // Normalize the time to a 0-1 range (0 = midnight, 0.5 = noon, 1 = midnight)
        float timeNormalized = (hour + minute / 60f + second / 3600f) / 24f;

        // Map the normalized time to the rotation of the directional light
        float sunAngle = Mathf.Lerp(-90f, 270f, timeNormalized); // -90 = midnight, 0 = sunrise, 90 = noon, 180 = sunset, 270 = midnight

        // Apply the rotation to the directional light
        directionalLight.transform.rotation = Quaternion.Euler(sunAngle, 0f, 0f);

        // Adjust the light intensity based on the sun's angle
        UpdateLightIntensity(sunAngle);

        // Adjust the cubemap exposure based on the sun's angle
        UpdateCubemapExposure(sunAngle);
    }

    private void UpdateLightIntensity(float sunAngle)
    {
        // Calculate intensity based on the sun's angle
        float intensity = Mathf.Clamp01(Mathf.Cos(Mathf.Deg2Rad * (sunAngle - 90f))); // Cos curve for smooth transition
        directionalLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, intensity);
    }

    private void UpdateCubemapExposure(float sunAngle)
    {
        // Calculate exposure based on the sun's angle
        float exposure = Mathf.Clamp01(Mathf.Cos(Mathf.Deg2Rad * (sunAngle - 90f))); // Cos curve for smooth transition
        float currentExposure = Mathf.Lerp(minExposure, maxExposure, exposure);

        // Apply the exposure to the skybox material
        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat("_Exposure", currentExposure);
        }
    }

    private void OnApplicationQuit()
    {
        skyboxMaterial.SetFloat("_Exposure", 0.85f);
    }
}
