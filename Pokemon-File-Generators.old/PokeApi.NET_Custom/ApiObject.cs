using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    [Serializable()]
    public abstract class ApiObject
    {
        /// <summary>
        /// The identifier for this <see cref="ApiObject" />.
        /// </summary>
        public int ID
        {
            get;
            internal set;
        }
    }
    [Serializable()]
    public abstract class NamedApiObject : ApiObject
    {
        /// <summary>
        /// The name for this <see cref="NamedApiObject" />.
        /// </summary>
        public string Name
        {
            get;
            internal set;
        }
    }
}
