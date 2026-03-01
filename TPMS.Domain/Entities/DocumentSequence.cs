namespace TPMS.Domain.Entities;

public class DocumentSequence
{
    public int Id { get; set; }

    //public int TenantId { get; set; }   // SaaS isolation

    public string ModuleName { get; set; } = default!;
    // "LEASE", "PROPERTY", "TENANT", "INVOICE"

    public string Prefix { get; set; } = default!;
    // LSE, PRO, TEN, INV

    public int CurrentNumber { get; set; }

    public int NumberLength { get; set; } = 5;
    // 00001 format length

    public bool ResetEveryYear { get; set; }

    public int? Year { get; set; }

   // public uint xmin { get; set; }
   // public byte[] RowVersion { get; set; } = default!; // Concurrency
}
