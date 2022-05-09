using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagerApi.Controllers;
using BookManagerApi.Models;
using BookManagerApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace BookManagerApi.Tests;

public class BookManagerControllerTests
{
    private BookManagerController _controller;
    private Mock<IBookManagementService> _mockBookManagementService;

    [SetUp]
    public void Setup()
    {
        //Arrange
        _mockBookManagementService = new Mock<IBookManagementService>();
        _controller = new BookManagerController(_mockBookManagementService.Object);
    }

    [Test]
    public async Task GetBooks_Returns_AllBooks()
    {
        //Arange
        _mockBookManagementService.Setup(b => b.GetAllBooksAsync()).Returns(Task.FromResult(GetTestBooks()));

        //Act
        var result = await _controller.GetBooks();

        //Assert
        result.Should().BeOfType(typeof(ActionResult<IEnumerable<Book>>));
        result.Value.Should().BeEquivalentTo(GetTestBooks());
        result.Value.Count().Should().Be(3);
    }

    [Test]
    public async Task GetBookById_Returns_CorrectBook()
    {
        //Arrange
        var testBookFound = GetTestBooks().FirstOrDefault();
        _mockBookManagementService.Setup(b => b.FindBookByIdAsync(1)).Returns(Task.FromResult(testBookFound));

        //Act
        var result = await _controller.GetBookById(1);

        //Assert
        result.Should().BeOfType(typeof(ActionResult<Book>));
        result.Value.Should().Be(testBookFound);
    }

    [Test]
    public async Task UpdateBookById_Updates_Correct_Book()
    {
        //Arrange
        long existingBookId = 3;
        Book existingBookFound = GetTestBooks()
            .FirstOrDefault(b => b.Id.Equals(existingBookId));

        var bookUpdates = new Book() { Id = 3, Title = "Book Three", Description = "I am updating this for Book Three", Author = "Person Three", Genre = Genre.Education };

        _mockBookManagementService.Setup(b => b.FindBookByIdAsync(existingBookId)).Returns(Task.FromResult(existingBookFound));

        //Act
        var result = await _controller.UpdateBookById(existingBookId, bookUpdates);

        //Assert
        result.Should().BeOfType(typeof(NoContentResult));
    }

    [Test]
    public async Task AddBook_Creates_A_Book()
    {
        //Arrange
        var newBook = new Book() { Id = 4, Title = "Book Four", Description = "This is the description for Book Four", Author = "Person Four", Genre = Genre.Education };

        _mockBookManagementService.Setup(b => b.Create(newBook)).Returns(Task.FromResult(newBook));

        //Act
        var result = await _controller.AddBook(newBook);

        //Assert
        result.Should().BeOfType(typeof(ActionResult<Book>));
    }

    private static List<Book> GetTestBooks()
    {
        return new List<Book>
        {
            new Book() { Id = 1, Title = "Book One", Description = "This is the description for Book One", Author = "Person One", Genre = Genre.Education },
            new Book() { Id = 2, Title = "Book Two", Description = "This is the description for Book Two", Author = "Person Two", Genre = Genre.Fantasy },
            new Book() { Id = 3, Title = "Book Three", Description = "This is the description for Book Three", Author = "Person Three", Genre = Genre.Thriller },
        };
    }
}
