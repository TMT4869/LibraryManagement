using FA.LibraryManagement.Core.Context;
using FA.LibraryManagement.Core.IRepositories;
using FA.LibraryManagement.Core.Repositories;

namespace FA.LibraryManagement.Core.Infrastructers;

/// <summary>
///     The unit of work class
/// </summary>
/// <seealso cref="IUnitOfWork" />
public class UnitOfWork : IUnitOfWork
{
    private IAuthorRepository _authorRepository;
    private ICategoryRepository _categoryRepository;
    private IUserRepository _userRepository;
    private IBookRepository _bookRepository;
    private IBookAuthorRepository _bookAuthorRepository;
    private IRoleRepository _roleRepository;
    private IUserRoleRepository _userRoleRepository;
    private ICartRepository _cartRepository;
    private IBorrowingRepository _borrowingRepository;
    private IBorrowingDetailRepository _borrowingDetailRepository;
    private IBookImageRepository _bookImageRepository;
    private IHistoryRepository _historyRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UnitOfWork" /> class
    /// </summary>
    public UnitOfWork()
    {
    }


    /// <summary>
    ///     Initializes a new instance of the <see cref="UnitOfWork" /> class
    /// </summary>
    /// <param name="context">The context</param>
    public UnitOfWork(LibraryManagementContext context)
    {
        AppDbContext = context;
    }

    /// <summary>
    ///     Gets the value of the app db context
    /// </summary>
    private LibraryManagementContext AppDbContext { get; }

    /// <summary>
    ///     Disposes this instance
    /// </summary>
    public void Dispose()
    {
        AppDbContext.Dispose();
    }

    public virtual IUserRepository UserRepository => _userRepository ??= new UserRepository(AppDbContext);

    public virtual IAuthorRepository AuthorRepository => _authorRepository ??= new AuthorRepository(AppDbContext);

    public virtual ICategoryRepository CategoryRepository =>
        _categoryRepository ??= new CategoryRepository(AppDbContext);

    public virtual IBookRepository BookRepository =>
        _bookRepository ??= new BookRepository(AppDbContext);

    public virtual IBookAuthorRepository BookAuthorRepository =>
        _bookAuthorRepository ??= new BookAuthorRepository(AppDbContext);

    public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(AppDbContext);
    public IUserRoleRepository RoleUserRepository => _userRoleRepository ??= new UserRoleRepository(AppDbContext);

    public ICartRepository CartRepository => _cartRepository ??= new CartRepository(AppDbContext);

    public IBorrowingRepository BorrowingRepository =>
        _borrowingRepository ??= new BorrowingRepository(AppDbContext);

    public IBorrowingDetailRepository BorrowingDetailRepository =>
        _borrowingDetailRepository ??= new BorrowingDetailRepository(AppDbContext);

    public IBookImageRepository BookImageRepository =>
        _bookImageRepository ??= new BookImageRepository(AppDbContext);

    public IHistoryRepository HistoryRepository => _historyRepository ??= new HistoryRepository(AppDbContext);

    /// <summary>
    ///     Saves the changes
    /// </summary>
    /// <returns>The int</returns>
    public virtual int SaveChanges()
    {
        return AppDbContext.SaveChanges();
    }
}