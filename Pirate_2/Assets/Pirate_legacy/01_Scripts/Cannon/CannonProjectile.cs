using System.Collections;
using UnityEngine;

public class CannonProjectile : MonoBehaviour
{
    float lifeTime = 1.5f; // 대포 수명
    [SerializeField] float additionalGravity = 22f; // 추가 중력 가속도
    public GameObject myParentShip;
    Rigidbody rb;
    int damagePower;
    [SerializeField] TrailRenderer trail;

    public virtual void Fire(Vector3 _dir, float _power, int _damagePower, GameObject _parentShip)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(_dir * _power, ForceMode.VelocityChange);

        damagePower = _damagePower;
        myParentShip = _parentShip;
        
        ResetTrailRenderer();
        trail.enabled = true;

        StartCoroutine(AutoDestroy());
    }

    private void FixedUpdate()
    {
        if (rb != null) rb.AddForce(Vector3.down * additionalGravity, ForceMode.Acceleration);
    }

    IEnumerator AutoDestroy()
    {
        // lifeTime 후 자동으로 삭제
        yield return new WaitForSeconds(lifeTime);
        ResetTrailRenderer();
        trail.enabled = false;

        this.gameObject.SetActive(false);
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ship") && other.gameObject != myParentShip)
        {
            trail.enabled = false;
            ResetTrailRenderer();

            // 데미지
            ShipMover shipMover = other.gameObject.GetComponent<ShipMover>();
            if (shipMover != null)
            {

                shipMover.GetDamage(damagePower);

                GameObject _go = null;
                Vector3 pos = this.transform.position;

                if (ObjectPoolManager.Instance.hit.TryGetNextObject(pos, Quaternion.identity, out _go))
                {
                    float _size = Random.Range(1.3f, 1.8f);
                    _go.transform.localScale = Vector3.one * _size;
                    _go.GetComponent<ParticleSystem>().Play();
                }
                ObjectInActiveSelf();

            }
        }
        if (other.gameObject.CompareTag("Water") )
        {
            trail.enabled = false;
            ResetTrailRenderer();

            GameObject _go = null;
            Vector3 pos = this.transform.position;
            pos.y = 0f;
            if (ObjectPoolManager.Instance.hitWater.TryGetNextObject(pos, Quaternion.identity, out _go))
            {
                _go.transform.eulerAngles = Vector3.zero;
                _go.GetComponent<ParticleSystem>().Play();
            }
            ObjectInActiveSelf();
        }
        if (other.gameObject.CompareTag("Island") )
        {
            trail.enabled = false;
            ResetTrailRenderer();

            GameObject _go = null;
            Vector3 pos = this.transform.position;

            if (ObjectPoolManager.Instance.hit.TryGetNextObject(pos, Quaternion.identity, out _go))
            {
                float _size = Random.Range(1.3f, 1.8f);
                _go.transform.localScale = Vector3.one * _size;
                _go.GetComponent<ParticleSystem>().Play();
            }
            ObjectInActiveSelf();
        }
    }

    void ObjectInActiveSelf()
    {
        trail.enabled = false;
        ResetTrailRenderer();
        this.gameObject.SetActive(false);
    }

    void ResetTrailRenderer()
{
    if (trail != null)
    {
        trail.Clear();
    }
}
}
