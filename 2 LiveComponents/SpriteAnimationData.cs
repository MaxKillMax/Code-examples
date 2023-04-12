using UnityEngine;

namespace WOE.LiveObjects.LiveComponents.SpriteAnimators
{
    [CreateAssetMenu(fileName = nameof(SpriteAnimationData), menuName = nameof(SpriteAnimationData), order = 51)]
    public class SpriteAnimationData : ScriptableObject
    {
        public Sprite[] Sprites;
        public float Duration;
        public bool Loop;
    }
}
