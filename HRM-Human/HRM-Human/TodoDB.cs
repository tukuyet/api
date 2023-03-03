using System;
using Microsoft.EntityFrameworkCore;
namespace HRM_Human

{
    public class TodoDb :DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();
    }
}
