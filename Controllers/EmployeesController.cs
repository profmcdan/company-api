using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CompanyEmployee.Contracts;
using CompanyEmployee.Entities.DataTransferObjects;
using CompanyEmployee.Entities.Models;
using CompanyEmployee.Entities.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CompanyEmployee.Controllers
{
    [ApiController]
    [Route("api/v1/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            if (!employeeParameters.AgeRangeIsValid()) 
                return BadRequest("Max age can't be less than min age.");
            
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                // ReSharper disable once HeapView.BoxingAllocation
                _logger.LogInfo($"Company with id: {companyId}  not found.");
                return NotFound();
            }

            var employees = await _repository.Employee.GetEmployeesAsync(companyId, 
                employeeParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(employees.MetaData));
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
        }

        [HttpGet("{id}", Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                // ReSharper disable once HeapView.BoxingAllocation
                _logger.LogInfo($"Company with id: {companyId}  not found.");
                return NotFound();
            }

            var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges: false);
            if (employee == null)
            {
                // ReSharper disable once HeapView.BoxingAllocation
                _logger.LogInfo($"Employee with id: {id}  not found.");
                return NotFound();
            }

            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, 
            [FromBody] CreateEmployeeDto employee)
        {
            if (employee == null)
            {
                _logger.LogError("Employee object is null");
                return BadRequest("Employee object is null");
            }

            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company is null)
            {
                // ReSharper disable once HeapView.BoxingAllocation
                _logger.LogInfo($"Company with id: {companyId} not found.");
                // ReSharper disable once HeapView.BoxingAllocation
                return BadRequest($"Company with id: {companyId} not found.");
            }

            var employeeEntity = _mapper.Map<Employee>(employee);
            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repository.SaveAsync();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return CreatedAtRoute("GetEmployeeForCompany", 
                new {companyId, id = employeeToReturn.Id}, employeeToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company is null)
            {
                // ReSharper disable once HeapView.BoxingAllocation
                _logger.LogInfo($"Company with id: {companyId} not found.");
                // ReSharper disable once HeapView.BoxingAllocation
                return BadRequest($"Company with id: {companyId} not found.");
            }

            var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges: false);
            if (employee == null)
            {
                // ReSharper disable once HeapView.BoxingAllocation
                _logger.LogInfo($"Employee with id: {id} not found.");
                return NotFound();
            }
            
            _repository.Employee.DeleteEmployee(employee);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] UpdateEmployeeDto employee)
        {
            if (employee == null)
            {
                _logger.LogError("Employee object is null.");
                return BadRequest("Employee object is null.");
            }

            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogError($"Company with id: {companyId} is not found.");
                return BadRequest($"Company with id: {companyId} is not found.");
            }

            var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges: false);
            if (employeeEntity == null)
            {
                _logger.LogError($"Employee with id: {id} is not found.");
                return BadRequest($"Employee with id: {id} is not found.");
            }

            _mapper.Map(employee, employeeEntity);
             await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
            [FromBody] JsonPatchDocument<UpdateEmployeeDto> patchDocument)
        {
            if (patchDocument == null)
            {
                _logger.LogError("Patch Doc object sent is null.");
                return BadRequest("Patch doc object sent is null.");
            }

            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogError($"Company with id: {companyId} is not found.");
                return BadRequest($"Company with id: {companyId} is not found.");
            }

            var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges: true);
            if (employeeEntity == null)
            {
                _logger.LogError($"Employee with id: {id} is not found.");
                return BadRequest($"Employee with id: {id} is not found.");
            }

            var employeeToPatch = _mapper.Map<UpdateEmployeeDto>(employeeEntity);
            patchDocument.ApplyTo(employeeToPatch);
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}