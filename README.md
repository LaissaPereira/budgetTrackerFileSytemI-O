# Budget Tracker

A simple personal budget tracker built with **.NET 10** and **.NET MAUI**. Add income and expense transactions, review them in a list, and generate a quick report showing total income, total expenses, and net balance. Every transaction added is also appended to a local log file for an auditable history.

## Project structure

```
BudgetTracker/
├── BudgetTracker.csproj        # Minimal console entry-point project (references Core)
├── Program.cs
├── BudgetTracker.Core/         # Platform-agnostic domain models & services
│   ├── Models/
│   │   ├── Transaction.cs
│   │   └── TransactionType.cs
│   ├── Events/
│   │   └── TransactionAdddedEventArgs.cs
│   └── Services/
│       ├── TransactionService.cs    # Validation + CRUD orchestration
│       ├── TransactionStorage.cs    # JSON file persistence
│       └── LoggerService.cs         # Appends a log line per transaction added
└── BudgetTracker.Maui/          # .NET MAUI app (UI)
    ├── MainPage.xaml(.cs)        # Transaction list + report panel
    ├── AddTransactionPage.xaml(.cs)
    ├── EditTransactionPage.xaml(.cs)
    └── Platforms/                # Android, iOS, MacCatalyst, Windows heads
```

- **BudgetTracker.Core** contains all business logic and has no UI dependencies, so it can be reused or unit tested independently of MAUI.
- **BudgetTracker.Maui** is the cross-platform UI (Android, iOS, Mac Catalyst, Windows) built on top of `BudgetTracker.Core`.
- Transaction data is persisted as JSON in the app's sandboxed `FileSystem.AppDataDirectory` (via `TransactionStorage`), so no user data is ever stored inside the repository.

## Features

- Add, edit, and remove income/expense transactions
- Input validation (amount must be positive, description required, valid date/type)
- Transaction list view with running total report (total income, total expenses, net balance)
- Append-only log file recording every transaction added

## Prerequisites

- [.NET SDK 10](https://dotnet.microsoft.com/download) or later
- .NET MAUI workload: `dotnet workload install maui`
- Platform-specific tooling for the target you want to build:
  - **iOS / Mac Catalyst**: Xcode (macOS only)
  - **Android**: Android SDK (installed automatically with the MAUI workload, or via Android Studio)
  - **Windows**: Windows 10/11 with the Windows App SDK (Windows only)

## Building & running

Restore the required MAUI workload once per machine:

```bash
dotnet workload install maui
```

### Run the MAUI app

From `BudgetTracker/BudgetTracker.Maui`:

```bash
# macOS / Mac Catalyst (runs as a native Mac app)
dotnet build -t:Run -f net10.0-maccatalyst

# iOS Simulator
dotnet build -t:Run -f net10.0-ios

# Android emulator/device
dotnet build -t:Run -f net10.0-android

# Windows
dotnet build -t:Run -f net10.0-windows10.0.19041.0
```

> **Note:** Building for the iOS Simulator requires `codesign` to succeed, which can fail on some macOS/Xcode combinations with a `resource fork, Finder information, or similar detritus not allowed` error. This is a known friction point between newer Xcode releases and the .NET iOS workload (see the `ValidateXcodeVersion` note in [`BudgetTracker.Maui.csproj`](BudgetTracker/BudgetTracker.Maui/BudgetTracker.Maui.csproj)). If you hit it, try `dotnet workload update` or match your Xcode version to the one expected by the installed MAUI workload.

### Build the Core library only

```bash
cd BudgetTracker/BudgetTracker.Core
dotnet build
```

## License

No license has been specified yet for this project.
