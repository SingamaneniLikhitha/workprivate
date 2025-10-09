<img width="1911" height="847" alt="image" src="https://github.com/user-attachments/assets/badf89da-ca82-4f76-aef3-c63c2ae58ada" />
// --- SUBSCRIBER BRANCH under HL4 (2000D) ---
var subscriberLoop = transactionset.Segments.OfType<Loop>().FirstOrDefault(l => l.SegmentId == "2000D")
                     ?? transactionset.Segments.OfType<Loop>().FirstOrDefault(l => l.SegmentId == "2000C");
if (subscriberLoop != null)
{
    var subscriberNM1loop = subscriberLoop?.Segments.OfType<Loop>().FirstOrDefault(l => l.SegmentId == "2100D");

    // DMG (DOB/Gender) 
    var dobYYYYMMDD = NormalizeToYYYYMMDD(GetCSVValue(record, "DOB"));
    if (!string.IsNullOrEmpty(dobYYYYMMDD))
    {
        var dmg = new DMG();
        dmg.DateOfBirth.FormatQualifier = "D8";
        dmg.DateOfBirth.ParsedValue = dobYYYYMMDD;
        var gender = GetCSVValue(record, "Gender");
        if (!string.IsNullOrEmpty(gender)) dmg.GenderCode = gender;
        subscriberLoop.AddSegment(dmg);
    }

    // NM1 – Subscriber identity (moved after DMG)
    var sNM1 = subscriberNM1loop?.Segments.OfType<NM1>().FirstOrDefault();
    if (sNM1 != null)
    {
        sNM1.Last = GetCSVValue(record, "LN");
        sNM1.First = GetCSVValue(record, "FN");
        sNM1.IdQualifier = "MI";
        sNM1.IdCode = GetCSVValue(record, "MemberID");
    }

    // TRN – should have no PAN in TRN02
    var trn = new TRN();
    trn.TraceTypeCode = "1";
    trn.TraceNumber = ""; // leave blank as per expected output
    subscriberLoop.AddSegment(trn);

    // REF*EJ (PAN)
    var pan = GetCSVValue(record, "PAN");
    if (!string.IsNullOrEmpty(pan))
        AddRefSegment(subscriberLoop, "EJ", pan);

    // AMT*T3
    var amtVal = GetCSVValue(record, "TotalSubmittedCharges");
    if (!string.IsNullOrEmpty(amtVal))
    {
        var amt = new AMT();
        amt.AmountQualifierCode = "T3";
        amt.MonetaryAmount.ParsedValue = amtVal;
        subscriberLoop.AddSegment(amt);
    }

    // Claim Service Date DTP (RD8)
    var from = NormalizeToYYYYMMDD(GetCSVValue(record, "ClaimDateFrom"));
    var to = NormalizeToYYYYMMDD(GetCSVValue(record, "ClaimDateTo"));
    if (!string.IsNullOrEmpty(from))
    {
        var dtp = new DTP { Qualifier = "472" };
        if (!string.IsNullOrEmpty(to))
        {
            dtp.Date.FormatQualifier = "RD8";
            dtp.Date.ParsedValue = $"{from}-{to}";
        }
        else
        {
            dtp.Date.FormatQualifier = "D8";
            dtp.Date.ParsedValue = from;
        }
        subscriberLoop.AddSegment(dtp);
    }

    // SVC (service line) and others remain unchanged
    if (!string.IsNullOrEmpty(GetCSVValue(record, "ProcCodes")))
    {
        var svc = new SVC
        {
            Procedure = new ProcedureIdentifier
            {
                Qualifier = GetCSVValue(record, "ProcCodeQualifier"),
                Code = GetCSVValue(record, "ProcCodes"),
            }
        };
        var lineAmt = GetCSVValue(record, "LineItemChargeAmount");
        if (!string.IsNullOrEmpty(lineAmt)) svc.ChargeAmount.ParsedValue = lineAmt;
        var units = GetCSVValue(record, "ProcUnits");
        if (!string.IsNullOrEmpty(units)) svc.UnitsOfServicePaidCount.ParsedValue = units;
        subscriberLoop.AddSegment(svc);
    }

    AddRefSegment(subscriberLoop, "FJ", GetCSVValue(record, "ServiceLineNumber"));

    var procDate = NormalizeToYYYYMMDD(GetCSVValue(record, "ProcDates"));
    if (!string.IsNullOrEmpty(procDate))
    {
        var procDtp = new DTP { Qualifier = "472" };
        procDtp.Date.FormatQualifier = "D8";
        procDtp.Date.ParsedValue = procDate;
        subscriberLoop.AddSegment(procDtp);
    }

    AddRefSegment(subscriberLoop, "1K", GetCSVValue(record, "ClaimNumber"));
    var ft = GetCSVValue(record, "FacilityType");
    var cs = GetCSVValue(record, "ClaimStatus");
    if (!string.IsNullOrEmpty(ft) && !string.IsNullOrEmpty(cs))
        AddRefSegment(subscriberLoop, "BLT", ft + cs);
    AddRefSegment(subscriberLoop, "LU", ft);
    AddRefSegment(subscriberLoop, "6P", GetCSVValue(record, "Group"));
}

