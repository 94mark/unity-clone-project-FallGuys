using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
    public class IconAttribute : Attribute
    {
        public readonly Type type;
        public readonly string path;

        public IconAttribute(Type type)
        {
            this.type = type;
        }

        public IconAttribute(string path)
        {
            this.path = path;
        }
    }
}