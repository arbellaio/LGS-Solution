using LGS.Models.Companies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LGS.Helpers.Ratings
{
    public static class RatingsHelper
    {
        public static float CalculateRating(List<CompanyRating> companyRatings)
        {
            if (companyRatings != null && companyRatings.Count > 0)
            {
                var totalNoOfRatings = companyRatings.Count();
                var sumofRatings = companyRatings.Sum(x => x.Rating);
                try
                {
                    var calculatedRating = sumofRatings / totalNoOfRatings;
                    return calculatedRating;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return 0;
        }
    }
}