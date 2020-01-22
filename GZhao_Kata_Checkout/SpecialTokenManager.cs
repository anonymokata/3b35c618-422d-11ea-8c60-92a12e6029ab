using System;
using System.Collections.Generic;
using System.Text;

namespace Gzhao_checkout_total
{
    class SpecialTokenManager
    {
        private List<SpecialToken> listOfTokens;

        public SpecialTokenManager()
        {
            listOfTokens = new List<SpecialToken>();
        }

        public SpecialTokenManager(List<SpecialToken> lst)
        {
            listOfTokens = lst;
        }

        /// <summary>
        /// Adds a token to the list of tokens.
        /// If a token already exists, increment its firing by one instead.
        /// </summary>
        public void AddToken(SpecialToken token)
        {
            if (!listOfTokens.Contains(token))
            {
                listOfTokens.Add(token);
            }
            else
            {
                token.Increment();
            }
        }

        /// <summary>
        /// Adds a special as a token into the list of tokens.
        /// </summary>
        /// <param name="token">The special we want tokenized.</param>
        public void AddToken(Special token)
        {
            bool duplicate = HasToken(token.itemAffected);
            if (!duplicate)
            {
                listOfTokens.Add(new SpecialToken(token));
            }
            else
            {
                listOfTokens[HasTokenPosition(token)].Increment();
            }
        }

        /// <summary>
        /// Adds a special as a token into the list of tokens.
        /// </summary>
        /// <param name="token">The special we want tokenized.</param>
        public void AddToken(string token)
        {
            bool duplicate = HasToken(token);
            if (!duplicate)
            {
                listOfTokens.Add(new SpecialToken(Database_API.GetSpecial(token)));
            }
            else
            {
                listOfTokens[HasTokenPosition(token)].Increment();
            }
        }
        
        /// <summary>
        /// Returns true if the manager has a special that affects the given item
        /// and can fire.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasToken(string key)
        {
            bool result = false;
            foreach (SpecialToken token in listOfTokens)
            {
                result = token.CanBeAppliedTo(key);
                if (result)
                {
                    result = token.fireCount > 0;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the position of the first special that affects
        /// the given item in the list of tokens.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int HasTokenPosition(string name)
        {
            bool has = false;
            int i = -1;
            while (!has && i < listOfTokens.Count-1)
            {
                i++;
                has = listOfTokens[i].Match(name);
                
            }

            return i;
        }

        /// <summary>
        /// Returns the position of the first special that affects
        /// the given item in the list of tokens.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int HasTokenPosition(Special name)
        {
            return HasTokenPosition(name.itemAffected);
        }

        /// <summary>
        /// Returns the token at the given position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public SpecialToken GetToken(int position)
        {
            return listOfTokens[position];
        }

        /// <summary>
        /// Returns the token that affects the given string.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SpecialToken GetToken(string name)
        {
            return listOfTokens[HasTokenPosition(name)];
        }

        /// <summary>
        /// Removes and returns a specialToken at the given position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public SpecialToken RemoveToken(int position)
        {
            SpecialToken token = listOfTokens[position];
            RemoveTokenInternal(token);
            return token;
        }

        /// <summary>
        /// Remove and return a specialToken that affects the given item.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SpecialToken RemoveToken(string name)
        {
            SpecialToken token = GetToken(name);
            RemoveTokenInternal(token);
            return token;
        }

        /// <summary>
        /// Removes the given special from the list of tokens.
        /// </summary>
        /// <param name="special"></param>
        public void RemoveToken(Special special)
        {
            int position = HasTokenPosition(special.itemAffected);
            SpecialToken token = listOfTokens[position];
            RemoveTokenInternal(token);
        }

        private void RemoveTokenInternal(SpecialToken token)
        {
            if(token.fireCount > 1)
            {
                token.Decrement();
            }
            else
            {
                listOfTokens.Remove(token);
            }
        }

        /// <summary>
        /// Return the total count of tokens within this manager.
        /// </summary>
        /// <returns></returns>
        public int GetTokenListSize()
        {
            return listOfTokens.Count;
        }
    }
}
