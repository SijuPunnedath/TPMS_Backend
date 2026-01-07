using TPMS.Domain.Entities;
using TPMS.Domain.Enums;

namespace TPMS.Domain.Guards;

public class LeaseGuard
{
    public static void Validate(Lease lease)
    {
        if (lease.StartDate >= lease.EndDate)
            throw new InvalidOperationException("Lease EndDate must be after StartDate.");

        switch (lease.LeaseType)
        {
            case LeaseType.Inbound:
                if (!lease.LandlordID.HasValue)
                    throw new InvalidOperationException("Inbound lease requires LandlordID.");

                lease.TenantID = null; // enforce rule
                break;

            case LeaseType.Outbound:
                if (!lease.TenantID.HasValue)
                    throw new InvalidOperationException("Outbound lease requires TenantID.");

                lease.LandlordID = null; // enforce rule
                break;

            default:
                throw new InvalidOperationException("Invalid LeaseType.");
        }
    }
}