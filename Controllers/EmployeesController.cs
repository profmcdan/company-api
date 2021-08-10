using System;
using System.Collections.Generic;
using AutoMapper;
using CompanyEmployee.Contracts;
using CompanyEmployee.Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                // ReSharper disable once HeapView.BoxingAllocation
                _logger.LogInfo($"Company with id: {companyId}  not found.");
                return NotFound();
            }

            var employees = _repository.Employee.GetEmployees(companyId, trackChanges: false);
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                // ReSharper disable once HeapView.BoxingAllocation
                _logger.LogInfo($"Company with id: {companyId}  not found.");
                return NotFound();
            }

            var employee = _repository.Employee.GetEmployee(companyId, id, trackChanges: false);
            if (employee == null)
            {
                // ReSharper disable once HeapView.BoxingAllocation
                _logger.LogInfo($"Employee with id: {id}  not found.");
                return NotFound();
            }

            return Ok(_mapper.Map<EmployeeDto>(employee));
        }
    }
}