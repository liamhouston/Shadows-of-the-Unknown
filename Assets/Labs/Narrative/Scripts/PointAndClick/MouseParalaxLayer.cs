using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Narrative
{
    /// <summary>
    /// A dispay effect class to give object a 3D scrolling appearance based on the mouse position in the window.
    /// Only works on static 2D objects.
    /// </summary>
    public class MouseParalaxLayer : MonoBehaviour
    {
        const float displayScale = 0.75f; // global scale for pivot distance

        //controls rate of change of pivot, positive is further, negative is closer, 0 will not pivot (typically center of room)
        [SerializeField] [Range(-1.0f, 1.0f)] private float distanceFromCamera = 0.5f;

        private Vector3 startPosition; // stores the starting position of the object so we can pivot around it

        // Start is called before the first frame update
        void Start()
        {
            startPosition = transform.position;//store the starting position
        }

        // Update is called once per frame
        void Update()
        {
            //Update the pivot based on mouse position
            Vector3 offset = GetMouseNormalized() * (distanceFromCamera * displayScale);
            transform.position = startPosition - offset;
        }

        /// <summary>
        /// Gets the mouse position in screen space, normalized from -1 to 1 based on width.
        /// Smoothly 
        /// </summary>
        private Vector2 GetMouseNormalized()
        {
            Vector2 size = new Vector2(Screen.width, Screen.height);
            Vector2 centered = (Vector2)Input.mousePosition - (0.5f * size);
            Vector2 normalized = centered / (size.x * 0.5f);
            Vector2 clamped = new Vector2(SmoothClamp(normalized.x, -1.0f, 1.0f, 0.5f), SmoothClamp(normalized.y, -1.0f, 1.0f, 0.5f));
            return clamped;
        }

        /// <summary>
        /// Calculates minimum with a smooth cutoff
        /// </summary>
        /// <param name="a">Value 1</param>
        /// <param name="b">Value 2</param>
        /// <param name="k">Smoothing difference</param>
        private float SmoothMin(float a, float b, float k)
        {
            float h = Mathf.Clamp01(0.5f + 0.5f * (a - b) / k);
            return Mathf.Lerp(a, b, h) - k * h * (1.0f - h);
        }

        /// <summary>
        /// Calculates maximum with a smooth cutoff
        /// </summary>
        /// <param name="a">Value 1</param>
        /// <param name="b">Value 2</param>
        /// <param name="k">Smoothing difference</param>
        private float SmoothMax(float a, float b, float k)
        {
            return -SmoothMin(-a, -b, k);
        }

        /// <summary>
        /// Smoothly clamps a value betweeen a minimum and maximum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="k"></param>
        private float SmoothClamp(float value, float min, float max, float k)
        {
            k = Mathf.Min(k, Mathf.Abs(max-min)); // bound the smoothing to avoid conflicts
            float step1 = SmoothMax(value, min, k);
            float step2 = SmoothMin(step1, max, k);
            return step2;
        }
    }
}