﻿using System;

namespace Utilities.SerializedCollections.KeysGenerators
{
    public class KeyListGeneratorData
    {
        public string Name { get; set; }
        public Type TargetType { get; set; }
        public Type GeneratorType { get; set; }
        public bool NeedsWindow { get; set; }

        public KeyListGeneratorData(string name, Type targetType, Type populatorType, bool needsWindow)
        {
            Name = name;
            TargetType = targetType;
            GeneratorType = populatorType;
            NeedsWindow = needsWindow;
        }
    }
}