using System;

namespace Langep.JamKit.DataStructures
{
    public class LimitedSizeOrderedSet<T>: OrderedSet<T>
    {
        private uint _maxCapacity;
        
        public LimitedSizeOrderedSet(uint maxCapacity = UInt32.MaxValue)
        {
            _maxCapacity = maxCapacity;
        }
        
        public virtual bool HasCapacityFor(uint amount = 1)
        {
            return Count + amount <= _maxCapacity;
        }

        public override bool Add(T item)
        {
            return HasCapacityFor(1) && base.Add(item);
        }
    }
}