using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeAwayAndDestroyed : MonoBehaviour
{
    [SerializeField] private float fadePerTime;
    [SerializeField] private float waitBeforeFade;
    private bool startFade = false;

    private Text actionContent {
        get {
            return GetComponent<Text>();
        }
    }

    private void Awake()
    {
        StartCoroutine(WaitCoroutine());
    }

    private void Update()
    {
        if (startFade) {
            if (actionContent.color.a > fadePerTime * Time.deltaTime)
            {
                actionContent.color = new Color(actionContent.color.r, actionContent.color.g, actionContent.color.b, actionContent.color.a - fadePerTime * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator WaitCoroutine() {
        yield return new WaitForSeconds(waitBeforeFade);
        startFade = true;
    }
}
