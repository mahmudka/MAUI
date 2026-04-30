using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AtestareTicket.Services;

public class WordTicketService
{
    private const string DocxPath = @"C:\Users\ahram\OneDrive\Documents\Программирование_в_сети_5-9.docx";

    private static readonly Lazy<Dictionary<int, List<string>>> _tickets =
        new(ParseTickets);

    public List<string> GetQuestions(int ticketNumber)
    {
        if (_tickets.Value.TryGetValue(ticketNumber, out var questions))
            return questions;
        return [];
    }

    public int TicketCount => _tickets.Value.Count;

    private static Dictionary<int, List<string>> ParseTickets()
    {
        using var zip = ZipFile.OpenRead(DocxPath);
        var entry = zip.Entries.First(e => e.FullName == "word/document.xml");
        using var stream = entry.Open();
        var xdoc = XDocument.Load(stream);

        XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        var paragraphs = xdoc.Descendants(w + "p")
            .Select(p => string.Concat(p.Descendants(w + "t").Select(t => (string?)t ?? "")))
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();

        var tickets = new Dictionary<int, List<string>>();
        int currentTicket = 0;

        foreach (var line in paragraphs)
        {
            var ticketMatch = Regex.Match(line.Trim(), @"^Билет\s+(\d+)$");
            if (ticketMatch.Success)
            {
                currentTicket = int.Parse(ticketMatch.Groups[1].Value);
                tickets[currentTicket] = [];
            }
            else if (currentTicket > 0 && Regex.IsMatch(line.Trim(), @"^\d+\."))
            {
                tickets[currentTicket].Add(line.Trim());
            }
        }

        return tickets;
    }
}
