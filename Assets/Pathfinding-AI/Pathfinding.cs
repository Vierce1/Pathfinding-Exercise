using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    abstract public class Node<T>
    {
        // store ref to T as Value. Abstract data type,
        // concrete implementation will use Vector2/3 etc
        public T Value { get; set; }
        protected Node(T value) => Value = value;
    }


}