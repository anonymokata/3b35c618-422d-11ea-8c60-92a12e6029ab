using System;
using System.Collections.Generic;
using System.Text;

namespace Gzhao_checkout_total
{
    public class SpecialToken
    {
        /// <summary>
        /// The Special that this token represents.
        /// </summary>
        public Special special { get; private set; }

        /// <summary>
        /// The total amount of items this Special gets to affect.
        /// </summary>
        public int affectCount { get; private set; }

        /// <summary>
        /// The total amount of times this special has been invoked.
        /// </summary>
        public int fireCount { get; private set; }

        /// <summary>
        /// The total amount of times this special may be invoked per purchase.
        /// </summary>
        private int fireLimit;

        /// <summary>
        /// Create a new special token that represents the given special.
        /// </summary>
        /// <param name="sp">The Special that is being represented.</param>
        /// <param name="amount">The something or other that does a thing but for the time
        /// being I'm too lazy to look up.</param>
        public SpecialToken(Special sp)
        {
            special = sp;
            fireLimit = sp.specialApplyLimit;
            fireCount = 1;
            BuildAffectValue();
        }

        /// <summary>
        /// Creates a new special token with a dummy special.
        /// </summary>
        public SpecialToken()
        {
            special = new Special();
            fireLimit = special.specialApplyLimit;
            fireCount = 1;
            BuildAffectValue();
        }

        /// <summary>
        /// Set the amount of items this special should apply.
        /// </summary>
        private void BuildAffectValue()
        {
            affectCount = special.itemsApplied * fireLimit;
        }

        /// <summary>
        /// Increase the fire value of this special by one.
        /// The limit is handled on get, so don't worry about it here.
        /// </summary>
        public void Increment()
        {
            fireCount++;
        }

        /// <summary>
        /// Decrease the fire value of this special by one.
        /// </summary>
        public void Decrement()
        {
            fireCount--;
        }

        /// <summary>
        /// Returns true if the special of this token affects 
        /// the given item.
        /// </summary>
        /// <param name="name">The name of the item being affected.</param>
        /// <returns></returns>
        public bool Match(string name)
        {
            return special.Match(name);
        }

        /// <summary>
        /// Compares the item given with the parameters of this special,
        /// returns true if the item does indeed apply to the special.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CanBeAppliedTo(ItemInCart item)
        {
            bool result = false;
            //what do we care about?
            //1. If we're affecting the same item.
            //2. If the price is lower than...what.

            if (special.Match(item.GetName()))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Compares the item given with the given name,
        /// returns true if the special can be applied to the given item name.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CanBeAppliedTo(string item)
        {
            bool result = false;
            //what do we care about?
            //1. If we're affecting the same item.
            //2. If the price is lower than...what.

            if (special.Match(item))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Returns the total number of items this special can be applied.
        /// </summary>
        /// <returns>fireCount * itemsApplied.</returns>
        public int GetAffectedCount()
        {
            int fire = fireCount;
            if(fireCount > fireLimit && fireLimit != -1)
            {
                fire = fireLimit;
            }

            return fire * special.itemsApplied;
        }
        
        /// <summary>
        /// Sets an item to have this token's special.
        /// </summary>
        /// <param name="item"></param>
        public void SetSpecial(ItemInCart item)
        {
            item.SetSpecialValue(special);
        }
    }
}
