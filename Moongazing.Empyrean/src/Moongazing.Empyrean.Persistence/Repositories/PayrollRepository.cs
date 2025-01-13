using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;
using OfficeOpenXml;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class PayrollRepository : EfRepositoryBase<PayrollEntity, Guid, BaseDbContext>, IPayrollRepository
{
    public PayrollRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<ICollection<PayrollEntity>> GetPayrollCurrentMonthAsync(Guid employeeId)
    {
        var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        return await Query()
            .Where(p => p.EmployeeId == employeeId && p.PayDate >= startDate && p.PayDate <= endDate)
            .ToListAsync();
    }
    public async Task<ICollection<PayrollEntity>> GetPayrollsByDateRangeAsync(Guid employeeId, DateTime startDate, DateTime endDate)
    {
        return await Query()
            .Where(p => p.EmployeeId == employeeId && p.PayDate >= startDate && p.PayDate <= endDate)
            .ToListAsync();
    }

    public async Task<byte[]> ExportToExcelAsync(List<PayrollEntity> payrolls)
    {
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Payroll Report");

        worksheet.Cells[1, 1].Value = "Employee ID";
        worksheet.Cells[1, 2].Value = "Pay Date";
        worksheet.Cells[1, 3].Value = "Gross Salary";
        worksheet.Cells[1, 4].Value = "Net Salary";
        worksheet.Cells[1, 5].Value = "Tax Deduction";
        worksheet.Cells[1, 6].Value = "Bonus";

        for (int i = 0; i < payrolls.Count; i++)
        {
            var payroll = payrolls[i];
            worksheet.Cells[i + 2, 1].Value = payroll.EmployeeId.ToString();
            worksheet.Cells[i + 2, 2].Value = payroll.PayDate.ToShortDateString();
            worksheet.Cells[i + 2, 3].Value = payroll.GrossSalary;
            worksheet.Cells[i + 2, 4].Value = payroll.NetSalary;
            worksheet.Cells[i + 2, 5].Value = payroll.TaxDeduction;
            worksheet.Cells[i + 2, 6].Value = payroll.Bonus;
        }

        return await package.GetAsByteArrayAsync();
    }

    public async Task<byte[]> ExportToWordAsync(List<PayrollEntity> payrolls)
    {
        using var memoryStream = new MemoryStream();
        using (var wordDocument = WordprocessingDocument.Create(memoryStream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
        {
            var mainPart = wordDocument.AddMainDocumentPart();
            mainPart.Document = new Document();
            var body = mainPart.Document.AppendChild(new Body());

            var table = new Table();
            var tableHeader = new TableRow();

            tableHeader.Append(new TableCell(new Paragraph(new Run(new Text("Employee ID")))));
            tableHeader.Append(new TableCell(new Paragraph(new Run(new Text("Pay Date")))));
            tableHeader.Append(new TableCell(new Paragraph(new Run(new Text("Gross Salary")))));
            tableHeader.Append(new TableCell(new Paragraph(new Run(new Text("Net Salary")))));
            tableHeader.Append(new TableCell(new Paragraph(new Run(new Text("Tax Deduction")))));
            tableHeader.Append(new TableCell(new Paragraph(new Run(new Text("Bonus")))));

            table.Append(tableHeader);

            foreach (var payroll in payrolls)
            {
                var row = new TableRow();

                row.Append(new TableCell(new Paragraph(new Run(new Text(payroll.EmployeeId.ToString())))));
                row.Append(new TableCell(new Paragraph(new Run(new Text(payroll.PayDate.ToShortDateString())))));
                row.Append(new TableCell(new Paragraph(new Run(new Text(payroll.GrossSalary.ToString())))));
                row.Append(new TableCell(new Paragraph(new Run(new Text(payroll.NetSalary.ToString())))));
                row.Append(new TableCell(new Paragraph(new Run(new Text(payroll.TaxDeduction.ToString())))));
                row.Append(new TableCell(new Paragraph(new Run(new Text(payroll.Bonus.ToString())))));

                table.Append(row);
            }

            body.Append(table);
        }

        return await Task.FromResult(memoryStream.ToArray());
    }
}