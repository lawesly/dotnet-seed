﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Data
{
    public interface IStore
    {
        DbContext CreateDbContext();

        Task InitializeAsync();
    }
}