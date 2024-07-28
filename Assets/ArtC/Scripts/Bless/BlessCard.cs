using System;
using ArtC.Systems;
using ArtC.Utils;
using R3;
using R3.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArtC.Bless {
    public class BlessCard : MonoBehaviour {
        [SerializeField] private RectTransform _content;
        [SerializeField] private ObservablePointerClickTrigger _pointerClickTrigger;
        [SerializeField] private ObservablePointerEnterTrigger _pointerEnterTrigger;
        [SerializeField] private ObservablePointerExitTrigger _pointerExitTrigger;
        [SerializeField] private TMP_Text _blessDescriptionText;

        [Header("パラメーター")] [SerializeField] private BlessTypes _blessType = BlessTypes.None;
        [SerializeField] private float _enterScaleMultiplier = 1.1f;

        private Vector3 _initialContentScale;

        public BlessTypes BlessType => _blessType;

        private void Awake() {
            _blessDescriptionText.text = "このテキストは見れないはずだよ！";
            _initialContentScale = _content.localScale;
        }

        private void Start() {
            _pointerClickTrigger.OnPointerClickAsObservable().Subscribe(_ => {
                if (_blessType == BlessTypes.None) {
                    Debug.LogError("BlessCard: BlessType が None です");
                    return;
                }

                Debug.Log($"BlessCard: {_blessType} が選択されました");

                _content.localScale = _initialContentScale;

                GameManager.Instance.SelectedBlessType.OnNext(_blessType);
            }).AddTo(this);

            _pointerEnterTrigger.OnPointerEnterAsObservable().Subscribe(_ => {
                Debug.Log("BlessCard: OnPointerEnterAsObservable");
                _content.localScale = _initialContentScale * _enterScaleMultiplier;
            }).AddTo(this);

            _pointerExitTrigger.OnPointerExitAsObservable().Subscribe(_ => {
                Debug.Log("BlessCard: OnPointerExitAsObservable");
                _content.localScale = _initialContentScale;
            }).AddTo(this);

            gameObject.SetActive(false);
        }

        public void SetBlessType(BlessTypes blessType) {
            _blessType = blessType;
        }

        public void SetBlessDescription(string description) {
            _blessDescriptionText.text = description;
        }
    }
}
