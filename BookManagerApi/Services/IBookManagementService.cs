﻿using System;
using BookManagerApi.Models;

namespace BookManagerApi.Services
{
	public interface IBookManagementService
	{
        List<Book> GetAllBooks();
        Book Create(Book book);
        Book Update(long id, Book book);
        Book FindBookById(long id);
        bool BookExists(long id);
        void DeleteBook(long id);
    }
}
