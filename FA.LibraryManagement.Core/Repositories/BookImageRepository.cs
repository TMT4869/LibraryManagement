using FA.LibraryManagement.Core.Context;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.IRepositories;
using FA.LibraryManagement.Core.Models;

namespace FA.LibraryManagement.Core.Repositories;

public class BookImageRepository : BaseRepository<BookImage>, IBookImageRepository
{
    public BookImageRepository(LibraryManagementContext context) : base(context)
    {
    }
}