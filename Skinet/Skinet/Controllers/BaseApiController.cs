using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Skinet.RequestHelpers;

namespace Skinet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController: ControllerBase
    {
        protected async Task<ActionResult> CreatePagedResult<T>(
            IGenericRepository<T> repo,
            ISpecification<T> specification,
            int pagedIndex,
            int pageSize) where T:BaseEntity
        {
            var items = await repo.ListAsync(specification);
            var count= await repo.CountAsync(specification);

            var pagination=new Pagination<T>(pagedIndex, pageSize,count,items);
            return Ok(pagination);
        }
    }
}
