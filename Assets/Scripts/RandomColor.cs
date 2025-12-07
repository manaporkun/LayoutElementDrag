using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    /// <summary>
    /// Utility component that assigns a random color to the attached Image component.
    /// Useful for demo/testing purposes to visually distinguish elements.
    /// </summary>
    public class RandomColor : MonoBehaviour
    {
        private void Start()
        {
            var image = GetComponent<Image>();
            if (image != null)
            {
                image.color = new Color(Random.value, Random.value, Random.value);
            }
            else
            {
                Debug.LogWarning($"[RandomColor] No Image component found on {gameObject.name}");
            }
        }
    }
}
