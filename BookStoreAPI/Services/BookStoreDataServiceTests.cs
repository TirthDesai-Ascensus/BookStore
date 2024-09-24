using BookStore.Shared.Entities;
using BookStoreAPI.Data;
using BookStoreAPI.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

[TestFixture]
public class BookStoreDataServiceTests
{
    private DbSet<Book> _fakeDbSet;
    private BookStoreDbContext _fakeContext;
    private BookStoreDataService _service;

    [SetUp]
    public void SetUp()
    {
        _fakeDbSet = A.Fake<DbSet<Book>>(options => options.Wrapping(new List<Book>().AsQueryable().BuildMockDbSet()));
        _fakeContext = A.Fake<BookStoreDbContext>();
        A.CallTo(() => _fakeContext.Books).Returns(_fakeDbSet);
        _service = new BookStoreDataService(_fakeContext);
    }

    [Test]
    public async Task DeleteBookAsync_BookExists_DeletesBook()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Test Book" };
        A.CallTo(() => _fakeDbSet.FindAsync(1)).Returns(new ValueTask<Book>(book));
        A.CallTo(() => _fakeDbSet.Remove(book)).Returns(book);
        A.CallTo(() => _fakeContext.SaveChangesAsync(default)).Returns(Task.FromResult(1));

        // Act
        await _service.DeleteBookAsync(1);

        // Assert
        A.CallTo(() => _fakeDbSet.Remove(book)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeContext.SaveChangesAsync(default)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task DeleteBookAsync_BookDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        A.CallTo(() => _fakeDbSet.FindAsync(1)).Returns(new ValueTask<Book>((Book)null));

        // Act
        Func<Task> act = async () => await _service.DeleteBookAsync(1);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Book not found");
    }
}