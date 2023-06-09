using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    Vector3 originalPos;

    private void Awake() {
        Instance = this;
        originalPos = transform.position;
    }
    public IEnumerator Shake (float duration, float magnitude)
    {

        float elapsed = 0.0f;
        StartCoroutine(GameManager.Instance.HitStop(0.05f,duration));
        while ( elapsed < duration){
            float x = Random.Range(-1f,1f) * magnitude ;
            float y = Random.Range(-1f,1f) * magnitude ;
            
            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
    }
}
