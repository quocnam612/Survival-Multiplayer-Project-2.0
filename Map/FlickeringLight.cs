using System.Collections;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] private Light lightSource;
    [SerializeField] private float min, max, intensityMultiplier, flickerInterval;

    private void OnEnable()
    {
        StartCoroutine(flicker(flickerInterval));
    }

    private void Update()
    {
        
    }

    private IEnumerator flicker(float interval) {
        while (interval > 0) {
            yield return new WaitForSeconds(interval);
            lightSource.intensity = Random.Range(min, max) * intensityMultiplier;
        }
        while (interval < 0) {
            yield return null;
            lightSource.intensity = Random.Range(min, max) * intensityMultiplier;
        }
    }
}
