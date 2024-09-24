

/* Add methods for retrieving books, Getting book by id, search books, add new book, update book and delete book data from database using BooStoreDbContext.  #file:'Book.cs' #file:'BookStoreDbContext.cs' 
         * Delete method takes id as argument and search for book, if exits delete the book otherwise throws error. Update method search for book using 
         * id and update if exists otherwise throws error.
 *
 * /fix (lines here ) . fix the warnings / errors in the code
 * /optimize (lines here ) . optimize the function. Throws exception if the book is not found for updation.
 * /explain (lines selection) to get detailed explanation of the code
 * /doc adds comments to selection or whole class
 *
        */
using Microsoft.EntityFrameworkCore;
using BookStore.Shared.Entities;
using BookStoreAPI.Data;

namespace BookStoreAPI.Services
{
    /// <summary>
    /// Provides methods for retrieving, adding, updating, and deleting book data from the database using BookStoreDbContext.
    /// </summary>
    public class BookStoreDataService
    {
        private readonly BookStoreDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreDataService"/> class.
        /// </summary>
        /// <param name="context">The BookStoreDbContext instance.</param>
        public BookStoreDataService(BookStoreDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all books from the database.
        /// </summary>
        /// <returns>A list of all books.</returns>
        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Retrieves a book by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the book.</param>
        /// <returns>The book with the specified ID.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the book with the specified ID is not found.</exception>
        public async Task<Book?> GetBookByIdAsync(int id)
        {
            var book = await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }
            return book;
        }

        /// <summary>
        /// Searches books by title or author in the database.
        /// </summary>
        /// <param name="searchTerm">The search term to match against book titles or authors.</param>
        /// <returns>A list of books matching the search term.</returns>
        public async Task<List<Book>> SearchBooksAsync(string searchTerm)
        {
            return await _context.Books
                .AsNoTracking()
                .Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm))
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="book">The book to add.</param>
        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a book in the database.
        /// </summary>
        /// <param name="book">The updated book.</param>
        /// <returns>The updated book.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the book with the specified ID is not found.</exception>
        public async Task<Book> UpdateBookAsync(Book book)
        {
            var existingBook = await _context.Books.FindAsync(book.Id);
            if (existingBook == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            _context.Entry(existingBook).CurrentValues.SetValues(book);
            await _context.SaveChangesAsync();
            return existingBook;
        }

        /// <summary>
        /// Deletes a book from the database.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown when the book with the specified ID is not found.</exception>
        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a list of distinct authors from the provided list of books.
        /// </summary>
        /// <param name="books">The list of books to extract authors from.</param>
        /// <returns>A list of unique author names.</returns>
        public List<string> GetDistinctAuthors(List<Book> books)
        {
            // Using a HashSet to store unique author names
            HashSet<string> authors = new HashSet<string>();
            foreach (var book in books)
            {
                authors.Add(book.Author);
            }
            return authors.ToList();
        }
    }
}


