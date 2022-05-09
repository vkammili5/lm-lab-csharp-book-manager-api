using System;
using BookManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagerApi.Services
{
	public class BookManagementService : IBookManagementService
	{
        private readonly BookContext _context;

        public BookManagementService(BookContext context)
        {
            _context = context;
        }


        public async Task<List<Book>> GetAllBooksAsync()
        {
            var books = await _context.Books.ToListAsync();
            return books;
        }

        public async Task<Book> Create(Book book)
        {
            _context.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> Update(long id, Book book)
        {
            var existingBookFound = await FindBookByIdAsync(id);

            existingBookFound.Title = book.Title;
            existingBookFound.Description = book.Description;
            existingBookFound.Author = book.Author;
            existingBookFound.Genre = book.Genre;

            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> FindBookByIdAsync(long id)
        {
            var book = await _context.Books.FindAsync(id);
            return book;
        }

        public bool BookExists(long id)
        {
            return _context.Books.Any(b => b.Id == id);
        }
    }
}

