# 🎟 AtestareTicket — Exam Ticket Generator

> A .NET MAUI desktop application for the Faculty of Computers, Informatics and Microelectronics (FCIM) at the Technical University of Moldova. It randomly assigns attestation exam tickets to students and displays the corresponding questions instantly.

---

## ✨ Features

- **Random ticket generation** — each student receives a unique ticket number drawn from the official question bank
- **Live question display** — questions are read directly from the Word document (`.docx`) and shown in a styled popup, no manual copy-paste needed
- **Student registry** — loads academic groups and student names from an Excel workbook
- **Automatic record keeping** — every generated ticket is saved to an output Excel file with the group, student name, and ticket number
- **Fully Russian UI** — all interface labels, hints, and controls are in Russian; FCIM branding is preserved

---

## 🖥 Screenshots

| Main Screen | Ticket Popup |
|---|---|
| Select group → select name → generate | Ticket number + 3 exam questions displayed |

---

## 🗂 Project Structure

```
AtestareTicket/
├── MainPage.xaml / .cs          # Main UI — group & name pickers, generate button
├── TicketPopupPage.xaml / .cs   # Custom modal popup showing ticket & questions
├── Services/
│   ├── ExcelService.cs          # Reads student groups/names; writes ticket records
│   └── WordTicketService.cs     # Parses exam questions from the .docx file
├── Resources/
│   ├── Images/                  # FCIM logo and app assets
│   └── Styles/                  # App-wide colors and styles
└── Platforms/Windows/           # Windows-specific entry point & manifest
```

---

## ⚙️ Prerequisites

| Requirement | Version |
|---|---|
| .NET SDK | 9.0+ |
| .NET MAUI workload | 9.0+ |
| Windows | 10 (build 19041+) |
| Visual Studio | 2022 17.8+ |

---

## 📁 Required Data Files

| File | Path | Purpose |
|---|---|---|
| Student registry | `C:\UTM\Atestare\Atestare Gruppe.xlsx` | Excel workbook — each sheet is one academic group, student names start at row 10 column B |
| Question bank | `C:\Users\...\Documents\Программирование_в_сети_5-9.docx` | Word document containing tickets in the format **Билет N** followed by 3 numbered questions |
| Output log | `C:\UTM\Atestation May.xlsx` | Created automatically on first run |

---

## 🚀 Running the App

### Via VS Code task
Open the Command Palette → **Run Task** → **Run MAUI Windows**

### Via terminal
```powershell
dotnet build AtestareTicket/AtestareTicket.csproj -f net9.0-windows10.0.19041.0 -c Debug
dotnet run --project AtestareTicket/AtestareTicket.csproj -f net9.0-windows10.0.19041.0
```

---

## 🔄 How It Works

```
1. App starts  →  ExcelService loads academic groups from Excel workbook
2. Student selects group  →  student names for that group are loaded
3. Student selects name  →  Generate button becomes active
4. Generate clicked  →  random ticket number is drawn (1 – N)
5. Record saved  →  group, name, ticket number written to output Excel
6. Popup shown  →  WordTicketService fetches the 3 questions for that ticket
                    and displays them in a styled modal dialog
7. Student closes popup  →  UI resets for the next student
```

---

## 📦 NuGet Dependencies

| Package | Purpose |
|---|---|
| `ClosedXML` | Reading and writing Excel files |
| `Microsoft.Maui.Controls` | MAUI UI framework |

---

## 🏛 About

Built for **FCIM · UTM** — Facultatea Calculatoare, Informatică și Microelectronică  
Universitatea Tehnică a Moldovei · [fcim.utm.md](https://fcim.utm.md)  
str. Studenților 9/7, Chișinău, Republic of Moldova
