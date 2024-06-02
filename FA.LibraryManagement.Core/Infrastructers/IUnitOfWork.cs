using FA.LibraryManagement.Core.IRepositories;

namespace FA.LibraryManagement.Core.Infrastructers;

/// <summary>
///     The unit of work interface
/// </summary>
/// <seealso cref="IDisposable" />
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    ///     Gets the value of the user repository
    /// </summary>
    public IUserRepository UserRepository { get; } // read only

    public IAuthorRepository AuthorRepository { get; } // read only

    public ICategoryRepository CategoryRepository { get; } // read only

    public IBookRepository BookRepository { get; } // read only

    public IBookAuthorRepository BookAuthorRepository { get; } // read only

    public IRoleRepository RoleRepository { get; } // read only

    public IUserRoleRepository RoleUserRepository { get; } // read only

    public ICartRepository CartRepository { get; } // read only

    public IBorrowingRepository BorrowingRepository { get; } // read only

    public IBorrowingDetailRepository BorrowingDetailRepository { get; } // read only

    public IBookImageRepository BookImageRepository { get; } // read only

    public IHistoryRepository HistoryRepository { get; } // read only

    /// <summary>
    ///     Saves the changes
    /// </summary>
    /// <returns>The int</returns>
    int SaveChanges();
}