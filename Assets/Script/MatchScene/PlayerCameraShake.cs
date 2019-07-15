using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraShake : MonoBehaviour
{
    public IEnumerator Shake(float ShakeTime, float ShakeScale)
    {
        Vector3 originPos = Camera.main.transform.localPosition;

        float curTime = 0.0f;

        while (curTime < ShakeTime)
        {
            originPos = new Vector3(transform.localPosition.x, originPos.y, originPos.z);

            float x = Camera.main.transform.localPosition.x + Random.Range(-1f, 1f) * ShakeScale;
            float y = Camera.main.transform.localPosition.z + Random.Range(-1f, 1f) * ShakeScale;

            Camera.main.transform.localPosition = new Vector3(x, originPos.y, y);

            curTime += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.localPosition = originPos;
    }
}
