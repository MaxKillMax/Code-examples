using WOE.Events;

namespace WOE
{
    /// <summary>
    /// Use (this as IPrioritable).TrySetPriority(ref ushort value, ushort new)
    /// </summary>
    public interface IPrioritable
    {
        /// <summary>
        /// Use (this as IPrioritable).TrySetPriority(ref ushort value, ushort new)
        /// </summary>
        public Event OnChanged { get; }

        /// <summary>
        /// Subscribe to find out when it's this item's turn
        /// </summary>
        public Event OnHighestPriorityReached { get; }

        /// <summary>
        /// Subscribe to find out when the queue of the object has passed
        /// </summary>
        public Event OnHighestPriorityGone { get; }

        /// <summary>
        /// The higher the more important
        /// </summary>
        public ushort Priority { get; }

        public bool TrySetPriority(ref ushort Priority, ushort value)
        {
            if (Priority == value)
                return false;

            Priority = value;
            OnChanged?.Invoke();
            return true;
        }
    }
}
