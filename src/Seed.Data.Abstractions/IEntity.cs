﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seed.Data
{
    public interface IEntity
    {
        JObject Properties { get; }
    }
}