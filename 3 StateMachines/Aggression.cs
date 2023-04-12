using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WOE.LiveObjects.LiveComponents.Movements;
using WOE.LiveObjects.LiveComponents.Targets;

namespace WOE.LiveObjects.LiveComponents.Aggressions
{
    public class Aggression : LiveComponent, IPrioritable
    {
        public Events.Event OnChanged { get; private set; } = new();
        public Events.Event OnHighestPriorityReached { get; private set; } = new();
        public Events.Event OnHighestPriorityGone { get; private set; } = new();

        private readonly Movement _movement;
        private readonly TargetType _targetType;
        private readonly Aura _aura;
        private readonly ushort _activePriority;

        private List<LiveObject> _enemies = new(5);

        private ushort _priority;
        public ushort Priority => _priority;

        public Aggression(AggressionData data) : base(data.DescriptionData)
        {
            _movement = data.Movement;
            _targetType = data.TargetType;
            _activePriority = data.Priority;
            _aura = data.Aura;

            _aura.OnLiveObjectAdded.AddListener(TryAddEnemy);
            _aura.OnLiveObjectRemoved.AddListener(TryRemoveEnemy);

            OnHighestPriorityGone.AddListener(_movement.Stop);
            OnHighestPriorityReached.AddListener(StartAggression);
        }

        private void TryAddEnemy(LiveObject liveObject)
        {
            if (_enemies.Contains(liveObject) || !liveObject.TryGetLiveComponent(out Target target, (t) => t != null) || target.Type != _targetType)
                return;

            _enemies.Add(liveObject);
            _enemies.OrderBy(e => Vector2.Distance(_aura.transform.position, e.transform.position));

            (this as IPrioritable).TrySetPriority(ref _priority, _activePriority);
        }

        private void TryRemoveEnemy(LiveObject liveObject)
        {
            if (!_enemies.Contains(liveObject))
                return;

            _enemies.Remove(liveObject);
        }

        private void StartAggression()
        {
            if (_enemies.Count == 0)
                (this as IPrioritable).TrySetPriority(ref _priority, 0);
            else
                _movement.Start(_enemies[0].transform.position);
        }

        public override List<ParameterData> GetParameters()
        {
            return new()
            {
                new()
                {
                    Order = 0,
                    Title = "Radius",
                    Value = _aura.Scale.ToString("N1")
                }
            };
        }
    }
}
