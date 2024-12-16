// System namespaces
global using System.Reflection;
global using System.Text.RegularExpressions;

// Microsoft namespaces
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Third-party libraries
global using Carter;
global using Mapster;
global using MassTransit;
global using MediatR;

// Project-specific namespaces
global using Stocks.API.Data;
global using Stocks.API.Exceptions;
global using Stocks.API.MessageBroker;
global using Stocks.API.Models;
global using Stocks.API.Repositories;
global using Stocks.API.Stock.Data;
global using Stocks.API.Stock.Events;
global using Stocks.API.Stock.Models;
global using Stocks.API.Stock.Repository;
global using Stocks.API.Stock.ValueObjects;