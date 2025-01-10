using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] ParticleSystem speedUpParticle;
    [SerializeField] float minFOV = 20f;
    [SerializeField] float maxFOV = 120f;
    [SerializeField] float zoomDuration = 1f;
    [SerializeField] private float zoomSpeedModifer = 5f;
    
    CinemachineCamera cinemachineCamera;

    void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }
    
    public void ChangeCameraFOV(float speedAmount)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeFOVRoutine(speedAmount));
        if (speedAmount > 0)
        {
            speedUpParticle.Play();
        }
    }

    IEnumerator ChangeFOVRoutine(float speedAmount)
    {
        float startFOV = cinemachineCamera.Lens.FieldOfView;
        float targetFOV = Mathf.Clamp(startFOV + speedAmount * zoomSpeedModifer, minFOV, maxFOV);
        float elapsedTime = 0f;

        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime / zoomDuration);
            yield return null;
        }
        
        cinemachineCamera.Lens.FieldOfView = targetFOV;
    }
}