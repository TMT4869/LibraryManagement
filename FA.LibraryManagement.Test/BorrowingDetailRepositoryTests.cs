using FA.LibraryManagement.Core.Context;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.IRepositories;
using FA.LibraryManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FA.LibraryManagement.Test
{
    public class BorrowingDetailRepositoryTests
    {
        private LibraryManagementContext _context;
        private IUnitOfWork _unitOfWork;
        private IBorrowingDetailRepository _borrowingDetailRepository;

        [SetUp]
        public void Setup()
        {
            DbContextOptions<LibraryManagementContext> options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            if (!_context.Database.EnsureCreated())
            {
            }
            _unitOfWork = new UnitOfWork(_context);
            _borrowingDetailRepository = _unitOfWork.BorrowingDetailRepository;
        }

        [Test]
        public void UpdateStatus_WithValidId_UpdatesStatusCorrectly()
        {
            // Arrange
            var borrowingDetail = new BorrowingDetail { Status = "OldStatus" };
            _borrowingDetailRepository.Create(borrowingDetail);
            _unitOfWork.SaveChanges();

            // Act
            _borrowingDetailRepository.UpdateStatus(borrowingDetail.Id, "NewStatus");

            // Assert
            Assert.That(borrowingDetail.Status, Is.EqualTo("NewStatus"));
        }

        [Test]
        public void CountByStatus_WithValidStatus_ReturnsCorrectCount()
        {
            // Arrange
            var borrowingDetails = new List<BorrowingDetail>
            {
                new BorrowingDetail { Status = "Status1" },
                new BorrowingDetail { Status = "Status1" },
                new BorrowingDetail { Status = "Status2" }
            };
            _borrowingDetailRepository.CreateRange(borrowingDetails);
            _unitOfWork.SaveChanges();

            // Act
            var count = _borrowingDetailRepository.CountByStatus("Status1");

            // Assert
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void TotalFine_ReturnsCorrectTotalFine()
        {
            // Arrange
            var borrowingDetails = new List<BorrowingDetail>
            {
                new BorrowingDetail { Fine = 10, Status = "Status1" },
                new BorrowingDetail { Fine = 20, Status = "Status1" },
                new BorrowingDetail { Fine = 30, Status = "Status1" }
            };
            _borrowingDetailRepository.CreateRange(borrowingDetails);
            _unitOfWork.SaveChanges();

            // Act
            var totalFine = _borrowingDetailRepository.TotalFine();

            // Assert
            Assert.That(totalFine, Is.EqualTo(60));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
            _unitOfWork.Dispose();
        }
    }
}
