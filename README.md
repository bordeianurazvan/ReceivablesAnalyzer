# ReceivablesAnalyzer
A basic set of HTTP APIs used for ingesting and analyzing receivables from prospective clients to help our organization decide if we should lend to a business, and how much we are comfortable providing.
Receivables are debts owed to a company for goods or services which have been provided but not yet paid for. Invoices and credit notes can be considered types of receivables.

## Project instalation and run
- clone repository
- run update-database in the Package Manager Console (PMC):
    - make sure you setup Ingestion API as startup project and select Ingestion.Infrastructure as Default Project in PMC.
- make sure you have same connection string added in the appsettings.json, in the DefaultConnection for each project
- configure startup projects in VS: select multiple startup project and for Ingestion.API and Analysis.API select "Start without debugging". 
 
## Key Features
- Ingestion Microservice used for ingesting and storing Invoices and Credit Notes.
- Analysis Microservice used for summary statistics about the stored Invoices and Credit Notes.

## Tehnical Stack
- .NET 6
- Entity Framework Core
- SQL Server
- xUnit

## Dependencies
- Swashbuckle Swagger
- Automapper
- Fluent Results
- Z.EntityFramework.Extensions.EFCore: BulkInsertAsync
- Moq
- MockQueryable.Moq

## Solution
Structuring solution is crucial for maintainability, scalability, and ease of development, so I went for:
- Microservice Architecture
- Clean Arhitecture: 
    - Independent Layers: Application, Domain, Infrastructure, and Presentation
    - Dependency Flow: dependency flow is designed to move from the outer layers to the inner layers
    - Separation of Concerns: each layer has a specific responsibility
- Domain Driver Design
    - Domain Layer: the "Domain" layer contains entities and repositories.
    - Repositories: aligns with DDD practices, where repositories define how the application interacts with the data stored
- Database per service: used as starting idea, currently microservices use a shared database

### Assumptions
- I considered Invoice and Credit Note as separate entities, even if in the code their properites are the same
- The code is duplicated between Invoice and Credit Note
- For Ingestion Microservice, unit tests for GetByReference, Update and Delete methods are skipped because will be similar with the one for GetAll and Post and they are out of the scope of the ingestion microservice as it's mentioned in the homework specifications.
- For Analysis Microservice, the endpoint which exposes summary data do a calculation of the OpeningValue for Invoice/CreditNote queried based on the input (closed, open, for a corresponding range using start date and end date.)

### Notes:
- In a strict sense, having all the microservices in a single repository doesn't align with the typical distributed nature associated with microservices architecture. The essence of microservices architecture lies in the independent deployment, scalability, and maintenance of individual services, often managed by separate teams. Each microservice typically has its own codebase, deployment pipeline, and data store.
However, for the purposes of this homework project, I choose to have all the microservices in a single repository. This approach simplifies the management of the project, it allows anyone to review the entire codebase easily without dealing with multiple repositories.
In real-world scenarios, especially in larger applications or production environments, is more likely to have separate repositories for each microservice.

- A Monolithic architecture is often simpler to develop and maybe it would have been more appropriate for this small homework, but considering the brief description: "write a set of HTTP APIs", but also the fact that key features can be very easily divided into separate services I went for Microservice Architecture.

- I started the solution considering Database Per Service in mind, but I didn't succeded to complete the replication process between these databases (one for each microservice),
until the due date of the homework, so that's why microservices share same database.
    - Shared Database Pros:
        - Data Consistency: data consistency across microservices is easily achievable since they are all reading and writing to the same data store.
    - Shared Database Cons:
        - Schema Coupling: changes to the database schema or data model can impact multiple microservices, leading to tight coupling.
        - Scaling Challenges: any scaling becomes more challenging as all microservices need to scale together.

- The initial plan I had, had a mechanism for database replication using Event Driven Design. I planned to use MassTranzit framework and RabbitMQ, to link the Ingestion DB to
Analysis DB, so each time a new Invoice/CreditNote is added, updated, deleted from the Ingestion DB, new event to be raised, this to arrive in the RabbitMQ queue and from there
to be consumed by the Analysis Microservice and synchronize DB accordly.

### Schema
```
1. Ingestion Microservice/
   |-- Presentation/
   |   |-- Controllers/
   |   |   |-- InvoiceController
   |   |   |-- CreditNoteController
   |-- Application/
   |   |-- Dtos/
   |   |   |-- InvoiceDto
   |   |   |-- CreditNoteDto
   |   |-- Services/
   |   |   |-- IInvoiceService
   |   |   |-- InvoiceService
   |   |   |-- ICreditNoteService
   |   |   |-- CreditNoteService
   |   |-- Validators/
   |   |   |-- DateTimeValidationAttribute
   |-- Domain/
   |   |-- Entities/
   |   |   |-- Invoice
   |   |   |-- CreditNote 
   |   |-- Configurations/
   |   |   |-- CreditNoteConfiguration
   |   |   |-- InvoiceConfiguration
   |   |-- RepositoryInterfaces/
   |   |   |-- IGenericRepository
   |   |   |-- IInvoiceRepository
   |   |   |-- ICreditNoteRepository
   |-- Infrastructure/
   |   |-- Migrations/
   |   |   |-- initialCreate
   |   |-- Repositories/
   |   |   |-- InvoiceRepository
   |   |   |-- CreditNoteRepository
   |-- Tests/
   |   |-- Integrations/
   |   |   |-- IntegrationTests
   |   |-- UnitTests/
   |   |   |-- Controllers
   |   |   |-- Services
```

#### Explanation:
- Application: services responsible for coordinating actions within the application. For example, Getting, Adding, Updating, Deleting receivables.
- Domain: defines domain entities: Invoice, CreditNote, interfaces for repositories and configurations for entities.
- Infrastructure: implements concrete repositories and database-related components.
- Presentation:  handles the HTTP request-response cycle. Invoice/CreditNote controller receives HTTP requests related to ingesting receivables.
```   
2. Summary Microservice/
   |-- Presentation/
   |   |-- Controllers/
   |   |   |-- InvoiceController
   |   |   |-- CreditNoteController
   |-- Application/
   |   |-- Models/
   |   |   |-- SummaryInvoice
   |   |   |-- SummaryCreditNote
   |   |-- Services/
   |   |   |-- IInvoiceService
   |   |   |-- InvoiceService
   |   |   |-- ICreditNoteService
   |   |   |-- CreditNoteService
   |   |-- Validators/
   |   |   |-- DateTimeValidationAttribute
   |-- Domain/
   |   |-- Entities/
   |   |   |-- Invoice
   |   |   |-- CreditNote 
   |   |-- Configurations/
   |   |   |-- CreditNoteConfiguration
   |   |   |-- InvoiceConfiguration
   |   |-- RepositoryInterfaces/
   |   |   |-- IGenericRepository
   |   |   |-- IInvoiceRepository
   |   |   |-- ICreditNoteRepository
   |-- Infrastructure/
   |   |-- Migrations/
   |   |   |-- initialCreate
   |   |-- Repositories/
   |   |   |-- InvoiceRepository
   |   |   |-- CreditNoteRepository
   |-- Tests/
   |   |-- Integrations/
   |   |   |-- IntegrationTests
   |   |-- UnitTests/
   |   |   |-- Controllers
   |   |   |-- Services
```
#### Explanation:
- Application: services responsible for coordinating actions within the application. For example exposing summary statistics about receivables.
- Domain: defines domain entities: Invoice, CreditNote, interfaces for repositories and configurations for entities.
- Infrastructure: implements concrete repositories and database-related components.
- Presentation: handles the HTTP request-response cycle. Invoice/CreditNote controller receives HTTP requests related to expose receivables data and summary statistics.

### Duration
- Arhitecture reasearch and desing - 1-2h
- Solution and place holder projects - 30min
- Ingestion Microservice - 8-10h (2h adding unit/integration tests)
- Analysis Microservice - 6-8h (1-2h adding unit/integration tests)
- Documentation 1-2h 
