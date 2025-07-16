using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Tournament.Api.Controllers;
using Tournament.Shared.Dto;
using Tournament.Core.Request;
using Service.Contracts;
using Tournament.Tests.TestFixtures;

namespace Tournament.Api.Tests.Controllers
{
    public class TournamentDetailsControllerTests : IClassFixture<TournamentDetailsControllerFixture>
    {
        private readonly TournamentDetailsControllerFixture _fixture;

        public TournamentDetailsControllerTests(TournamentDetailsControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllTournamentDetails_ReturnsOk_WhenItemsExist()
        {
            var controller = _fixture.CreateController();

            var dtoList = new List<TournamentDetailsDto>
            {
                new() { Id = 1, Title = "Test", StartDate = new(2025, 8, 1), EndDate = new(2025, 10, 30) }
            };

            var pagedResult = new PagedResult<TournamentDetailsDto>
            {
                Items = dtoList,
                TotalItems = 1,
                PageSize = 10,
                CurrentPage = 1
            };

            _fixture._tournamentDetailsServiceMock
                .Setup(s => s.GetAllAsync(It.IsAny<PagedRequest>(), false, false))
                .ReturnsAsync(pagedResult);

            var result = await controller.GetAllTournamentDetails();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<TournamentDetailsDto>>(ok.Value);
            Assert.Single(returned);
            Assert.Equal("Test", returned.First().Title);
        }

        [Fact]
        public async Task GetAllTournamentDetails_ReturnsNotFound_WhenNoItems()
        {
            var controller = _fixture.CreateController();

            var pagedResult = new PagedResult<TournamentDetailsDto>
            {
                Items = Enumerable.Empty<TournamentDetailsDto>(),
                TotalItems = 0,
                PageSize = 10,
                CurrentPage = 1
            };

            _fixture._tournamentDetailsServiceMock
                .Setup(s => s.GetAllAsync(It.IsAny<PagedRequest>(), false, false))
                .ReturnsAsync(pagedResult);

            var result = await controller.GetAllTournamentDetails();

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetTournamentDetails_ReturnsOk_WhenFound()
        {
            var controller = _fixture.CreateController();

            var dto = new TournamentDetailsDto
            {
                Id = 5,
                Title = "Found",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1)
            };

            _fixture._tournamentDetailsServiceMock
                .Setup(s => s.GetByIdAsync(5, false, false))
                .ReturnsAsync(dto);

            var result = await controller.GetTournamentDetails(5, includeGames: false);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dto, ok.Value);
        }

        [Fact]
        public async Task GetTournamentDetails_ReturnsNotFound_WhenNotExists()
        {
            var controller = _fixture.CreateController();

            _fixture._tournamentDetailsServiceMock
                .Setup(s => s.GetByIdAsync(99, false, false))
                .ReturnsAsync((TournamentDetailsDto)null!);

            var result = await controller.GetTournamentDetails(99, false);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostTournamentDetails_ReturnsCreatedAtAction()
        {
            var controller = _fixture.CreateController();

            var createDto = new TournamentDetailsCreateDto { Title = "New", StartDate = DateTime.Today };
            var createdDto = new TournamentDetailsDto { Id = 8, Title = "New", StartDate = DateTime.Today, EndDate = DateTime.Today };

            _fixture._tournamentDetailsServiceMock
                .Setup(s => s.CreateAsync(createDto))
                .ReturnsAsync(createdDto);

            var result = await controller.PostTournamentDetails(createDto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(controller.GetTournamentDetails), created.ActionName);
            Assert.Equal(createdDto, created.Value);
        }

        [Fact]
        public async Task PutTournamentDetails_ReturnsNoContent()
        {
            var controller = _fixture.CreateController();

            var updateDto = new TournamentDetailsUpdateDto { Title = "Upd", StartDate = DateTime.Today };

            _fixture._tournamentDetailsServiceMock
                .Setup(s => s.UpdateAsync(3, updateDto))
                .Returns(Task.CompletedTask);

            var result = await controller.PutTournamentDetails(3, updateDto);

            Assert.IsType<NoContentResult>(result);
            _fixture._tournamentDetailsServiceMock.Verify(s => s.UpdateAsync(3, updateDto), Times.Once);
        }

        [Fact]
        public async Task PutTournamentDetails_ReturnsBadRequest_WhenModelStateInvalid()
        {
            var controller = _fixture.CreateController();
            controller.ModelState.AddModelError("Title", "required");

            var result = await controller.PutTournamentDetails(1, new TournamentDetailsUpdateDto());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutTournamentDetails_ReturnsNotFound_WhenServiceThrows()
        {
            var controller = _fixture.CreateController();

            var dto = new TournamentDetailsUpdateDto { Title = "x", StartDate = DateTime.Today };

            _fixture._tournamentDetailsServiceMock
                .Setup(s => s.UpdateAsync(20, dto))
                .Throws<KeyNotFoundException>();

            var result = await controller.PutTournamentDetails(20, dto);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTournamentDetails_ReturnsNoContent()
        {
            var controller = _fixture.CreateController();

            _fixture._tournamentDetailsServiceMock
                .Setup(s => s.DeleteAsync(2))
                .Returns(Task.CompletedTask);

            var result = await controller.DeleteTournamentDetails(2);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTournamentDetails_ReturnsNotFound_WhenServiceThrows()
        {
            var controller = _fixture.CreateController();

            _fixture._tournamentDetailsServiceMock
                .Setup(s => s.DeleteAsync(404))
                .Throws<KeyNotFoundException>();

            var result = await controller.DeleteTournamentDetails(404);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task PatchTournamentDetails_ReturnsNoContent()
        {
            var controller = _fixture.CreateController();

            var patch = new JsonPatchDocument<TournamentDetailsUpdateDto>();

            _fixture._tournamentDetailsServiceMock
                .Setup(s => s.PatchAsync(7, patch))
                .Returns(Task.CompletedTask);

            var result = await controller.PatchTournamentDetails(7, patch);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PatchTournamentDetails_ReturnsBadRequest_WhenModelStateInvalid()
        {
            var controller = _fixture.CreateController();
            controller.ModelState.AddModelError("any", "err");

            var result = await controller.PatchTournamentDetails(7, new JsonPatchDocument<TournamentDetailsUpdateDto>());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Search_ReturnsBadRequest_WhenNoFilters()
        {
            var controller = _fixture.CreateController();

            var result = await controller.SearchTournamentsByTitle(null, null, false);

            var bad = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("At least one filter (title or date) must be provided.", bad.Value);
        }

        [Fact]
        public async Task Search_ReturnsOk_WhenServiceReturnsData()
        {
            var controller = _fixture.CreateController();

            var paged = new PagedResult<TournamentDetailsDto>
            {
                Items = new[] { new TournamentDetailsDto { Id = 11, Title = "Cup", StartDate = DateTime.Today, EndDate = DateTime.Today } },
                TotalItems = 1,
                PageSize = 10,
                CurrentPage = 1
            };

            _fixture._tournamentDetailsServiceMock
                .Setup(s => s.SearchAsync(It.IsAny<PagedRequest>(), "Cup", null, false, false))
                .ReturnsAsync(paged);

            var result = await controller.SearchTournamentsByTitle("Cup", null, false);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(paged, ok.Value);
        }
    }
}
