using WOE.Events;
using WOE.LiveObjects.LiveComponents.SpriteChangers;
using WOE.LiveObjects.Updates;

namespace WOE.LiveObjects.LiveComponents.SpriteAnimators
{
    public class SpriteAnimator : LiveComponent, IUpdatable
    {
        public Event OnAnimationsEnded { get; private set; } = new();

        private SpriteChanger _spriteChanger;
        private SpriteAnimationData _animation;

        private int _counter = 0;
        private bool _update = false;

        public float Delay { get; private set; }
        public float Time { get; set; }

        public SpriteAnimator(SpriteAnimatorData data) : base(data.DescriptionData)
        {
            _spriteChanger = data.SpriteChanger;
        }

        public void Stop()
        {
            _update = false;
            Delay = float.MaxValue;
            OnAnimationsEnded?.Invoke();
        }

        public void SetAnimation(SpriteAnimationData data)
        {
            if (Equals(data, _animation))
                return;

            _update = data != null && data.Sprites != null && data.Sprites.Length != 0 && data.Duration != 0;

            if (_update)
            {
                _counter = 0;
                _animation = data;

                Delay = data.Duration / data.Sprites.Length;
                Time = 0;

                RefreshSprite();
            }
            else
            {
                Delay = float.MaxValue;
            }
        }

        public void Update()
        {
            if (!_update)
                return;

            _counter++;

            if (_counter >= _animation.Sprites.Length)
            {
                if (_animation.Loop)
                    RefreshSprite();
                else
                    OnAnimationsEnded?.Invoke();
            }
            else
            {
                _spriteChanger.SetSprite(_animation.Sprites[_counter]);
            }
        }

        private void RefreshSprite()
        {
            _counter = 0;
            _spriteChanger.SetSprite(_animation.Sprites[_counter]);
        }
    }
}
