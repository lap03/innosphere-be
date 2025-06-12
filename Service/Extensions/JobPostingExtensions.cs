using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Service.Models.JobPostings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Extensions
{
    public static class JobPostingExtensions
    {
        public static IQueryable<JobPosting> ApplyBaseQuery(this IQueryable<JobPosting> query)
        {
            return query
                .Include(j => j.JobPostingTags)
                    .ThenInclude(jpt => jpt.JobTag)
                .Include(j => j.City)
                .Include(j => j.Employer)
                .Where(j => !j.IsDeleted);
        }

        public static IQueryable<JobPosting> FilterByCity(this IQueryable<JobPosting> query, int? cityId)
        {
            return cityId.HasValue
                ? query.Where(j => j.CityId == cityId.Value)
                : query;
        }

        public static IQueryable<JobPosting> FilterByJobType(this IQueryable<JobPosting> query, string? jobType)
        {
            return !string.IsNullOrEmpty(jobType)
                ? query.Where(j => j.JobType == jobType)
                : query;
        }

        public static IQueryable<JobPosting> FilterByHourlyRateRange(this IQueryable<JobPosting> query, float? minHourlyRate, float? maxHourlyRate)
        {
            if (minHourlyRate.HasValue)
            {
                query = query.Where(j => j.HourlyRate >= minHourlyRate.Value);
            }
            if (maxHourlyRate.HasValue)
            {
                query = query.Where(j => j.HourlyRate <= maxHourlyRate.Value);
            }
            return query;
        }

        public static IQueryable<JobPosting> FilterByStartDate(this IQueryable<JobPosting> query, DateTime? startFrom)
        {
            return startFrom.HasValue
                ? query.Where(j => j.StartTime >= startFrom.Value)
                : query;
        }

        public static IQueryable<JobPosting> FilterByEndDate(this IQueryable<JobPosting> query, DateTime? endTo)
        {
            return endTo.HasValue
                ? query.Where(j => j.EndTime == null || j.EndTime <= endTo.Value)
                : query;
        }

        public static IQueryable<JobPosting> FilterByKeyword(this IQueryable<JobPosting> query, string? keyword)
        {
            return !string.IsNullOrEmpty(keyword)
                ? query.Where(j => j.Title.Contains(keyword) ||
                                  (j.Description != null && j.Description.Contains(keyword)))
                : query;
        }

        public static IQueryable<JobPosting> FilterByStatus(this IQueryable<JobPosting> query, string? status)
        {
            return !string.IsNullOrEmpty(status)
                ? query.Where(j => j.Status == status)
                : query;
        }

        public static IQueryable<JobPosting> ApplyDefaultSorting(this IQueryable<JobPosting> query)
        {
            return query.OrderByDescending(j => j.PostedAt);
        }

        //public static async Task<(IEnumerable<T> Data, int Total)> ToPagedResultAsync<T>(
        //    this IQueryable<JobPosting> query,
        //    JobPostingFilterModel filter,
        //    Func<IEnumerable<JobPosting>, IEnumerable<T>> mapper)
        //{
        //    var total = await query.CountAsync();

        //    var data = await query
        //        .Skip((filter.Page - 1) * filter.PageSize)
        //        .Take(filter.PageSize)
        //        .ToListAsync();

        //    return (mapper(data), total);
        //}
    }
}
