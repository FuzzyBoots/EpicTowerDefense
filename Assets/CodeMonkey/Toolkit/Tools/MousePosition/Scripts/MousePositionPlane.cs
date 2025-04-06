using UnityEngine;

namespace CodeMonkey.Toolkit.TMousePosition {

    public class MousePositionPlane {


        public static Vector3 GetPosition() {
            Ray mouseCameraRay;
            if (Camera.main)
            {
                mouseCameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            } else
            {
                Debug.Log("Null Camera.main");
                return Vector3.zero;
            }

            Plane plane = new Plane(Vector3.up, Vector3.zero);

            if (plane.Raycast(mouseCameraRay, out float distance)) {
                return mouseCameraRay.GetPoint(distance);
            } else {
                return Vector3.zero;
            }
        }

    }

}