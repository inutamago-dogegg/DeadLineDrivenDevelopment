using R3;
using UnityEngine;
using UnityEngine.UI;

namespace ArtC.GUI {
    public class YourTaskHitPointDisplay : MonoBehaviour {
        [SerializeField] private YourTask.YourTask _yourTask;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private GameObject _hpSliderParent;

        [Header("パラメーター")] [SerializeField] private float _displaySeconds = 3f;

        private float _remainingDisplaySeconds = 0f;

        private void Start() {
            _hpSliderParent.SetActive(false);
            _hpSlider.maxValue = _yourTask.MaxHitPoint;
            _hpSlider.value = _yourTask.MaxHitPoint;

            _yourTask.CurrentHitPoint
                .Subscribe(hp => {
                    if (Mathf.Approximately(hp, _yourTask.MaxHitPoint)) {
                        _hpSliderParent.SetActive(false);
                        return;
                    }

                    _hpSlider.value = hp;
                    _remainingDisplaySeconds = _displaySeconds;
                    _hpSliderParent.SetActive(true);
                }).AddTo(this);
        }

        private void FixedUpdate() {
            _remainingDisplaySeconds -= Time.fixedDeltaTime;

            if (_remainingDisplaySeconds <= 0f) {
                _hpSliderParent.SetActive(false);
                return;
            }
        }
    }
}
