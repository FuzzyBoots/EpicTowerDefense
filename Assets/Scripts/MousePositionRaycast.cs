using UnityEngine;

namespace TowerDefense {

    public class MousePositionRaycast : MonoBehaviour {


        public static MousePositionRaycast Instance { get; private set; }
        public Collider LastCollider { get => lastCollider; private set => lastCollider = value; }

        [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
        [SerializeField] private bool moveThisTransformOnUpdate;

        [SerializeField] Collider lastCollider;

        private void Awake() {
            Instance = this;
        }

        private void Update() {
            if (moveThisTransformOnUpdate) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, mouseColliderLayerMask)) {
                    transform.position = raycastHit.point;
                    LastCollider = raycastHit.collider;
                }
            }
        }

        private Vector3 GetPosition_Instance() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, mouseColliderLayerMask)) {
                return raycastHit.point;
            } else {
                return Vector3.zero;
            }
        }

        public static Vector3 GetPosition() {
            if (Instance == null) {
                Debug.LogError("There is no MousePositionRaycast in the Scene!");
            }
            return Instance.GetPosition_Instance();
        }

    }

}