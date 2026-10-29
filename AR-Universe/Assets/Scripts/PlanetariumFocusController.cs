using UnityEngine;

public class PlanetariumFocusController : MonoBehaviour
{
    [Header("Camera & Target")]
    [SerializeField] private Transform planetariumCamera; // XR Origin -> Main Camera
    [SerializeField] private float focusDistance = 0.5f;   // Distance in meters from camera
    [SerializeField] private Vector3 displayScale = Vector3.one * 0.1f; // Display scale
    [SerializeField] private float rotationSpeed = 20f;    // Self-spin speed while viewing

    [Header("Solar System Root")]
    [SerializeField] private GameObject solarSystemRoot; // 'solar system2222anim'

    private GameObject displayClone;
    private Animator[] childAnimators;
    private Animation[] childAnimations;

    private void Update()
    {
        // Smoothly rotate the preview planet in place
        if (displayClone != null)
        {
            displayClone.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    public void FocusOnPlanet(GameObject planet)
    {
        if (planet == null) return;

        // Clean up any existing preview
        ExitPlanetarium();

        // 1. Pause the background solar system animations
        PauseSolarSystem(true);

        // 2. Instantiate a preview clone directly as a child of the camera
        if (planetariumCamera != null)
        {
            displayClone = Instantiate(planet, planetariumCamera);
            displayClone.transform.localPosition = new Vector3(0f, 0f, focusDistance);
            displayClone.transform.localRotation = Quaternion.identity;

            if (displayScale != Vector3.zero)
            {
                displayClone.transform.localScale = displayScale;
            }

            // Disable colliders or extra scripts on the clone so it behaves purely as a visual display
            Collider col = displayClone.GetComponent<Collider>();
            if (col != null) col.enabled = false;
        }
    }

    public void ExitPlanetarium()
    {
        // Destroy the visual clone
        if (displayClone != null)
        {
            Destroy(displayClone);
            displayClone = null;
        }

        // Resume solar system orbits
        PauseSolarSystem(false);
    }

    private void PauseSolarSystem(bool pause)
    {
        if (solarSystemRoot == null) return;

        float speed = pause ? 0f : 1f;

        childAnimators = solarSystemRoot.GetComponentsInChildren<Animator>();
        foreach (var anim in childAnimators)
        {
            anim.speed = speed;
        }

        childAnimations = solarSystemRoot.GetComponentsInChildren<Animation>();
        foreach (var anim in childAnimations)
        {
            foreach (AnimationState state in anim)
            {
                state.speed = speed;
            }
        }
    }
}