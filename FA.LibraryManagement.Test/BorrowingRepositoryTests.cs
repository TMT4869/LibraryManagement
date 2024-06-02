using FA.LibraryManagement.Core.Context;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.IRepositories;
using FA.LibraryManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FA.LibraryManagement.Test
{
    public class BorrowingRepositoryTests
    {
        private LibraryManagementContext _context;
        private IUnitOfWork _unitOfWork;
        private IBorrowingRepository _borrowingRepository;

        [SetUp]
        public void Setup()
        {
            DbContextOptions<LibraryManagementContext> options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            if (!_context.Database.EnsureCreated())
            {
                return;
            }
            _unitOfWork = new UnitOfWork(_context);
            _borrowingRepository = _unitOfWork.BorrowingRepository;
        }

        [Test]
        public void UpdateStatus_WithValidId_UpdatesStatusCorrectly()
        {
            // Arrange
            var borrowing = new Borrowing { Status = "OldStatus" };
            _borrowingRepository.Create(borrowing);
            _unitOfWork.SaveChanges();

            // Act
            _borrowingRepository.UpdateStatus(borrowing.Id, "NewStatus");

            // Assert
            Assert.That(borrowing.Status, Is.EqualTo("NewStatus"));
        }

        [Test]
        public void CountByStatus_WithValidStatus_ReturnsCorrectCount()
        {
            // Arrange
            var borrowings = new List<Borrowing>
            {
                new Borrowing { Status = "Status1" },
                new Borrowing { Status = "Status1" },
                new Borrowing { Status = "Status2" }
            };
            _borrowingRepository.CreateRange(borrowings);
            _unitOfWork.SaveChanges();

            // Act
            var count = _borrowingRepository.CountByStatus("Status1");

            // Assert
            Assert.That(count, Is.EqualTo(2));
        }


        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
            _unitOfWork.Dispose();
        }
    }
}