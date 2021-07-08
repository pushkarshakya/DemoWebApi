using DemoWebApi.DAL;
using DemoWebApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DemoWebApi.Tests.Controllers
{
    public class ObjectTypeControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ObjectTypeControllerTests(WebApplicationFactory<Startup> webApplicationFactory)
        {
            _client = webApplicationFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IObjectTypeRepository, ObjectTypeRepositoryMock>();
                });
            })
                .CreateClient();
        }

        [Fact]
        public async Task ShouldReturnNonEmptyObjectTypeListWhenDataAvailable()
        {
            var response = await _client.GetAsync($"/ObjectType/GetAll");
            response.EnsureSuccessStatusCode();
            var objectTypes = await Utilities.GetResponseContent<List<ObjectType>>(response);

            Assert.IsType<List<ObjectType>>(objectTypes);
            Assert.NotEmpty(objectTypes);
        }

        [Fact]
        public async Task ShouldReturnObjectTypeForValidId()
        {
            int validId = 1;

            var response = await _client.GetAsync($"/ObjectType/Get/{validId}");
            response.EnsureSuccessStatusCode();
            var objectType = await Utilities.GetResponseContent<ObjectType>(response);

            Assert.Equal(validId, objectType.ObjectTypeId);
        }

        [Fact]
        public async Task ShouldReturnNotFoundStatusForInvalidId()
        {
            int invalidId = -1;

            var response = await _client.GetAsync($"/ObjectType/{invalidId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public async Task ShouldReturnOneForSuccessfulDataInsert()
        {
            var objectType = new ObjectType { ObjectTypeName = "o1", Description = "object1", Level = 1 };
            var content = Utilities.GetRequestContent(objectType);

            var response = await _client.PostAsync($"/ObjectType/Insert", content);
            response.EnsureSuccessStatusCode();

            var result = await Utilities.GetResponseContent<int>(response);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task ShouldReturnBadRequestForNullDataInsert()
        {
            ObjectType objectType = null;
            var content = Utilities.GetRequestContent(objectType);

            var response = await _client.PostAsync($"/ObjectType/Insert", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnInternalServerErrorForInvalidLevel()
        {
            var invalidLevelObjectType = new ObjectType { ObjectTypeName = "o1", Description = "object1", Level = 10 };
            var content = Utilities.GetRequestContent(invalidLevelObjectType);

            var response = await _client.PostAsync($"/ObjectType/Insert", content);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnOneForSuccessfulDataUpdate()
        {
            var objectType = new ObjectType { ObjectTypeId = 1, ObjectTypeName = "new name", Description = "new description", Level = 1 };
            var content = Utilities.GetRequestContent(objectType);

            var response = await _client.PutAsync($"/ObjectType/Update/1", content);
            response.EnsureSuccessStatusCode();

            var result = await Utilities.GetResponseContent<int>(response);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task ShouldReturnNotFoundForNullDataUpdate()
        {
            int invalidObjectTypeId = -1;
            ObjectType objectType = new ObjectType(); ;
            var content = Utilities.GetRequestContent(objectType);

            var response = await _client.PutAsync($"/ObjectType/Update/{invalidObjectTypeId}", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnOneForSuccessfulDataDelete()
        {
            var objectTypeId = 1;

            var response = await _client.DeleteAsync($"/ObjectType/Delete/{objectTypeId}");
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnNotFoundForInvalidIdDataDelete()
        {
            int invalidObjectTypeId = -1;

            var response = await _client.DeleteAsync($"/ObjectType/Delete/{invalidObjectTypeId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}
