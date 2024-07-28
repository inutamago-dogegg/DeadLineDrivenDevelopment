using ArtC.Damage;
using UnityEngine;

namespace ArtC.Projectile {
    public class Projectile : MonoBehaviour, IAttacker {
        [SerializeField] private Rigidbody2D _projectileRigidbody2D;
        [SerializeField] private ProjectileCurrentParams _currentParams;

        public AttackerTypes Type => AttackerTypes.Projectile;

        private void OnTriggerEnter2D(Collider2D other) {
            //Debug.Log("Projectile OnTriggerEnter2D!");
            var attachedRigidbody = other.attachedRigidbody;

            if (attachedRigidbody == null) {
                //Debug.Log("Projectile OnTriggerEnter2D: attachedRigidbody is null!");
                return;
            }

            var damageApplicable = attachedRigidbody.GetComponent<IDamageApplicable>();

            if (damageApplicable == null) {
                //Debug.Log("Projectile OnTriggerEnter2D: damageApplicable is null!");
                return;
            }

            if (damageApplicable.DamageApplicableType is not DamageApplicableTypes.YourTask) {
                //Debug.Log("Projectile OnTriggerEnter2D: damageApplicable is not YourTask!");
                return;
            }

            //Debug.Log("Projectile Attacked!");

            var damageData = new DamageData(this, _currentParams.DamageValue.Value);
            damageApplicable.ApplyDamage(damageData);

            _currentParams.RemainingPenetrateCount--;

            if (_currentParams.RemainingPenetrateCount <= 0) {
                Destroy(gameObject);
            }
        }

        public void Initialize(ProjectileInitialParams projectileInitialParams) {
            _currentParams.Speed = projectileInitialParams.Speed;
            _currentParams.Size = projectileInitialParams.Size;
            _currentParams.Direction = projectileInitialParams.Direction;
            _currentParams.RemainingPenetrateCount = projectileInitialParams.PenetrateCount;
            _currentParams.DamageValue = projectileInitialParams.DamageValue;

            _projectileRigidbody2D.velocity = projectileInitialParams.Direction * projectileInitialParams.Speed;
            transform.localScale = new Vector3(projectileInitialParams.Size, projectileInitialParams.Size, 1f);

            Destroy(gameObject, projectileInitialParams.LifeTimeSeconds);
        }
    }
}
