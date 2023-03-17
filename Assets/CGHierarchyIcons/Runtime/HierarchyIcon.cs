using UnityEngine;

namespace HierarchyIcons
{
    public class HierarchyIcon : MonoBehaviour
    {
#if UNITY_EDITOR
        public Texture2D icon;
        
        [Range(-3, 5)]
        public int position = -2;
        public Direction direction = Direction.LeftToRight;

        [TextArea]
        public string tooltip;
        
        public enum Direction
        {
            RightToLeft,
            LeftToRight
        }
#endif
    }
}