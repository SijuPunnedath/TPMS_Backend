using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Documents.Commands;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Queries;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadDocumentDto dto)
        {
            var result = await _mediator.Send(new UploadDocumentCommand(dto));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetDocumentByIdQuery(id));
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetByOwner([FromQuery] GetDocumentsByOwnerQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDocumentCommand command)
        {
            if (id != command.DocumentID) return BadRequest();
            var success = await _mediator.Send(command);
            return success ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteDocumentCommand(id));
            return success ? Ok() : NotFound();
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveDocuments([FromQuery] GetActiveDocumentsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetDocumentHistory([FromQuery] GetDocumentVersionHistoryQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDeleteDocument(int id)
        {
            var success = await _mediator.Send(new SoftDeleteDocumentCommand { DocumentID = id });
            if (!success) return NotFound();
            return Ok(new { message = "Document soft-deleted successfully." });
        }

        [HttpPut("{id}/restore")]
        public async Task<IActionResult> RestoreDocument(int id)
        {
            var success = await _mediator.Send(new RestoreDocumentCommand { DocumentID = id });
            if (!success) return NotFound();
            return Ok(new { message = "Document restored successfully." });
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedDocuments()
        {
            var result = await _mediator.Send(new GetDeletedDocumentsQuery());
            return Ok(result);
        }

        [HttpGet("GetDochistory")]
        public async Task<IActionResult> GetHistory([FromQuery] string ownerType, [FromQuery] int ownerId, [FromQuery] string? documentType)
        {
            var result = await _mediator.Send(new GetDocumentHistoryQuery
            {
                OwnerType = ownerType,
                OwnerID = ownerId,
                DocumentTypeName = documentType
            });

            return Ok(result);
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery] int? documentId, [FromQuery] string? ownerType, [FromQuery] int? ownerId, [FromQuery] string? documentType, [FromQuery] string? version)
        {
            var result = await _mediator.Send(new DownloadDocumentQuery
            {
                DocumentID = documentId,
                OwnerType = ownerType,
                OwnerID = ownerId,
                DocumentTypeName = documentType,
                Version = version
            });

            return File(result.FileBytes, result.ContentType, result.FileName);
        }

        [HttpGet("access-history")]
        public async Task<IActionResult> GetAccessHistory(
    [FromQuery] int? documentId,
    [FromQuery] string? accessedBy,
    [FromQuery] DateTime? fromDate,
    [FromQuery] DateTime? toDate)
        {
            var query = new GetDocumentAccessHistoryQuery
            {
                DocumentID = documentId,
                AccessedBy = accessedBy,
                FromDate = fromDate,
                ToDate = toDate
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

      /*  [HttpPost("upload-chunk")]
        public async Task<IActionResult> UploadChunk([FromForm] UploadChunkDto dto)
        {
            await _mediator.Send(new UploadDocumentChunkCommand(dto));
            return Ok(new { message = $"Chunk {dto.ChunkNumber} uploaded successfully." });
        } */
        
      [HttpPost("upload/chunk")]
      public async Task<IActionResult> UploadChunk([FromForm] UploadChunkDto dto)
      {
          await _mediator.Send(new UploadDocumentChunkCommand(dto));
          return Ok(new { message = "Chunk uploaded", sessionId = dto.SessionId });
      }

      [HttpPost("upload/finalize/{sessionId:guid}")]
      public async Task<IActionResult> FinalizeUpload(Guid sessionId)
      {
          var doc = await _mediator.Send(new FinalizeDocumentUploadCommand(sessionId));
          return Ok(doc);
      }
     
      
     [HttpGet("upload/session/{sessionId:guid}")]
      public async Task<IActionResult> GetSessionStatus(Guid sessionId)
      {
          var session = await _mediator.Send(new GetDocumentUploadSessionQuery(sessionId));
          if (session == null) return NotFound();
          return Ok(session);
      } 
      
      [HttpGet("tree/full")]
      public async Task<IActionResult> GetFullDocumentTree()
      {
          var result = await _mediator.Send(new GetFullDocumentTreeQuery());
          return Ok(result);
      }
      
      /// <summary>
      /// Stream-based document upload (supports large files up to 1 GB)
      /// </summary>
      
      //[Authorize]
      [HttpPost("upload-stream")]
      [DisableRequestSizeLimit]
      [RequestSizeLimit(1024L * 1024 * 1024)] // 1 GB
      [RequestFormLimits(
          MultipartBodyLengthLimit = 1024L * 1024 * 1024
      )]
      public async Task<IActionResult> UploadDocumentStream(
          [FromForm] UploadDocumentStremDto dto,
          CancellationToken cancellationToken)
      {
          // Basic validation
          if (dto.File == null || dto.File.Length == 0)
              return BadRequest("File is required.");

          // Call your handler
          var result = await _mediator.Send(
              new UploadDocumentStremCommand(dto),
              cancellationToken);

          return Ok(result);
      }
      
      [HttpGet("{documentId:int}/download")]
      public async Task<IActionResult> Download(int documentId)
      {
          var result = await _mediator.Send(
              new DownloadDocumentByIdQuery(documentId));

          // STREAMING RESPONSE
          return File(
              result.Stream,
              result.ContentType,
              result.FileName,
              enableRangeProcessing: true // allows resume / large files
          );
      }
      
      
      //-- List Docs
      [HttpGet]
      public async Task<IActionResult> GetDocuments(
          [FromQuery] int ownerTypeId,
          [FromQuery] int ownerId,
          [FromQuery] int? documentCategoryId,
          [FromQuery] int? documentTypeId,
          [FromQuery] bool includeArchived = false)
      {
          var result = await _mediator.Send(new GetDocumentsQuery
          {
              OwnerTypeID = ownerTypeId,
              OwnerID = ownerId,
              DocumentCategoryID = documentCategoryId,
              DocumentTypeID = documentTypeId,
              IncludeArchived = includeArchived
          });

          return Ok(result);
      }
      
      [HttpGet("{documentId:int}/view")]
      public async Task<IActionResult> ViewDocument(int documentId)
      {
          var result = await _mediator.Send(
              new ViewDocumentQuery { DocumentID = documentId });

          return File(
              result.Stream,
              result.ContentType,
              enableRangeProcessing: true); // 🔥 streaming support
      }
      
      /// <summary>
      /// Get missing documents for an entity (Tenant / Lease / Asset)
      /// </summary>
      [HttpGet("missing")]
      public async Task<IActionResult> GetMissingDocuments(
          [FromQuery] int ownerTypeId,
          [FromQuery] int ownerId)
      {
          if (ownerTypeId <= 0 || ownerId <= 0)
              return BadRequest("Invalid ownerTypeId or ownerId.");

          var query = new GetMissingDocumentsQuery(ownerTypeId, ownerId);

          var result = await _mediator.Send(query);

          return Ok(result);
      }

      /// <summary>
      /// Get document compliance percentage for an entity
      /// </summary>
      [HttpGet("compliance")]
      public async Task<IActionResult> GetCompliance(
          [FromQuery] int ownerTypeId,
          [FromQuery] int ownerId)
      {
          if (ownerTypeId <= 0 || ownerId <= 0)
              return BadRequest("Invalid ownerTypeId or ownerId.");

          var result = await _mediator.Send(
              new GetDocumentComplianceQuery(ownerTypeId, ownerId));

          return Ok(result);
      }
      
      [HttpPost("upload-v2")]
      public async Task<IActionResult> UploadV2(
          [FromForm] UploadDocumentStremDto dto)
      {
          var result = await _mediator.Send(
              new UploadDocumentStreamV2Command
              {
                  Document = dto
              });

          return Ok(result);
      }

    }
}
