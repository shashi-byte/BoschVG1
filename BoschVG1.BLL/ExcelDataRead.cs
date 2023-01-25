using BoschVG1.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace BoschVG1.BLL
{
    public class ExcelDataRead
    {
        private enum FileExtensions : short
        {
            xlsx, xlsm
        }
        public async Task<List<PalletMatrix>> GetConvertedPalletExcelData(IFormFile uploadFile)
        {
            if (Array.Find(Enum.GetNames(typeof(FileExtensions)), x => x.Equals(Path.GetExtension(uploadFile.FileName).Remove(0, 1), StringComparison.OrdinalIgnoreCase)) == null)
            {
                throw new Exception($"File Declined: This upload only accepts the following formats: .xlsx or .xlsm");
            }


            var stream = uploadFile.OpenReadStream();
            var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            stream.Close();

            if (ms == null)
            {
                throw new Exception("Excel file cannot be empty");
            }

            using (ms)
            using (var package = new ExcelPackage(ms))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var sheet = package.Workbook.Worksheets.FirstOrDefault();

                if (sheet == null)
                {
                    throw new Exception($"File Declined: Cannot find excel sheet");
                }

                var rowHeader = 1;
                var palletmatrixdata = sheet.ConvertSheetToObjects<PalletMatrix>(uploadFile.FileName, rowHeader, true);
                //var data = sheet.ConvertSheetToObjects<NewPriceRequest>(uploadFile.FileName, rowHeader, true);
                var rowStart = 2;
                //data.ForEach(x => { x.RowNumberInExcel = rowStart++; x.Filename = sourcefilename; x.StartUpload = now; });
                return palletmatrixdata;
            }
        }
    }
}
