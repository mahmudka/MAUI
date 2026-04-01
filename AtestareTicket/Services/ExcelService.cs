using ClosedXML.Excel;

namespace AtestareTicket.Services;

public class ExcelService
{
    private const string ExcelFilePath = @"D:\Atestare Gruppe.xlsx";

    /// <summary>
    /// Returns all sheet names from the workbook — each sheet = one student group.
    /// </summary>
    public List<string> GetGroupNames()
    {
        using var workbook = new XLWorkbook(ExcelFilePath);
        return workbook.Worksheets.Select(ws => ws.Name).ToList();
    }

    /// <summary>
    /// Returns student names (column B) for the given sheet, starting at row 10,
    /// stopping at the first empty cell.
    /// </summary>
    public List<string> GetStudentNames(string groupName)
    {
        using var workbook = new XLWorkbook(ExcelFilePath);
        var worksheet = workbook.Worksheet(groupName);

        var names = new List<string>();
        int row = 10; // data starts at row 10

        while (true)
        {
            var cell = worksheet.Cell(row, 2); // Column B = index 2
            var value = cell.IsEmpty() ? string.Empty : cell.GetString().Trim();

            if (string.IsNullOrWhiteSpace(value))
                break;

            names.Add(value);
            row++;
        }

        return names;
    }

    /// <summary>
    /// Appends a ticket record to "Atestation May.xlsx" on drive D.
    /// Creates the file with headers if it does not exist yet.
    /// </summary>
    public void SaveTicketRecord(string group, string student, int ticketNumber)
    {
        const string outputPath = @"D:\Atestation May.xlsx";
        XLWorkbook workbook;
        IXLWorksheet worksheet;

        if (File.Exists(outputPath))
        {
            workbook = new XLWorkbook(outputPath);
            worksheet = workbook.Worksheets.First();
        }
        else
        {
            workbook = new XLWorkbook();
            worksheet = workbook.Worksheets.Add("Atestation");

            // Header row
            worksheet.Cell(1, 1).Value = "Group";
            worksheet.Cell(1, 2).Value = "Student";
            worksheet.Cell(1, 3).Value = "Ticket Number";

            // Style headers
            var headerRange = worksheet.Range(1, 1, 1, 3);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#0D2B5E");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }

        // Find next empty data row (skip header)
        int nextRow = worksheet.LastRowUsed()?.RowNumber() + 1 ?? 2;

        worksheet.Cell(nextRow, 1).Value = group;
        worksheet.Cell(nextRow, 2).Value = student;
        worksheet.Cell(nextRow, 3).Value = ticketNumber;

        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        workbook.SaveAs(outputPath);
        workbook.Dispose();
    }
}
