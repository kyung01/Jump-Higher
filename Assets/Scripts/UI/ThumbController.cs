using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UI
{

	public class ThumbController : MonoBehaviour
	{
		public Canvas canvas;
		public UnityEngine.UI.Image imageBackground;
		public UnityEngine.UI.Image imageCenter;
		// Use this for initialization
		void Start()
		{

		}
		public int getDirection()
		{
			float dis = imageCenter.rectTransform.position.x-imageBackground.rectTransform.position.x;
			if (Mathf.Abs(dis) < 10)
			{
				return 0;
			}
			if (dis<0) return -1;
			else return 1;
		}

		private void Update()
		{

			if(Application.platform == RuntimePlatform.WindowsEditor)
			{
				UpdatePosition(Input.mousePosition );

			}
			else
			{

				if (Input.touchCount == 0)
				{
					UpdatePosition(imageBackground.rectTransform.position);
				}
				else
				{
					UpdatePosition(Input.touches[0].position);
				}
			}
		}
		// Update is called once per frame
		void UpdatePosition(Vector3 inputPosition)
		{
			//Debug.Log(imageBackground.canvas.scaleFactor);
			var center = imageBackground.rectTransform.position;
			float allowedDistance = imageBackground.rectTransform.rect.width*0.5f * canvas.scaleFactor;
			var dis = inputPosition - center;
			if (dis.magnitude > allowedDistance)
			{
				//Debug.Log("UPdating " +(imageBackground.canvas.scaleFactor * allowedDistance));
				var correctedPosition = center + dis.normalized * allowedDistance;
				//Debug.Log(imageBackground.rectTransform.position + " to " +correctedPosition);
				updateImageCetner(correctedPosition);
			}
			else updateImageCetner(inputPosition);
		}
		bool isInsideRect(RectTransform rectTransform, Vector3 position)
		{
			float xMin = rectTransform.position.x + rectTransform.rect.xMin;
			float xMax = rectTransform.position.x + rectTransform.rect.xMax;
			float yMin = rectTransform.position.y + rectTransform.rect.yMin;
			float yMax = rectTransform.position.y + rectTransform.rect.yMax;
			return position.x > xMin && position.x < xMax && position.y > yMin && position.y < yMax;
		}
		void updateImageCetner( Vector3 position)
		{


			imageCenter.rectTransform.position = new Vector3(
				position.x , 
				position.y , 0);
		}
	}


}