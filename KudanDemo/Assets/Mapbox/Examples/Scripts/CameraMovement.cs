namespace Mapbox.Examples
{
    using Mapbox.Unity.Map;
    using Mapbox.Utils;
    using UnityEngine;

	public class CameraMovement : MonoBehaviour
	{
		[SerializeField]
		float _panSpeed = 20f;

		[SerializeField]
		float _zoomSpeed = 50f;

        public double latitude = 0f;
        public double longitude = 0f;

        public BasicMap basicMap;

		[SerializeField]
		Camera _referenceCamera;

		Quaternion _originalRotation;
		Vector3 _origin;
		Vector3 _delta;
		bool _shouldDrag;

		void Awake()
		{
			_originalRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

			if (_referenceCamera == null)
			{
				_referenceCamera = GetComponent<Camera>();
				if (_referenceCamera == null)
				{
					throw new System.Exception("You must have a reference camera assigned!");
				}
			}
		}

		void LateUpdate()
		{
            var x = 0f;
            var y = 0f;
            var z = 0f;

            if (Input.GetMouseButton(0))
            {
                var mousePosition = Input.mousePosition;
                mousePosition.z = _referenceCamera.transform.localPosition.y;
                _delta = _referenceCamera.ScreenToWorldPoint(mousePosition) - _referenceCamera.transform.localPosition;
                _delta.y = 0f;
                if (_shouldDrag == false)
                {
                    _shouldDrag = true;
                    _origin = _referenceCamera.ScreenToWorldPoint(mousePosition);
                }
            }
            else
            {
                _shouldDrag = false;
            }

            if (_shouldDrag == true)
            {
                var offset = _origin - _delta;
                offset.y = transform.localPosition.y;
                transform.localPosition = offset;
            }
            else
            {
                x = Input.GetAxis("Horizontal");
                z = Input.GetAxis("Vertical");
                y = -Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
                transform.localPosition += transform.forward * y + (_originalRotation * new Vector3(x * _panSpeed, 0, z * _panSpeed));
            }

            //if (Input.GetMouseButtonDown(1))
            //{
            //    ScrollToLatLon(latitude, longitude);
            //}
		}

        public void ScrollToLatLon(double lat, double lon)
        {
            Debug.Log("scrolled map");
            basicMap.Initialize(new Vector2d(lat, lon), 15);
            transform.position = new Vector3(0f, transform.position.y, 0f);            
        }
	}
}