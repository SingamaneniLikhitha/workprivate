using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.X12;
using Domain.X12.Fields;
using Domain.X12.Loops;
using Domain.X12.Parsers;
using Domain.X12.Segments;
using Optum360.ClaimModels.Claim;

namespace CSVRealtimeParser
{
    class ElgCSVParser
    {
        static async Task Main(string[] args)
        {

            X12Parser x12Parser = new X12Parser();
            X12Message x12Mess;
            GS gsSegment;
            ISA isaSegment;
            IEA ieaSegment;
            GE geSegment;
            InterchangeControlLoop interchangeLoop;
            string defaultHeader = "TxnType,TxnSubType,PAN,Version,RequestType,PatientResponsibilityEstimator,Filler,Filler,Filler,PayerID,MemberID,MemberIDType,MemberIDSuffix,Suffix,LN,FN,MI,DOB,SSN,CardID,CardDate,Gender,RelCode,Group,DependentLN,DependentFN,DependentMI,DependentDOB,DependentGender,DependentSSN,PlanCode,ClientCity,ClientState,ClientCounty,ClientZip,OtherInsurance,MedicalRecordNumber,SecondaryMemberID,DependentSuffix,FamilyBenefitsSearch,ProviderID,ProviderLN,ProviderFN,ProviderState,ProviderNetwork,ProviderIDType,ProviderRole,ProviderSpecialty,Filler,Filler,ServiceProvider1ID,ServiceProvider1IDType,ServiceProvider1LN,ServiceProvider1FN,ServiceProvider1Specialty,ServiceProvider1Address,ServiceProvider1City,ServiceProvider1State,ServiceProvider1Role,ServiceProvider2ID,ServiceProvider2IDType,ServiceProvider2Role,ServiceProvider3ID,ServiceProvider3IDType,ServiceProvider3Role,AmbulanceTransportType,ProviderContactName,ProviderPhone,ProviderPhoneExt,ProviderFax,AmbulanceTransportReason,AmbulanceTransportDistance,ProviderAddress,ProviderCity,ProviderZip,ServiceProvider1Zip,ServiceProvider1Phone,ServiceProvider1PhoneExt,ServiceProvider2Specialty,StartDate,EndDate,ServiceType,FacilityType,ClaimStatus,ProcCodeModifier,AdditionalDates,DateType,ProcCodeQualifier,ProcDates,DiagCodes,ProcCodes,ProcUnits,ProcUnitType,VisitDays,CareType,RequestNumber,Comments,CertPeriod,TotalSubmittedCharges,DocumentControlNumber,DocumentCode,ServiceLineNumber,ServicePriority,AutoAccident,WorkRelated,AnotherPartyResponsible,AccidentState,AccidentCountry,PrognosisCode,SkilledNursingFacilityIndicator,DischargeFacilityType,SurgicalProcCode,LineItemChargeAmount,FacilityPlaceOfServiceQualifier,BillType,LineOfBusiness,ReleaseOfInfo,AmbulanceDistanceQualifier,AdmissionSourceCode,Filler,Filler,Filler,DiagCodeQualifier,DiagDates,SurgicalProcCodeQualifier,OriginalUnitsOfService,Filler,Filler,Filler,ServiceProvider1Network,ServiceProvider2Network,ServiceProvider3Network,ServiceProvider1ContactName,ServiceProvider1ContactPhone,ServiceProvider1ContactPhoneExt,ServiceProvider1ContactFax,Filler,ProviderUserID,ProviderType,ServiceProvider1Type,ServiceProvider2Type,ServiceProvider3Type,ProviderAdditionalID,ProviderAdditionalIDType,ServiceProvider1AdditionalID,ServiceProvider1AdditionalIDType,ServiceProvider2AdditionalID,ServiceProvider2AdditionalIDType,ServiceProvider3AdditionalID,ServiceProvider3AdditionalIDType,ProviderEmail,ServiceProvider1Email,ServiceProvider2Email,ServiceProvider3Email,ProviderAddress2,ServiceProvider1Address2,ServiceProvider2Address2,ServiceProvider3Address2,ServiceProvider1Comments,ServiceProvider2Comments,ServiceProvider3Comments,ProviderEntityType,ServiceProvider1EntityType,ServiceProvider2EntityType,ServiceProvider3EntityType,ServiceProvider2LN,ServiceProvider2FN,ServiceProvider2Address,ServiceProvider2City,ServiceProvider2State,ServiceProvider2Zip,ServiceProvider3LN,ServiceProvider3FN,ServiceProvider3Address,ServiceProvider3City,ServiceProvider3State,ServiceProvider3Zip,RequestingProviderMI,RequestingProviderSuffix,ServiceProviderMI,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,Filler,DependentID,DependentSecondaryIDType,DependentSecondaryID,DependentAddress,DependentAddress2,DependentCity,DependentState,DependentCounty,DependentZip,ClientAddress,ClientAddress2,Filler,Filler,Filler,Filler,Filler,Filler,LocationType,LocationName,LocationAddress,LocationAddress2,LocationCity,LocationState,LocationZip";
            Stopwatch timer = new Stopwatch();
            timer.Start();

            //string csv270File = "C:\\Users\\nvaidyan\\Documents\\Mydocs\\2025\\June\\20\\elg.csv";
            string csv270File = "C:\\Users\\nvaidyan\\Documents\\Mydocs\\2025\\June\\17\\noheader.csv";

            string elgDataSubscriber =
                "ISA*00*          *00*          *ZZ*770545613      *ZZ*770545613      *990101*1200*^*00501*123456789*0*P*:~" +
                "GS*HS*770545613*XXXXXXXXX*20990101*1200*12345*X*005010X279A1~" +
                "ST*270*0001*005010X279A1~" +
                "BHT*0022*13*12345*20290101*1200~" +
                "HL*1**20*1~" +
                "NM1*PR*2*XXXXX*****PI*XXXXX~" +
                "HL*2*1*21*1~" +
                "NM1*1P*2*XXXXX*****XX*XXXXXXX~" +
                "HL*3*2*22*0~" +
                "TRN*1*XXXXXX*9CHGHEALTH~" +
                "NM1*IL*1*XXXXX*XXXXXX****MI*XXXXXXXX~" +
                "SE*18*0001~" +
                "GE*1*12345~" +
                "IEA*1*123456789~";

            string elgDataDependent =
                "ISA*00*          *00*          *ZZ*770545613      *ZZ*770545613      *990101*1200*^*00501*123456789*0*P*:~" +
                "GS*HS*770545613*XXXXXXXXX*20990101*1200*12345*X*005010X279A1~" +
                "ST*270*0001*005010X279A1~" +
                "BHT*0022*13*12345*20290101*1200~" +
                "HL*1**20*1~" +
                "NM1*PR*2*XXXXX*****PI*XXXXX~" +
                "HL*2*1*21*1~" +
                "NM1*1P*2*XXXXX*****XX*XXXXXXX~" +
                "HL*3*2*22*1~" +
                "NM1*IL*1*XXXXX*XXXXXX****MI*XXXXXXXX~" +
                "HL*4*3*23~" +
                "TRN*1*XXXXXX*9CHGHEALTH~" +
                "NM1*03*1*XXXXX*XXXXXX****MI*XXXXXXXX~" +
                "SE*18*0001~" +
                "GE*1*12345~" +
                "IEA*1*123456789~";

                List<string> dependentNodeNames = new List<string>();
                dependentNodeNames.Add("DependentLN");
                dependentNodeNames.Add("DependentFN");
                dependentNodeNames.Add("DependentMI");
                dependentNodeNames.Add("DependentDOB");
                dependentNodeNames.Add("DependentGender");

                string jsonOutput = ParsePipeDelimitedFileToJson(csv270File, defaultHeader);
                using JsonDocument csvJson = JsonDocument.Parse(jsonOutput);

                // Print raw JSON (compact)
                //Console.WriteLine(csvJson.RootElement.GetRawText());

                //Print pretty (indented) JSON
                //string prettyJson = JsonSerializer.Serialize(csvJson.RootElement, new JsonSerializerOptions { WriteIndented = true });
                //Console.WriteLine(prettyJson);

                string currentTimeSSS_FileExt = DateTime.Now.ToString("HHmmssFFF");

                string outEligFile = "C:\\Users\\nvaidyan\\Documents\\Mydocs\\2025\\July\\created-270."
                    + currentTimeSSS_FileExt + ".txt";

                if (csvJson.RootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement record in csvJson.RootElement.EnumerateArray())
                    {
                    // Example: print each record as pretty JSON
                    //string prettyRecord = JsonSerializer.Serialize(record, new JsonSerializerOptions { WriteIndented = true });

                        string elgData = "";
                        string patientType = getPatientType(record, dependentNodeNames);
                        switch (patientType)
                        {
                            case "subscriber":
                                elgData = elgDataSubscriber;
                                break;

                                case "dependent":
                                elgData = elgDataDependent;
                                break;
                        }
                        var x12Data = x12Parser.ParseString(elgData);
                        var interChanges = x12Data.Interchanges;
                        x12Mess = new X12Message();

                        string today_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
                        string today_MMDDYYYY = DateTime.Now.ToString("MMddyyyy");
                        string currentTime = DateTime.Now.ToString("HH:mm");
                        string currentTimeSSS = DateTime.Now.ToString("HHmm");
                        string isaControlNumber = GetControlNumber();
                        string gsControlNumber = GetControlNumber();

                        foreach (var interChange in interChanges)
                        {
                            interchangeLoop = new InterchangeControlLoop();
                            isaSegment = interChange.ISA;
                            ieaSegment = interChange.IEA;

                            foreach (var group in interChange.Groups)
                            {
                                gsSegment = group.GS;
                                geSegment = group.GE;

                                foreach (var transactionset in group.TransactionSets)
                                {
                                    var stSegment = transactionset.Segments.OfType<ST>().FirstOrDefault();
                                    var bhtSegment = transactionset.Segments.OfType<BHT>().FirstOrDefault();

                                    //set ISA
                                    isaSegment.InterchangeDate.ParsedValue = today_YYYYMMDD;
                                    isaSegment.InterchangeTime.ParsedValue = currentTime;
                                    isaSegment.InterchangeControlNumber = isaControlNumber;
                                    //isaSegment.ProductionUsageIndicator = GetX12Value(doc, "Envelope", "ISA15")[0];

                                    //set IEA
                                    ieaSegment.InterchangeControlNumber = isaControlNumber;

                                    //set GS
                                    gsSegment.CreateDate.ParsedValue = today_YYYYMMDD;
                                    gsSegment.CreateTime.ParsedValue = currentTime;
                                    gsSegment.GroupControlNumber = int.Parse(gsControlNumber);

                                    //set GE
                                    geSegment.GroupControlNumber = int.Parse(gsControlNumber);

                                    //set ST

                                    //set BHT
                                    bhtSegment.OriginatorIdentifier = isaControlNumber;
                                    bhtSegment.CreateDate.ParsedValue = today_YYYYMMDD;
                                    bhtSegment.CreateTime.ParsedValue = currentTimeSSS;

                                    //set Payer Loop (2100A)
                                    var payerLoop = transactionset.Segments.OfType<Loop>().FirstOrDefault(l => l.SegmentId == "2000A");
                                    var payerNameLoop = payerLoop.Segments.OfType<Loop>().FirstOrDefault(l => l.SegmentId == "2100A");

                                    var payerNameSegment = payerNameLoop.Segments.OfType<NM1>().FirstOrDefault();
                                    payerNameSegment.Last = GetCSVX12Value(record, "PayerID");
                                    payerNameSegment.IdCode = GetCSVX12Value(record, "PayerID");

                                    //set Provider Loop (2100B)
                                    var providerLoop = payerLoop.Segments.OfType<Loop>().FirstOrDefault(l => l.SegmentId == "2000B");
                                    var providerNameLoop = providerLoop.Segments.OfType<Loop>().FirstOrDefault(l => l.SegmentId == "2100B");
                                    var providerNameSegment = providerNameLoop.Segments.OfType<NM1>().FirstOrDefault();

                                    providerNameSegment.Last = GetCSVX12Value(record, "ProviderLN");

                                    if(!string.IsNullOrEmpty(GetCSVX12Value(record, "ProviderID")))
                                    { 
                                        if(GetCSVX12Value(record, "ProviderID").Length ==9)
                                        {
                                            providerNameSegment.IdQualifier = "FI";
                                        }
                                        providerNameSegment.IdCode = GetCSVX12Value(record, "ProviderID");
                                    }
                                    else
                                    {
                                        providerNameSegment.IdQualifier = "";
                                        providerNameSegment.IdCode = "";
                                    }

                                    /*if (!String.IsNullOrEmpty(GetX12Value(doc, "Provider", "REF01")) &&
                                        !string.IsNullOrEmpty(GetX12Value(doc, "Provider", "REF02")))
                                    {
                                        providerNameLoop.AddSegment(new REF
                                        {
                                            IdQualifier = GetX12Value(doc, "Provider", "REF01"),
                                            IdCode = GetX12Value(doc, "Provider", "REF02")
                                        });
                                    }*/

                                    if (!string.IsNullOrEmpty(GetCSVX12Value(record, "ProviderAddress")))
                                    {
                                        providerNameLoop.AddSegment(new N3
                                        {
                                            Address1 = GetCSVX12Value(record, "ProviderAddress"),
                                            Address2 = GetCSVX12Value(record, "ProviderAddress2")
                                        });
                                    }

                                    if (!string.IsNullOrEmpty(GetCSVX12Value(record, "ProviderCity")))
                                    {
                                        providerNameLoop.AddSegment(new N4
                                        {
                                            City = GetCSVX12Value(record, "ProviderCity"),
                                            State = GetCSVX12Value(record, "ProviderState"),
                                            Zip = GetCSVX12Value(record, "ProviderZip")
                                        });
                                    }

                                    /*if (!string.IsNullOrEmpty(GetX12Value(doc, "Provider", "PRV01")))
                                    {
                                        providerNameLoop.AddSegment(new PRV
                                        {
                                            ProviderCode = GetX12Value(doc, "Provider", "PRV01"),
                                            IdQualifier = GetX12Value(doc, "Provider", "PRV02"),
                                            IdCode = GetX12Value(doc, "Provider", "PRV03")
                                        });
                                    }*/

                                    //set subscriber loop
                                    var subscriberLoop = providerLoop.Segments.OfType<Loop>().FirstOrDefault(l => l.SegmentId == "2000C");
                                    var subscriberHLSegment = subscriberLoop.Segments.OfType<HL>().FirstOrDefault();

                                    var subscriberTRNSegment = subscriberLoop.Segments.OfType<TRN>().FirstOrDefault();
                                    var subscriberNameLoop = subscriberLoop.Segments.OfType<Loop>().FirstOrDefault(l => l.SegmentId == "2100C");
                                    var subscriberName = subscriberNameLoop.Segments.OfType<NM1>().FirstOrDefault();

                                    subscriberName.First = GetCSVX12Value(record, "FN");
                                    subscriberName.Last = GetCSVX12Value(record, "LN");

                                    if (!string.IsNullOrEmpty(GetCSVX12Value(record, "MemberID")))
                                    {
                                        subscriberName.IdCode = GetCSVX12Value(record, "MemberID");
                                    }
                                    else
                                    {
                                        subscriberName.IdQualifier = "";
                                        subscriberName.IdCode = "";
                                    }

                                    if (!String.IsNullOrEmpty(GetCSVX12Value(record, "PAN")))
                                    {
                                        subscriberNameLoop.AddSegment(new REF
                                        {
                                            IdQualifier = "EJ",
                                            IdCode = GetCSVX12Value(record, "PAN")
                                        });
                                    }

                                    if (patientType.ToLower().Equals("subscriber"))
                                    {
                                        subscriberTRNSegment.TraceTypeCode = "1";
                                        subscriberTRNSegment.TraceNumber = GetCSVX12Value(record, "PAN");

                                        if (!string.IsNullOrEmpty(GetCSVX12Value(record, "ClientAddress")))
                                        {
                                            subscriberNameLoop.AddSegment(new N3
                                            {
                                                Address1 = GetCSVX12Value(record, "ClientAddress"),
                                                Address2 = GetCSVX12Value(record, "ClientAddress2")
                                            });
                                        }

                                        if (!string.IsNullOrEmpty(GetCSVX12Value(record, "ClientCity")))
                                        {
                                            subscriberNameLoop.AddSegment(new N4
                                            {
                                                City = GetCSVX12Value(record, "ClientCity"),
                                                State = GetCSVX12Value(record, "ClientState"),
                                                Zip = GetCSVX12Value(record, "ClientZip")
                                            });
                                        }

                                        /*if (!string.IsNullOrEmpty(GetX12Value(doc, "Subscriber", "PRV01")))
                                        {
                                            subscriberNameLoop.AddSegment(new PRV
                                            {
                                                ProviderCode = GetX12Value(doc, "Subscriber", "PRV01"),
                                                IdQualifier = GetX12Value(doc, "Subscriber", "PRV02"),
                                                IdCode = GetX12Value(doc, "Subscriber", "PRV03")
                                            });
                                        }*/

                                        if (!String.IsNullOrEmpty(GetCSVX12Value(record, "DOB")))
                                        {
                                            DMG dmg = new DMG();
                                            dmg.DateOfBirth.FormatQualifier = "D8";
                                            dmg.DateOfBirth.ParsedValue = ConvertMMDDYYYYtoYYYYMMDD(GetCSVX12Value(record, "DOB"));
                                            dmg.GenderCode = GetCSVX12Value(record, "Gender");
                                            subscriberNameLoop.AddSegment(dmg);
                                        }

                                        if (!String.IsNullOrEmpty(GetCSVX12Value(record, "StartDate")) &&
                                            !string.IsNullOrEmpty(GetCSVX12Value(record, "EndDate")))
                                        {
                                            DTP dtp = new DTP();
                                            dtp.Qualifier = "291";
                                            dtp.Date.FormatQualifier = "RD8";
                                            dtp.Date.ParsedValue = ConvertMMDDYYYYtoYYYYMMDD(GetCSVX12Value(record, "StartDate")) +
                                            "-" + ConvertMMDDYYYYtoYYYYMMDD(GetCSVX12Value(record, "EndDate"));
                                            subscriberNameLoop.AddSegment(dtp);
                                        }

                                        /*if (!String.IsNullOrEmpty(GetX12Value(doc, "Subscriber", "INS01")) &&
                                            !string.IsNullOrEmpty(GetX12Value(doc, "Subscriber", "INS02")))
                                        {
                                            INS ins = new INS();
                                            ins.InsuredIndicator = GetX12Value(doc, "Subscriber", "INS01");
                                            ins.IndividualRelationshipCode = GetX12Value(doc, "Subscriber", "INS02");
                                            subscriberNameLoop.AddSegment(ins);
                                        }*/

                                        if (!string.IsNullOrEmpty(GetCSVX12Value(record, "ServiceType")))
                                        {
                                            WeakTypeLoop subscriberBenefitInformationLoop = new WeakTypeLoop("2110C", "Subscriber Benefit");
                                            EQ eQ = new EQ();
                                            eQ.ServiceTypeCode = GetCSVX12Value(record, "ServiceType");
                                            subscriberBenefitInformationLoop.AddSegment(eQ);
                                            subscriberNameLoop.AddSegment(subscriberBenefitInformationLoop);
                                        }

                                    }
                                    else if (patientType.ToLower().Equals("dependent"))
                                    {                                    
                                        //set dependent loop
                                        var dependentLoop = subscriberLoop.Segments.OfType<Loop>().FirstOrDefault(l => l.SegmentId == "2000D");
                                        var dependentHLSegment = dependentLoop.Segments.OfType<HL>().FirstOrDefault();
                                        var dependentTRNSegment = dependentLoop.Segments.OfType<TRN>().FirstOrDefault();
                                        var dependentNameLoop = dependentLoop.Segments.OfType<Loop>().FirstOrDefault(l => l.SegmentId == "2100D");
                                        var dependentName = dependentNameLoop.Segments.OfType<NM1>().FirstOrDefault();

                                        dependentTRNSegment.TraceTypeCode = "1";
                                        dependentTRNSegment.TraceNumber = GetCSVX12Value(record, "PAN");

                                        dependentName.First = GetCSVX12Value(record, "DependentFN");
                                        dependentName.Last = GetCSVX12Value(record, "DependentLN");
                                        dependentName.IdCode = GetCSVX12Value(record, "MemberID");

                                        if (!String.IsNullOrEmpty(GetCSVX12Value(record, "PAN")))
                                        {
                                            dependentNameLoop.AddSegment(new REF
                                            {
                                                IdQualifier = "EJ",
                                                IdCode = GetCSVX12Value(record, "PAN")
                                            });
                                        }

                                        if (!string.IsNullOrEmpty(GetCSVX12Value(record, "DependentAddress")))
                                        {
                                            dependentNameLoop.AddSegment(new N3
                                            {
                                                Address1 = GetCSVX12Value(record, "DependentAddress"),
                                                Address2 = GetCSVX12Value(record, "DependentAddress2")
                                            });
                                        }

                                        if (!string.IsNullOrEmpty(GetCSVX12Value(record, "DependentCity")))
                                        {
                                            dependentNameLoop.AddSegment(new N4
                                            {
                                                City = GetCSVX12Value(record, "DependentCity"),
                                                State = GetCSVX12Value(record, "DependentState"),
                                                Zip = GetCSVX12Value(record, "DependentZip")
                                            });
                                        }

                                        if (!String.IsNullOrEmpty(GetCSVX12Value(record, "DependentDOB")))
                                        {
                                            DMG dmg = new DMG();
                                            dmg.DateOfBirth.FormatQualifier = "D8";
                                            dmg.DateOfBirth.ParsedValue = ConvertMMDDYYYYtoYYYYMMDD(GetCSVX12Value(record, "DependentDOB"));
                                            dmg.GenderCode = GetCSVX12Value(record, "DependentGender");
                                            dependentNameLoop.AddSegment(dmg);
                                        }

                                        if (!String.IsNullOrEmpty(GetCSVX12Value(record, "StartDate")) &&
                                            !string.IsNullOrEmpty(GetCSVX12Value(record, "EndDate")))
                                        {
                                            DTP dtp = new DTP();
                                            dtp.Qualifier = "291";
                                            dtp.Date.FormatQualifier = "RD8";
                                            dtp.Date.ParsedValue = ConvertMMDDYYYYtoYYYYMMDD(GetCSVX12Value(record, "StartDate")) +
                                            "-" + ConvertMMDDYYYYtoYYYYMMDD(GetCSVX12Value(record, "EndDate"));
                                            dependentNameLoop.AddSegment(dtp);
                                        }

                                        if (!string.IsNullOrEmpty(GetCSVX12Value(record, "ServiceType")))
                                        {
                                            WeakTypeLoop dependentBenefitInformationLoop = new WeakTypeLoop("2110D", "Dependent Benefit");
                                            EQ eQ = new EQ();
                                            eQ.ServiceTypeCode = GetCSVX12Value(record, "ServiceType");
                                            dependentBenefitInformationLoop.AddSegment(eQ);
                                            dependentNameLoop.AddSegment(dependentBenefitInformationLoop);
                                        }

                                        /*
                                        if (!String.IsNullOrEmpty(GetX12Value(doc, "Dependent", "INS01")) &&
                                            !string.IsNullOrEmpty(GetX12Value(doc, "Dependent", "INS02")))
                                        {
                                            INS ins = new INS();
                                            ins.InsuredIndicator = GetX12Value(doc, "Dependent", "INS01");
                                            ins.IndividualRelationshipCode = GetX12Value(doc, "Dependent", "INS02");
                                            dependentNameLoop.AddSegment(ins);
                                        }
                                    */
                                    }

                                        string x12String = isaSegment.ToString() +
                                                        gsSegment.ToString() +
                                                        transactionset.ToString() +
                                                        geSegment.ToString() +
                                                        ieaSegment.ToString() + "\r\n";

                                        await File.AppendAllTextAsync(outEligFile, x12String, Encoding.UTF8);
                                        }
                                    }
                                }
                            }
                }

            timer.Stop();
            Console.WriteLine("Time taken in milliseconds to parse CSV and create 270 X12 data:" + timer.ElapsedMilliseconds);
        }
        private static string getPatientType(JsonElement record, List<string> dependentNodeNames)
        {            
            string patientType = "subscriber";

            foreach (var str in dependentNodeNames)
            {
                if(!string.IsNullOrEmpty(GetCSVX12Value(record, str)))
                {
                    patientType = "dependent";
                    break;
                }
            }
            return patientType;
        }

        private static string? ConvertMMDDYYYYtoYYYYMMDD(string? mmddyyyy)
        {
            if (string.IsNullOrWhiteSpace(mmddyyyy) || mmddyyyy.Length != 8)
                return mmddyyyy;

            if (DateTime.TryParseExact(mmddyyyy, "MMddyyyy", null, System.Globalization.DateTimeStyles.None, out var dt))
                return dt.ToString("yyyyMMdd");

            return mmddyyyy;
        }
        private static string? GetControlNumber()
        {
            var random = new Random();
            int randomNineDigit = random.Next(100_000_000, 1_000_000_000); // Range: 100000000 to 999999999
            return randomNineDigit.ToString();
        }
        private static string? GetCSVX12Value(JsonElement jsonElement, string node)
        {
            string x12ElementValue = "";

            if (!string.IsNullOrEmpty(node))
            {
                try
                {
                        x12ElementValue = jsonElement.GetProperty(node).GetString();
                }
                catch (KeyNotFoundException knfe)
                {
                    //log it
                }
            }
            return x12ElementValue;
        }
        private static string ParsePipeDelimitedFileToJson(string filePath, string defaultHeader)
        {
            var lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
                return "";

            int startLine = 1;
            var headers = lines[0].Split(',');

            if(!headers.Contains("TxnType", StringComparer.OrdinalIgnoreCase)) //no header, use default value
            {
                headers = defaultHeader.Split(",");
                startLine = 0;
            }
            
            var records = new List<Dictionary<string, string>>();

            for (int i = startLine; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                var record = new Dictionary<string, string>();
                for (int j = 0; j < headers.Length && j < values.Length; j++)
                {
                    record[headers[j]] = values[j];
                }
                records.Add(record);
            }
            string jsonOutput = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
            return jsonOutput;
        }
    }
}