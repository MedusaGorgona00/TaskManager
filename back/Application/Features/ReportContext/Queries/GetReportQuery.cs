using Application.Common.Interfaces;
using Application.Exceptions;
using Application.Features.ReportContext.Dto;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqExtensions;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Features.ReportContext.Queries
{
    public class GetReportQuery : IRequest<IList<ReportDto>>
    {
        /// <summary>
        /// JobTypeRefId
        /// </summary>
        public int? JobTypeRefId { get; set; }

        /// <summary>
        /// SubmitDateFrom
        /// </summary>
        public DateTime? SubmitDateFrom { get; set; }

        /// <summary>
        /// SubmitDateTo
        /// </summary>
        public DateTime? SubmitDateTo { get; set; }

        /// <summary>
        /// JobDescription
        /// </summary>
        public string? JobDescription { get; set; }
    }

    public class GetBranchQueryHandler : IRequestHandler<GetReportQuery, IList<ReportDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetBranchQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<ReportDto>> Handle(GetReportQuery query, CancellationToken cancellationToken)
        {
            //1st option to migrate sql-script to EF LINQ-query
            var lst1 = await _context.Notifications
                .Include(x => x.Execution)
                .ThenInclude(x => x.Job)
                .Where(n => n.Id == _context.Notifications
                    .Where(nn => nn.ExecutionId == n.ExecutionId)
                    .GroupBy(nn => nn.ExecutionId)
                    .Select(g => g.Max(nn => nn.Id))
                    .FirstOrDefault())
                .WhereIfParamNotNull(query.JobTypeRefId, x => query.JobTypeRefId == x.Execution.Job.JobTypeId)
                .WhereIfParamNotNull(query.SubmitDateFrom, x => x.Execution.CreatedDate >= query.SubmitDateFrom)
                .WhereIfParamNotNull(query.SubmitDateTo, x => x.Execution.CreatedDate <= query.SubmitDateTo)
                .WhereIfParamNotNull(query.JobDescription, x => query.JobDescription == x.Execution.Job.Description)
                .Select(n => new ReportDto
                {
                    JobId = n.Execution.JobId,
                    NotificationId = n.Id,
                    JobCreatedDate = n.Execution.CreatedDate,
                    JobEndDate = n.Execution.FinishedAt,
                    Text = n.Text,
                    NotificationCreatedDate = n.CreatedDate,
                    JobTypeId = n.Execution.Job.JobTypeId,
                    Description = n.Execution.Job.Description,
                    Message = (_context.ExecutionHistories
                        .Where(eh => eh.ExecutionId == n.ExecutionId)
                        .Select(eh => eh.Message)
                        .FirstOrDefault())
                })
                .ToListAsync(cancellationToken);

            //2nd option to migrate sql-script to EF LINQ-query
            var lst2 = from n in _context.Notifications
                join e in _context.Executions on n.ExecutionId equals e.Id
                join j in _context.Jobs on e.JobId equals j.Id
                       where n.Id == _context.Notifications
                                 .Where(nn => nn.ExecutionId == e.Id)
                                 .GroupBy(nn => nn.ExecutionId)
                                 .Select(g => g.Max(nn => nn.Id))
                                 .FirstOrDefault()
                             && (query.JobTypeRefId == null || query.JobTypeRefId == j.JobTypeId)
                             && (query.SubmitDateFrom == null || e.CreatedDate >= query.SubmitDateFrom)
                             && (query.SubmitDateTo == null || e.CreatedDate <= query.SubmitDateTo)
                             && (query.JobDescription == null || query.JobDescription == j.Description)
                       select new
                {
                    JobId = j.Id,
                    NotificationId = n.Id,
                    JobCreatedDate = e.StartedAt,
                    JobEndDate = e.FinishedAt,
                    n.Text,
                    NotificationCreatedDate = n.CreatedDate,
                    JobTypeId = j.JobTypeId,
                    j.Description,
                    Notification = _context.ExecutionHistories
                        .Where(a => a.ExecutionId == e.Id)
                        .Select(a => a.Message)
                        .FirstOrDefault()
                };

            var result = await lst2
                .Select(n => new ReportDto
                {
                    JobId = n.JobId,
                    NotificationId = n.NotificationId,
                    JobCreatedDate = n.JobCreatedDate,
                    JobEndDate = n.JobEndDate,
                    Text = n.Text,
                    NotificationCreatedDate = n.NotificationCreatedDate,
                    JobTypeId = n.JobTypeId,
                    Description = n.Description,
                    Message = n.Notification
                })
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
