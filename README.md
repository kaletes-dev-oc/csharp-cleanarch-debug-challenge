# C# Clean Architecture Debug Challenge

Small interview exercise for a C# developer.

## Goal

The candidate should:
1. Clone the repo
2. Run tests / console app
3. Find and fix one logical bug in the application layer

Target solve time: ***.

## Tech

- .NET 8
- Clean architecture style layering:
  - `Domain`
  - `Application`
  - `Infrastructure`
  - `ConsoleApp`
  - `Application.Tests`

## Run

```bash
dotnet test
```

```bash
dotnet run --project src/DebugChallenge.ConsoleApp
```

### Console sample input

Cart lines:

```text
KB-01:1,MS-02:2,KB-01:1
```

Coupon:

```text
SAVE10
```

## What interviewer can ask

- Why does one test fail?
- Where is the bug located in clean architecture terms?
- What is the safest fix?
- Would you add another test?

## Note for interviewer

There is intentionally **one business-logic bug** (not a syntax or dependency issue).
