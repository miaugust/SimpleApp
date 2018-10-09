using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleApp.Areas.Identity.Data;

    public class userContext : DbContext
    {
        public userContext (DbContextOptions<userContext> options)
            : base(options)
        {
        }

        public DbSet<SimpleApp.Areas.Identity.Data.SimpleAppUser> SimpleAppUser { get; set; }
    }
