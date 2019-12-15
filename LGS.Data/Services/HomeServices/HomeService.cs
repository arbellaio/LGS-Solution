using LGS.Models.Communication;
using LGS.Models.Companies;
using LGS.Models.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace LGS.Data.Services.HomeServices
{
    public class HomeService : IHomeService
    {
        #region AppDbContext

        private readonly ApplicationDbContext _context;

        public HomeService(ApplicationDbContext context)
        {
            _context = context ?? throw new NullReferenceException("Context");
        }

        #endregion

        public async Task<List<Company>> GetAllCompanies()
        {
            var companies = await _context.Companies.ToListAsync();
            return companies;
        }

        public async Task<Company> GetCompanyDetailByCompanyId(int companyId)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            if (companyId > 0)
            {
                var company = await _context.Companies.Include(x => x.CompanyRatings).Include(x => x.CustomerReviews).FirstOrDefaultAsync(x => x.Id.Equals(companyId));
                return company;
            }

            return null;
        }

        public async Task SetCompanyRating(float rating, int companyId)
        {
            var companyRating = new CompanyRating
            {
                CompanyId = companyId,
                Rating = rating,
            };
            _context.CompanyRatings.AddOrUpdate(companyRating);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SendMessage(Customer newCustomer, CustomerMessage customerMessage)
        {
            if (newCustomer != null && customerMessage != null)
            {
                var oldCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Email.Equals(newCustomer.Email));
                if (oldCustomer != null)
                {
                    newCustomer.Id = oldCustomer.Id;
                }
                _context.Customers.AddOrUpdate(newCustomer);
                _context.CustomerMessages.AddOrUpdate(customerMessage);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> SendReview(Customer newCustomer, CustomerReview customerReview)
        {
            if (newCustomer != null && customerReview != null)
            {
                var oldCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Email.Equals(newCustomer.Email));
                if (oldCustomer != null)
                {
                    newCustomer.Id = oldCustomer.Id;
                }
                _context.Customers.AddOrUpdate(newCustomer);
                _context.CustomerReviews.AddOrUpdate(customerReview);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

     
    }

    public interface IHomeService
    {
        Task<List<Company>> GetAllCompanies();
        Task<Company> GetCompanyDetailByCompanyId(int companyId);
        Task SetCompanyRating(float rating, int companyId);
        Task<bool> SendMessage(Customer customer, CustomerMessage customerMessage);
        Task<bool> SendReview(Customer newCustomer, CustomerReview customerReview);
    }
}