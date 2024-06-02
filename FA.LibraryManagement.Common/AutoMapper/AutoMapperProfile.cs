using AutoMapper;
using FA.LibraryManagement.Common.ViewModels;
using FA.LibraryManagement.Core.Models;

namespace FA.LibraryManagement.Common.AutoMapper;

/// <summary>
///     The just blog profile class
/// </summary>
/// <seealso cref="Profile" />
public class AutoMapperProfile : Profile
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AutoMapperProfile" /> class
    /// </summary>
    public AutoMapperProfile()
    {
        CreateMap<Book, BookVM>()
            .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.BookAuthors.Select(ba => ba.AuthorId)))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.AuthorNames,
                opt => opt.MapFrom(src =>
                    src.BookAuthors.Select(ba => ba.Author.Name).AsEnumerable()))
            .ForMember(dest => dest.BookImages, opt => opt.MapFrom(src => src.BookImages));

        CreateMap<BookVM, Book>()
            .ForMember(dest => dest.BookAuthors,
                opt => opt.MapFrom(src => src.Authors.Select(authorId => new BookAuthor { AuthorId = authorId })))
            .ForMember(dest => dest.BookImages,
                opt => opt.MapFrom(src => src.BookImages));

        CreateMap<User, UserVM>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.UserRoles.FirstOrDefault().RoleId));

        CreateMap<User, UserCreateVM>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
            .ReverseMap();

        CreateMap<Role, RoleVM>().ReverseMap();

        CreateMap<Cart, CartVM>().ReverseMap();

        CreateMap<Borrowing, BorrowingVM>().ReverseMap();

        CreateMap<BorrowingDetail, BorrowingDetailVM>().ReverseMap();

        CreateMap<BorrowingDetail, BorrowingTodayListVM>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.NumberOfBooks, opt => opt.MapFrom(src => src.NumberOfBooks))
            .ReverseMap();

        CreateMap<BookImage, BookImageVM>().ReverseMap();

        CreateMap<Category, CategoryVM>().ReverseMap();
        CreateMap<Author, AuthorVM>().ReverseMap();

        CreateMap<History, HistoryVM>().ReverseMap();
    }
}