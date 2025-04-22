using CsvHelper;                             
using CsvHelper.Configuration;               
using CsvHelper.TypeConversion;              
using System;
using System.Globalization;

namespace EmpLoad.Services.Foundations.EmployeeMaps
{
    public class FlexibleDateTimeConverter : DateTimeConverter
    {
        private static readonly string[] Formats = new[]
        {
            "dd/MM/yyyy",   
            "d/M/yyyy",     
            "d/MM/yyyy",    
            "dd/M/yyyy"     
        };

        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return base.ConvertFromString(text, row, memberMapData);
            }

            if (DateTime.TryParseExact(
                    text.Trim(),
                    Formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime date))
            {
                return date;
            }

            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}
