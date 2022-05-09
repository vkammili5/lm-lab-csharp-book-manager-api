using System;
using BookManagerApi.Models;

namespace BookManagerApi.Services
{
	public interface IBookManagementService
	{
        Task<List<Book>> GetAllBooksAsync();
        Task<Book> Create(Book book);
        Task<Book> Update(long id, Book book);
        Task<Book> FindBookByIdAsync(long id);
        bool BookExists(long id);
    }
}
