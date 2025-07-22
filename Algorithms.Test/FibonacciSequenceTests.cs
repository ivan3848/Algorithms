using AutoMapper;
using EBM.Contract.Repository;
using EBM.Entity.Context;
using EBM.Entity.Models;
using EBM.Entity.Request.Brands;
using EBM.Entity.ViewModels;
using EBM.Service.Services;
using EBM.Service.Validations;
using Moq;
using System.Net;

namespace EBM.Service.Tests
{
    public class BrandServiceTests
    {
        public Mock<IBrandRepository> BrandRepoMock { get; } = new();
        public Mock<IProductRepository> ProductRepoMock { get; } = new();
        public Mock<IMapper> MapperMock { get; } = new();
        private readonly Mock<IAuditExecutionContext> AuditExecutionContextMock = new();


        public BrandService CreateService()
        {
            return new BrandService(
                BrandRepoMock.Object,
                ProductRepoMock.Object,
                MapperMock.Object,
                AuditExecutionContextMock.Object
            );
        }

        [Fact(DisplayName = "Insert: Throws ServiceValidation if brand name already exists and is active")]
        public async Task Insert_BrandNameExistsAndActive_ThrowsServiceValidation()
        {
            // Arrange
            var request = new InsertBrandRequest { BrandName = "Marca X" };
            var existing = new Brand { IsActive = true, BrandName = "UnitTest" };
            BrandRepoMock
                .Setup(r => r.GetEntityBy(It.IsAny<System.Linq.Expressions.Expression<Func<Brand, bool>>>(), null))
                .ReturnsAsync(existing);

            var service = CreateService();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ServiceValidation>(() => service.Insert(request));
            Assert.Equal("Ya existe un registro con este nombre", ex.Message);
            Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        }

        [Fact(DisplayName = "Insert: Reactivates brand if exists and is inactive")]
        public async Task Insert_BrandNameExistsAndInactive_ActivatesBrand()
        {
            // Arrange
            var request = new InsertBrandRequest { BrandName = "Marca Y" };
            var existing = new Brand { BrandId = 5, IsActive = false, BrandName = "UnitTest" };
            var activated = new Brand { BrandId = 5, IsActive = true, BrandName = "UnitTest" };
            var viewModel = new BrandViewModel { BrandName = "UnitTest" };

            BrandRepoMock
                .Setup(r => r.GetEntityBy(It.IsAny<System.Linq.Expressions.Expression<Func<Brand, bool>>>(), null))
                .ReturnsAsync(existing);
            BrandRepoMock
                .Setup(r => r.Activate(existing.BrandId))
                .ReturnsAsync(activated);
            MapperMock
                .Setup(m => m.Map<BrandViewModel>(activated))
                .Returns(viewModel);

            var service = CreateService();

            // Act
            var result = await service.Insert(request);

            // Assert
            Assert.NotNull(result);
            BrandRepoMock.Verify(r => r.Activate(existing.BrandId), Times.Once);
        }

        [Fact(DisplayName = "Insert: Adds brand if name does not exist")]
        public async Task Insert_BrandNameDoesNotExist_AddsBrand()
        {
            // Arrange
            var request = new InsertBrandRequest { BrandName = "Marca Z" };
            var brand = new Brand { BrandName = request.BrandName };
            var viewModel = new BrandViewModel { BrandName = "UnitTest" };

            BrandRepoMock
                .Setup(r => r.GetEntityBy(It.IsAny<System.Linq.Expressions.Expression<Func<Brand, bool>>>(), null))
                .ReturnsAsync((Brand?)null);
            MapperMock
                .Setup(m => m.Map<Brand>(request))
                .Returns(brand);
            BrandRepoMock
                .Setup(r => r.Add(brand))
                .ReturnsAsync(brand);
            MapperMock
                .Setup(m => m.Map<BrandViewModel>(brand))
                .Returns(viewModel);

            var service = CreateService();

            // Act
            var result = await service.Insert(request);

            // Assert
            Assert.NotNull(result);
            BrandRepoMock.Verify(r => r.Add(brand), Times.Once);
        }

        [Fact(DisplayName = "Update: Throws ServiceValidation if another brand with same name exists and is active")]
        public async Task Update_BrandNameExists_ThrowsServiceValidation()
        {
            // Arrange
            var request = new UpdateBrandRequest { BrandId = 1, BrandName = "Marca Q" };
            BrandRepoMock
                .Setup(r => r.Validate(It.IsAny<System.Linq.Expressions.Expression<Func<Brand, bool>>>()))
                .ReturnsAsync(true);

            var service = CreateService();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ServiceValidation>(() => service.Update(request));
            Assert.Equal("Ya existe un registro con este nombre", ex.Message);
            Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        }

        [Fact(DisplayName = "Update: Updates brand if no duplicate name exists")]
        public async Task Update_BrandNameDoesNotExist_UpdatesBrand()
        {
            // Arrange
            var request = new UpdateBrandRequest { BrandId = 2, BrandName = "Marca W" };
            var brand = new Brand { BrandName = "UnitTest" };
            var viewModel = new BrandViewModel { BrandName = "UnitTest" };

            BrandRepoMock
                .Setup(r => r.Validate(It.IsAny<System.Linq.Expressions.Expression<Func<Brand, bool>>>()))
                .ReturnsAsync(false);
            MapperMock
                .Setup(m => m.Map<Brand>(request))
                .Returns(brand);
            BrandRepoMock
                .Setup(r => r.Update(brand))
                .ReturnsAsync(brand);
            MapperMock
                .Setup(m => m.Map<BrandViewModel>(brand))
                .Returns(viewModel);

            var service = CreateService();

            // Act
            var result = await service.Update(request);

            // Assert
            Assert.NotNull(result);
            BrandRepoMock.Verify(r => r.Update(brand), Times.Once);
        }

        [Fact(DisplayName = "Remove: Throws ServiceValidation if brand has associated products")]
        public async Task Remove_BrandHasProducts_ThrowsServiceValidation()
        {
            // Arrange
            var brandId = 10;
            ProductRepoMock
                .Setup(r => r.Validate(It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>()))
                .ReturnsAsync(true);

            var service = CreateService();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ServiceValidation>(() => service.Remove(brandId));
            Assert.Equal("Esta marca tiene productos asociados, no se puede eliminar", ex.Message);
            Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        }

        [Fact(DisplayName = "Remove: Deactivates brand if no associated products")]
        public async Task Remove_BrandNoProducts_DeactivatesBrand()
        {
            // Arrange
            var brandId = 11;
            var brand = new Brand { BrandId = brandId, BrandName = "UnitTest" };
            ProductRepoMock
                .Setup(r => r.Validate(It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>()))
                .ReturnsAsync(false);
            BrandRepoMock
                .Setup(r => r.Deactivate(brandId))
                .ReturnsAsync(brand);

            var service = CreateService();

            // Act
            var result = await service.Remove(brandId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(brandId, result.BrandId);
            BrandRepoMock.Verify(r => r.Deactivate(brandId), Times.Once);
        }
    }
}
