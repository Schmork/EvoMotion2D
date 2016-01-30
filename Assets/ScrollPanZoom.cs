using UnityEngine;
using System.Collections;

namespace EvoMotion2D
{
    public class ScrollPanZoom : MonoBehaviour
    {

        public float ScrollSpeed = 0;
        public float ScrollEdge = 0.01f;
        public float PanSpeed = 10;
        Vector2 ZoomRange = new Vector2(0, 1000);
        float CurrentZoom = 20;
        public float ZoomSpeed = 0.03f;

        void Update()
        {
            //PAN
            if (Input.GetMouseButton(2))
            {
                //(Input.mousePosition.x - Screen.width * 0.5)/(Screen.width * 0.5)

                transform.Translate(Vector3.right * Time.deltaTime * PanSpeed * (Input.mousePosition.x - Screen.width * 0.5f) / (Screen.width * 0.5f), Space.World);
                transform.Translate(Vector3.up * Time.deltaTime * PanSpeed * (Input.mousePosition.y - Screen.height * 0.5f) / (Screen.height * 0.5f), Space.World);

            }
            else {
                if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge))
                {
                    transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed, Space.World);
                }
                else if (Input.GetKey("a") || Input.mousePosition.x <= Screen.width * ScrollEdge)
                {
                    transform.Translate(Vector3.right * Time.deltaTime * -ScrollSpeed, Space.World);
                }

                if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge))
                {
                    transform.Translate(Vector3.up * Time.deltaTime * ScrollSpeed, Space.World);
                }
                else if (Input.GetKey("s") || Input.mousePosition.y <= Screen.height * ScrollEdge)
                {
                    transform.Translate(Vector3.up * Time.deltaTime * -ScrollSpeed, Space.World);
                }
            }

            //ZOOM IN/OUT

            CurrentZoom -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime;
            CurrentZoom = Mathf.Clamp(CurrentZoom, ZoomRange.x, ZoomRange.y);

            GetComponent<Camera>().orthographicSize = 10 + CurrentZoom;
        }
    }
}