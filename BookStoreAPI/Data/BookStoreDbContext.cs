using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Shared.Entities;

namespace BookStoreAPI.Data
{
    //Create a DbContext class named BookStoreDbContext with a DbSet for Book class. #Book.cs
    public class BookStoreDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
        {
        }
    }
}
