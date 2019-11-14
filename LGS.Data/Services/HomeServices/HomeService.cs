using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGS.Models.Companies;

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
            if (companyId > 0)
            {
                var company = await _context.Companies.Include(x => x.CompanyRatings).FirstOrDefaultAsync(x => x.Id.Equals(companyId));
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
    }

    public interface IHomeService
    {
        Task<List<Company>> GetAllCompanies();
        Task<Company> GetCompanyDetailByCompanyId(int companyId);
        Task SetCompanyRating(float rating, int companyId);
    }
}