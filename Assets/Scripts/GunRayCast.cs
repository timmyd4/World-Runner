using UnityEngine;
using System.Collections;

public class GunRaycast : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
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
        // Get direction from firePoint toward mouse click
        Ray camRay = playerCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 origin = firePoint.position;
        Vector3 direction = (camRay.GetPoint(1f) - origin).normalized;

        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            EnemyStats enemy = hit.transform.GetComponentInParent<EnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            StartCoroutine(ShowImpactRay(origin, hit.point));
        }
        else
        {
            StartCoroutine(ShowImpactRay(origin, origin + direction * range));
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
