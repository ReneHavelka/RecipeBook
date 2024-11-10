# Recipe Book

Aplikácia je ukážkou tvorby programového riešenia s UI na báze ASP.NET Core, Blazor, Server-Side rendering.  BE využíva framework .NET  Core a Entity Framework Core.
(Po dokončení bude môcť byť použitá ako pomocník pre populárne hoby - varenie, ako kniha zozbieraných receptov.)

Pozostáva zo štyroch projektov, vrstiev – Domain, Application, Infrastructure a BlazorApp (UI), rešpektuje princípy SOLID a využíva paradigmu Clean Architecture.

Pre testovanie je použitý framework MSTest.
Z dôvodov odporúčaní developerov z Microsoft-u a ich argumentácie je pre testovanie použitá konkrétna databáza, nie mocking, prípadne iná improvizácia databázy.
