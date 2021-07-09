using DemoWebApi.DAL;
using DemoWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ObjectTypeController : ControllerBase
    {
        private readonly IObjectTypeRepository _objectTypeRepository;
        public ObjectTypeController(IObjectTypeRepository objectTypeRepository)
        {
            _objectTypeRepository = objectTypeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ObjectType>>> GetAll()
        {
            var result = await _objectTypeRepository.GetAllAsync();
            return result != null ? Ok(result) : Ok(new List<ObjectType>());
        }

        [HttpGet("{objectTypeId}")]
        public async Task<ActionResult<ObjectType>> Get(int objectTypeId)
        {
            var result = await _objectTypeRepository.GetAsync(objectTypeId);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Insert(ObjectType objectType)
        {
            var result = await _objectTypeRepository.InsertAsync(objectType);
            return result == 1 ? Ok(result) : BadRequest();
        }

        [HttpPut("{objectTypeId}")]
        public async Task<ActionResult<int>> Update(ObjectType objectType)
        {
            var result = await _objectTypeRepository.UpdateAsync(objectType);
            return result == 1 ? Ok(result) : NotFound();
        }

        [HttpDelete("{objectTypeId}")]
        public async Task<ActionResult> Delete(int objectTypeId)
        {
            var result = await _objectTypeRepository.DeleteAsync(objectTypeId);
            return result == 1 ? Ok() : NotFound();
        }
    }
}
