# NET-Core.Library.Domain

## Overview

This is project containing the domain contexts, repositories, unit of work implementations. 

## Project Structure

| Directory Name | Usage |
|----------------|-------|
| DBModels | Application's Domain classes (POCO) |
| Infrastructure | Application DB context class, Generic Repository Pattern & Unit of Work pattern implementor classes |
| Infrastructure\Contracts | Generic Repository Pattern, Unit of Work pattern and other abstraction definitions |
| Infrastructure\EFCore | Entity Framework Core specific |