using UnityEngine;
using System.Collections;

public class GunRaycast : MonoBehaviour
{
    public float damage = 10f;
    public float range = 500f;
    public Camera playerCamera;
    public Transform firePoint; // Player or gun position

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Get the world point the mouse is pointing at
        Ray camRay = playerCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, firePoint.position); // Adjust if not shooting on Y plane

        float rayDistance;
        if (groundPlane.Raycast(camRay, out rayDistance))
        {
            Vector3 targetPoint = camRay.GetPoint(rayDistance);
            Vector3 direction = (targetPoint - firePoint.position).normalized;

            Ray ray = new Ray(firePoint.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range))
            {
                EnemyStats enemy = hit.transform.GetComponentInParent<EnemyStats>();
                if (enemy != null)
                {
                    Debug.Log("Hit enemy: " + hit.transform.name);
                    enemy.TakeDamage(damage);
                }

                StartCoroutine(ShowImpactRay(ray.origin, hit.point));
            }
            else
            {
                StartCoroutine(ShowImpactRay(ray.origin, ray.origin + direction * range));
            }

            Debug.DrawRay(ray.origin, direction * range, Color.red, 1f);
        }
    }

    IEnumerator ShowImpactRay(Vector3 start, Vector3 end)
    {
        GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
        line.transform.position = (start + end) / 2f;
        line.transform.LookAt(end);
        float length = Vector3.Distance(start, end);
        line.transform.localScale = new Vector3(0.05f, 0.05f, length);

        Destroy(line.GetComponent<Collider>());
        Destroy(line, 0.05f);

        yield return null;
    }
}
