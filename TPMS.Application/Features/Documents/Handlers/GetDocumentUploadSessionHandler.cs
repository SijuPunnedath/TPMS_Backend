using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Documents.Handlers;


    public class GetDocumentUploadSessionHandler
        : IRequestHandler<GetDocumentUploadSessionQuery, DocumentUploadSessionDto>
    {
        private readonly TPMSDBContext _db;
        private readonly IOwnerTypeCacheService _ownerTypeCache;

        public GetDocumentUploadSessionHandler(
            TPMSDBContext db,
            IOwnerTypeCacheService ownerTypeCache)
        {
            _db = db;
            _ownerTypeCache = ownerTypeCache;
        }

        public async Task<DocumentUploadSessionDto> Handle(
            GetDocumentUploadSessionQuery request,
            CancellationToken cancellationToken)
        {
            var session = await _db.DocumentUploadSessions
                .FirstOrDefaultAsync(s => s.SessionId == request.SessionId, cancellationToken);

            if (session == null)
                throw new KeyNotFoundException($"Upload session '{request.SessionId}' not found.");

            // Convert OwnerTypeID → OwnerTypeName using cache
            var ownerTypeName = _ownerTypeCache.GetOwnerTypeName(session.OwnerTypeID);

            return new DocumentUploadSessionDto
            {
                SessionId = session.SessionId,
                FileName = session.FileName,
                OwnerType = ownerTypeName,
                OwnerID = session.OwnerID,
                TotalChunks = session.TotalChunks,
                UploadedChunks = session.UploadedChunks,
                IsCompleted = session.IsCompleted,
                Status = session.Status,
                ErrorMessage = session.ErrorMessage,
                StartedAt = session.StartedAt,
                CompletedAt = session.CompletedAt
            };
        }
    }
