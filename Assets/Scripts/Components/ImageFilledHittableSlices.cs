using UnityEngine;
using UnityEngine.UI;

namespace TFM.Components
{
    /// <summary>
    /// Class <c>ImageFilledHittableSlices</c> is a custom image class that allows for hittable slices.
    /// </summary>
    public class ImageFilledHittableSlices : Image
    {
        /// <summary>
        /// Method <c>IsRaycastLocationValid</c> checks if the raycast location is valid.
        /// </summary>
        /// <param name="screenPoint">The screen point.</param>
        /// <param name="eventCamera">The event camera.</param>
        /// <returns>A boolean.</returns>
        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            var result = base.IsRaycastLocationValid(screenPoint, eventCamera);
            if (!result)
                return false;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out var localPoint);

            // Get the local point's normalized coordinates (0,0) to (1,1) within the rect
            var rect = rectTransform.rect;
            localPoint.x = (localPoint.x - rect.xMin) / rect.width;
            localPoint.y = (localPoint.y - rect.yMin) / rect.height;

            // Center the localPoint to (0.5, 0.5)
            localPoint -= new Vector2(0.5f, 0.5f);

            // Calculate angle in degrees
            var angle = Mathf.Atan2(localPoint.y, localPoint.x) * Mathf.Rad2Deg;

            // Adjust the angle based on fill origin and direction
            angle = fillOrigin switch
            {
                0 => // Bottom
                    (fillClockwise ? (360 - angle) : angle) - 90,
                1 => // Right
                    (fillClockwise ? (360 - angle) : angle) - 180,
                2 => // Top
                    (fillClockwise ? (360 - angle) : angle) - 270,
                3 => // Left
                    (fillClockwise ? (360 - angle) : angle) - 0,
                // Adjust the angle to be between 0 and 360 degrees
                _ => (angle + 360) % 360
            };

            if (angle < 0)
                angle += 360f;
            if (angle >= 360f)
                angle -= 360f;

            return (angle / 360f) <= fillAmount;
        }
    }
}
