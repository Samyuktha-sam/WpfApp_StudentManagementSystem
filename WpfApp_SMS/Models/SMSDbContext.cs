﻿using Microsoft.EntityFrameworkCore;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_SMS.Models
{
    public class SMSDbContext:DbContext
    {
        public DbSet<Student> Students { get; set; }

        public readonly string _path = @"C:\Users\Asus\Downloads\WpfApp_SMS\WpfApp_SMS\DB\SMSdb.db";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_path}");
        }
    }
}
