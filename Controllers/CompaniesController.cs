using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CompanyEmployee.Contracts;
using CompanyEmployee.Entities.DataTransferObjects;
using CompanyEmployee.Entities.Models;
using CompanyEmployee.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployee.Controllers
{
    [ApiController]
    [Route("api/v1/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompaniesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges: false);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies); 
            return Ok(companiesDto);
        }

        [HttpGet("{id}", Name = "CompanyById")]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges: false);
            if (company == null)
            {
                // ReSharper disable once HeapView.BoxingAllocation
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var companyDto = _mapper.Map<CompanyDto>(company);
            return Ok(companyDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto company)
        {
            if (company == null)
            {
                _logger.LogError("Company object sent is null");
                return BadRequest("Company object sent is null");
            }

            var companyEntity = _mapper.Map<Company>(company);
             _repository.Company.CreateCompany(companyEntity);
            _repository.Save();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return CreatedAtRoute("CompanyById", new {id = companyToReturn.Id}, companyToReturn);
        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollections([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids cannot be null");
                return BadRequest("Parameter ids cannot be null");
            }

            var companyEntities = await _repository.Company.GetByIdsAsync(ids, trackChanges: false);
            if (ids.Count() != companyEntities.Count())
            {
                _logger.LogError("Some ids are not valid in the collection");
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<CompanyDto>>(companyEntities));
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CreateCompanyDto> companyDtos)
        {
            if (companyDtos == null)
            {
                _logger.LogError("Company collection is null");
                return BadRequest("Company collection is null.");
            }

            var companies = _mapper.Map<IEnumerable<Company>>(companyDtos);
            foreach (var company in companies)
            {
                _repository.Company.CreateCompany(company);
            }
            
            _repository.Save();

            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            var ids = string.Join(",", companiesToReturn.Select(c => c.Id));

            return CreatedAtRoute("CompanyCollection", new {ids}, companiesToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges: false);
            if (company is null)
            {
                // ReSharper disable once HeapView.BoxingAllocation
                _logger.LogInfo($"Company with id: {id} not found.");
                // ReSharper disable once HeapView.BoxingAllocation
                return BadRequest($"Company with id: {id} not found.");
            }
            
            _repository.Company.DeleteCompany(company);
            _repository.Save();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] UpdateCompanyDto company)
        {
            if (company == null)
            {
                _logger.LogError("Company object is null.");
                return BadRequest("Company object is null.");
            }

            var companyEntity = await _repository.Company.GetCompanyAsync(id, trackChanges: true);
            if (companyEntity == null)
            {
                _logger.LogError($"Company with id: {id} not found.");
                return BadRequest($"Company with id: {id} not found.");
            }

            _mapper.Map(company, companyEntity);
            _repository.Save();

            return NoContent();
        }
    }
}