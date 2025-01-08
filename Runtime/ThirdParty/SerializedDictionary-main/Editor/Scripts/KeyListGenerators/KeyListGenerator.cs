﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.SerializedCollections.KeysGenerators
{
    public abstract class KeyListGenerator : ScriptableObject
    {
        public abstract IEnumerable GetKeys(System.Type type);
    }
}