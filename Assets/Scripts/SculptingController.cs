using UnityEngine;

public class SculptingController : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public GameObject clayCubePrefab;

    [Header("Placement")]
    public LayerMask sculptLayer;      // what we can place onto (e.g. RawBlock)
    public float offsetDistance = 0.125f;
    public float maxDistance = 10f;    // how far you can sculpt

    void Update()
    {
        // Left click = place
        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceCube();
        }

        // Right click = remove
        if (Input.GetMouseButtonDown(1))
        {
            TryRemoveCube();
        }
    }

    void TryPlaceCube()
    {
        if (mainCamera == null || clayCubePrefab == null)
            return;

        // Ray straight out of the camera center
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        // Only hit things on the sculpt layer (e.g. RawBlock)
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, sculptLayer))
        {
            // Position cube just outside the surface along the normal
            Vector3 spawnPos = hitInfo.point + hitInfo.normal * offsetDistance;

            // Optional: snap to a grid for voxel-y feel
            float snap = 0.25f; // size of the ClayCube
            spawnPos.x = Mathf.Round(spawnPos.x / snap) * snap;
            spawnPos.y = Mathf.Round(spawnPos.y / snap) * snap;
            spawnPos.z = Mathf.Round(spawnPos.z / snap) * snap;

            Instantiate(clayCubePrefab, spawnPos, Quaternion.identity);
        }
    }

    void TryRemoveCube()
    {
        if (mainCamera == null)
            return;

        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        // No layer mask here; we want to be able to hit ClayCubes directly
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance))
        {
            // If we hit a clay cube, delete it
            if (hitInfo.collider.CompareTag("ClayCube"))
            {
                Destroy(hitInfo.collider.gameObject);
            }
        }
    }
}
