﻿// SampSharp
// Copyright 2019 Tim Potze
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using SampSharp.Entities;
using SampSharp.Entities.SAMP.Components;

namespace TestMode.Entities.Services
{
    public class FunnyService : IScopedFunnyService
    {
        public Guid FunnyGuid { get; } = Guid.NewGuid();

        public string MakePlayerNameFunny(Entity player)
        {
            var name = player.GetComponent<Player>().Name;

            var result = string.Empty;

            for (var i = name.Length - 1; i >= 0; i--) result += name[i];

            return result;
        }
    }
}