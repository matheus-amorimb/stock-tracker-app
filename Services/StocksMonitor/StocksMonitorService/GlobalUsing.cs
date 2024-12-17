// System namespaces
global using System.Net;
global using System.Text.Json;
global using System.Text.Json.Serialization;

// Third-party libraries
global using Quartz;
global using MassTransit;
global using MediatR;
global using StackExchange.Redis;

// Data-related namespaces
global using StocksMonitorService.Data;
global using StocksMonitorService.Data.Cache;

// Message Broker-related namespaces
global using StocksMonitorService.MessageBroker;

// Stocks-related namespaces
global using StocksMonitorService;
global using StocksMonitorService.Stocks.Events.Contracts;
global using StocksMonitorService.Stocks.Events.Consumers;
global using StocksMonitorService.Stocks.Events.Handlers;
global using StocksMonitorService.Stocks.Services;
global using StocksMonitorService.Stocks.Types;
global using StocksMonitorService.Stocks.Types.AlphaVantage;
global using StocksMonitorService.Stocks.Workers;
