using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    public static float radius = 6.5f;
    public static float intensity = 1.5f;
    public Light2D myLight;

    void Start(){
        myLight.pointLightOuterRadius = radius;
        myLight.intensity = intensity;
    }
}
