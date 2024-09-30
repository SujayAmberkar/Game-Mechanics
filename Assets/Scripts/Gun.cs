using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float fireRate = 0.1f; // Time between each shot
    private float nextFireTime = 0f; // Time until next shot is allowed
    private int shotCount = 0; // Counter for the number of shots fired

    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && Time.time >= nextFireTime && Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, 1000f))
        {
            nextFireTime = Time.time + fireRate; // Update next fire time
            // show raycast 
            Debug.DrawLine(muzzle.position, hit.point, Color.red);
            Debug.Log(hit.point);
            Recoil();
        }
        // Reset shot count when appropriate, e.g., when not firing
        if (!Input.GetMouseButton(0))
        {
            shotCount = 0; // Reset shot count when mouse button is released
        }
    }

    // gun procedural recoil
    private void Recoil()
    {
        float randomRecoil;
        float randomPitch;
        float randomYaw;

        if (shotCount < 2)
        {
            // Consistent recoil for the first two shots
            randomRecoil = 0.15f; // Fixed recoil displacement
            randomPitch = 0.5f;   // Fixed pitch rotation
            randomYaw = 0.5f;     // Fixed yaw rotation
        }
        else
        {
            // Random recoil for subsequent shots
            randomRecoil = Random.Range(0.1f, 0.2f);
            randomPitch = Random.Range(-1f, 1f);
            randomYaw = Random.Range(-1f, 1f);
        }

        // Apply recoil displacement and rotation
        transform.localPosition -= new Vector3(0, 0, randomRecoil);
        transform.localEulerAngles += new Vector3(randomPitch, randomYaw, 0);

        // Smoothly return to original position and rotation
        StartCoroutine(ReturnToOriginalPositionSmooth());

        // Increment shot count
        shotCount++;
    }

    private IEnumerator ReturnToOriginalPositionSmooth()
    {
        float duration = 0.2f; // Duration over which to return to original position
        float elapsed = 0;
        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.localRotation;
        while (elapsed < duration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, originalPosition, elapsed / duration);
            transform.localRotation = Quaternion.Lerp(startRotation, originalRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
    }
}
