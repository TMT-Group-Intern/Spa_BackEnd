using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using Spa.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IEnumerable<Customer> GetAllCustomer()
        {
            var customers = _customerRepository.GetAllCustomer();
            return customers;
        }

        public async Task CreateCustomer(Customer customer)
        {
            var lastCusID = await GenerateCustomerCodeAsync();

            customer.CustomerCode = lastCusID;
            _customerRepository.CreateCustomers(customer);
        }

        public async Task<string> GenerateCustomerCodeAsync()
        {
            var lastCustomerCode = await _customerRepository.GetLastCustomerAsync();

            if (lastCustomerCode == null)
            {
                return "KH0001";
            }
            var lastCode = lastCustomerCode.CustomerCode;
            int numericPart = int.Parse(lastCode.Substring(2));
            numericPart++;
            return "KH" + numericPart.ToString("D4");
        }

        public async Task UpdateCustomer(long customerId, Customer customer)
        {      
            try {
                var customerFromId = _customerRepository.GetCustomerById(customerId);
                bool checckPhone = await GetCustomerByPhone(customer.Phone, customerId);
                if (checckPhone)
                {
                    throw new DuplicatePhoneNumberException("The phone number already exists in the system.");
                }         
                customerFromId.Gender = customer.Gender;
                customerFromId.FirstName = customer.FirstName;
                customerFromId.LastName = customer.LastName;
                customerFromId.Email = customer.Email;
                customerFromId.Phone = customer.Phone;
                customerFromId.DateOfBirth = customer.DateOfBirth;
               await _customerRepository.UpdateCustomer(customerFromId);              
            }
            catch (DuplicatePhoneNumberException)
            {             
                throw;
            }
            catch (Exception ex)
            {              
                throw new Exception("An error occurred while updating customer", ex); 
            }
        }

        public Customer GetCustomerById(long id)
        {
            return _customerRepository.GetCustomerById(id);
        }

        public bool isExistCustomer(long id)
        {
            return _customerRepository.GetCustomerById(id) == null ? false : true;
        }

        public async Task DeleteCustomer(long customerId)  //Delete cus
        {
            var cusToDelete = GetCustomerById(customerId);
            try {
                await _customerRepository.DeleteCustomer(cusToDelete);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
            { 
               throw new ForeignKeyViolationException("Cannot delete customer because it is referenced by other entities.");
            }      
        }

        public async Task<bool> GetCustomerByPhone(string phone, long id)
        {
            bool flag = true;
            //if (phone == null)
            //{
            //    throw new ArgumentNullException(nameof(phone));
            //}
            //else if (phone.Length == 0)
            //{
            //    throw new ArgumentException("Phone number cannot be empty", nameof(phone));
            //}
            var customer = await _customerRepository.GetCustomerByPhone(phone,  id);
            if (customer == null)
            {
                flag = false;
            }

            //return customer == null ? false:true;

            return flag;
        }


    }
}
